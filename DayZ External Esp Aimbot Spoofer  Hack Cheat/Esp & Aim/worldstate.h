#ifndef WORLDSTATE_H
#define WORLDSTATE_H

#include <QObject>
#include <QVector>
#include <QMap>
#include <QtXml>
#include "memoryapi.h"
#include "entitydata.h"

class EntityRange
{
public:
    EntityRange(): start(0), end(0) {}
    EntityRange(const QVector<EntityData>::const_iterator& s, const QVector<EntityData>::const_iterator& e): start(s), end(e){}
    QVector<EntityData>::const_iterator start;
    QVector<EntityData>::const_iterator end;
};

class WorldState
{
public:
    WorldState(const QString& dumpFile, const QString& idxFile);
    WorldState(const QString& stateFile);

    void saveState(const QString& stateFile);

    QMap <EntityData::type, EntityRange> entityRanges;
    QString worldName;
private:
    QVector <EntityData> entityArray;
    QVector<quint32> masterOffsets;
    QVector<quint32> tableOffsets;
    quint32          objTableAddress;
    void handleEntity   (quint32 entityAddress, MemoryAPI& mem);
    void initRanges();
    void initOffsets();

    QDomElement makeElement(QDomDocument& domDoc, const QString& name, const QString& strData = QString());
};

#endif // WORLDSTATE_H
