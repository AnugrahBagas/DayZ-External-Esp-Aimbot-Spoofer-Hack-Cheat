#include "interactivemap.h"
#include "settingsmanager.h"
#include <QDebug>
#include <QtGlobal>
#include <QMap>
#include <QtConcurrent>
#include <QTextStream>

InteractiveMap::InteractiveMap(QWidget* pwgt) : QWidget(pwgt)
{
    painter = new QPainter();
    image   = new QPixmap(":/res/bigmap.jpg");

    scale = 1;

    setUpdatesEnabled(false);

    QPalette pal;
    pal.setColor(QPalette::Background,QColor(Qt::GlobalColor::black));
    setPalette(pal);
    setAutoFillBackground(true);

    worldState = nullptr;
    input = nullptr;
}

InteractiveMap::~InteractiveMap()
{
    delete worldState;
    delete painter;
    delete image;
}

void InteractiveMap::paintEvent(QPaintEvent *pe)
{
    renderMutex.lock();
    painter->begin(this);
//////////////////////////////////////////////////
    QTransform mat;
    painter->setTransform(mat);
    painter->scale(scale, scale);
    painter->translate(translate);
    painter->drawPixmap(0,0, *image);

    if(cache.isNull())
    {
        cache = QPixmap(image->size()*4);
        cache.setDevicePixelRatio(4);
        cache.fill(Qt::transparent);
        QPainter cachePaint(&cache);
        for(QMap<EntityData::type, EntityRange>::const_iterator it = worldState->entityRanges.cbegin(); it!=worldState->entityRanges.cend();++it)
        {
            if(getFilterValue(it.key()))
            {
                for(QVector<EntityData>::const_iterator i = it.value().start; i!= it.value().end; ++i)
                {
                    float x = i->getCoords().x();
                    float y = i->getCoords().y();
                    x = (((x) / (15360.0f / 975.0f)));
                    y = (((15360.0f - y) / (15360.0f / 970.0f)) - 4.0f);

                    QFont font("Arial");
                    QPen  pen;
                    pen.setWidthF(4.0f/scale);
                    pen.setStyle(Qt::SolidLine);
                    font.setPointSizeF(qMax(float(8.0f*1.0f/scale),2.0f));
                    cachePaint.setFont(font);
                    cachePaint.setPen(pen);

                    if(getFilterValue(QString("name")))
                        cachePaint.drawText(x,y,i->shortDescription());
                    cachePaint.drawPoint(x,y);
                }
            }
        }
    }
    painter->drawPixmap(0,0,cache);
//////////////////////////////////////////////////
    painter->end();
    renderMutex.unlock();
}

void InteractiveMap::updateScale(qreal value, const QPointF& dpos)
{
    qreal newScale = scale * value;
    if(newScale >= minScale && newScale <= maxScale)
    {
        scale = newScale;
        translate += dpos/scale;
        updateCache();
    }
}

void InteractiveMap::updateTranslate(const QPointF& value)
{
    QPointF newV = translate + (value * 1/scale);
    translate = newV;
    update();
}

void InteractiveMap::mousePressEvent(QMouseEvent *pe)
{
    if(pe->buttons() & Qt::LeftButton)
        startMove = pe->pos();
    else if(pe->buttons() & Qt::MidButton)
    {
        if(worldState)
        {
            QPointF pos = pe->pos()/scale - translate;
            if(pos.x() >= 0.0f && pos.x() <= image->width() && pos.y() >= 0.0f && pos.y() <= image->height())
            {
                pos.rx() = pos.x() * (15360.0f / 975.0f);
                pos.ry() = -((15360.0f/970.0f)*(pos.y()+4.0f)-15360.0f);
                findCloseObjects(pos);
            }
        }

    }
}

void InteractiveMap::mouseMoveEvent(QMouseEvent *pe)
{
    if(pe->buttons() & Qt::LeftButton)
    {
        updateTranslate(pe->pos() - startMove);
        startMove = pe->pos();
    }
}

void InteractiveMap::wheelEvent(QWheelEvent *pe)
{
    float dScale = (pe->angleDelta().y() < 0) ? 1/scaleStep : scaleStep;

    QPointF nPos = pe->pos() * (dScale);
    QPointF dPos = pe->pos() - nPos;

    updateScale(dScale,dPos);
}

bool InteractiveMap::getFilterValue(EntityData::type t)
{
    return SettingsManager::instance().value(QVariant(static_cast<int>(t)).toString(), false).toBool();
}

bool InteractiveMap::getFilterValue(QString t)
{
    return SettingsManager::instance().value(t, false).toBool();
}

void InteractiveMap::updateCache()
{
    cache = QPixmap();
    update();
}

void InteractiveMap::loadDump(QString dumpFile, QString idxFile)
{
    setUpdatesEnabled(false);
    renderMutex.lock();

    delete worldState;
    worldState = nullptr;
    worldState = new WorldState(dumpFile, idxFile);

    renderMutex.unlock();
    setUpdatesEnabled(true);
    updateCache();
    emit saveStateChanged(true);
}

void InteractiveMap::loadState(QString stateFile)
{
    setUpdatesEnabled(false);
    renderMutex.lock();

    delete worldState;
    worldState = nullptr;
    worldState = new WorldState(stateFile);

    renderMutex.unlock();
    setUpdatesEnabled(true);
    updateCache();
    emit saveStateChanged(true);
}

void InteractiveMap::saveState(QString stateFile)
{
    worldState->saveState(stateFile);
}

void InteractiveMap::closeState()
{
    setUpdatesEnabled(false);
    renderMutex.lock();
    closeObjWatcher.waitForFinished();

    delete worldState;
    worldState = nullptr;

    cache = QPixmap();

    renderMutex.unlock();

    emit saveStateChanged(false);
}

void addToAnswer(QString& result, const QString& interm)
{
    if(!interm.isEmpty())
        result += interm;
}

void InteractiveMap::findCloseObjects(QPointF coords)
{
    if(!closeObjWatcher.isRunning())
    {
        input = new QVector<CloseObjects>;
        for(QMap<EntityData::type, EntityRange>::iterator it = worldState->entityRanges.begin(); it!=worldState->entityRanges.end();++it)
        {
            if(getFilterValue(it.key()))
            {
                CloseObjects obj(&it.value(), coords);
                input->append(obj);
            }
        }
        closeObjFuture = QtConcurrent::mappedReduced(*input, &CloseObjects::findCloseObjects, addToAnswer);

        connect(&closeObjWatcher, &QFutureWatcher<QString>::finished, this, &InteractiveMap::sendCloseObjects);
        closeObjWatcher.setFuture(closeObjFuture);
    }
}

void InteractiveMap::sendCloseObjects()
{
    emit showCloseObjects(closeObjWatcher.result());
    delete input;
    input = nullptr;
}

QString CloseObjects::findCloseObjects() const
{
    QString result;
    QTextStream stream(&result);
    stream.setRealNumberNotation(QTextStream::FixedNotation);
    stream.setRealNumberPrecision(2);
    for(QVector<EntityData>::const_iterator it = range->start; it != range->end; ++it)
    {
        float len = qSqrt(qPow((it->getCoords().x() - coords.x()),2) + qPow((it->getCoords().y() - coords.y()),2));
        if(len <= 350)
        {
            stream << it->fullDescription() << "\n" << QVariant(it->getCoords().x()/100).toFloat() << " " << QVariant((15360 - it->getCoords().y())/100).toFloat() << "\n";
        }
    }
    return result;
}
