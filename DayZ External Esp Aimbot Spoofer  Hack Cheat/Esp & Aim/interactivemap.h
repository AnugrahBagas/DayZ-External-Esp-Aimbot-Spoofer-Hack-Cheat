#ifndef INTERACTIVEMAP_H
#define INTERACTIVEMAP_H

#include <QObject>
#include <QWidget>
#include <QMouseEvent>
#include <QWheelEvent>
#include <QPixmap>
#include <QPainter>
#include <QSettings>
#include <QMap>
#include <QList>
#include <QPicture>
#include <QMutex>
#include <QtConcurrent>
#include <QVector>
#include <QImage>
#include "worldstate.h"

class CloseObjects
{
public:
    CloseObjects() {}
    CloseObjects(EntityRange *r, QPointF p): range(r), coords(p) {}
    QString findCloseObjects() const;
private:
    EntityRange*    range;
    QPointF         coords;
};

class InteractiveMap : public QWidget
{
    Q_OBJECT

public:
    InteractiveMap(QWidget* pwgt = nullptr);
    virtual ~InteractiveMap();

protected:
    virtual void paintEvent(QPaintEvent* pe);

private:
    const float minScale = 1.0f;
    const float maxScale = 8.0f;
    const float scaleStep= 2.0f;

    void updateScale(const qreal value, const QPointF& dpos);
    void updateTranslate(const QPointF& value);

    bool getFilterValue(EntityData::type t);
    bool getFilterValue(QString t);

    void mousePressEvent  (QMouseEvent* pe);
    void mouseMoveEvent   (QMouseEvent* pe);
    void wheelEvent       (QWheelEvent *pe);

    void findCloseObjects(QPointF coords);
    QVector<CloseObjects>* input;

    QPainter*   painter;
    QPixmap*    image;
    WorldState* worldState;

    qreal scale;
    QPointF translate;
    QPoint startMove;

    QPixmap cache;

    QMutex renderMutex;
    QFutureWatcher<QString> closeObjWatcher;
    QFuture<QString> closeObjFuture;

public slots:
    void loadState(QString stateFile);
    void loadDump(QString dumpFile, QString idxFile);
    void closeState();
    void saveState(QString stateFile);
    void updateCache();
    void sendCloseObjects();

signals:
    void showCloseObjects(QString str);
    void saveStateChanged(bool state);
};

#endif // INTERACTIVEMAP_H
