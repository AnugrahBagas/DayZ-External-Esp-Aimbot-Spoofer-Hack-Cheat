#ifndef ENTITYDATA_H
#define ENTITYDATA_H

#include <QString>
#include <QMap>
#include <QPointF>

class WorldState;

class EntityData
{
public:
    friend class WorldState;
    enum class type {airplane, car, motorcycle, ship, helicopter, parachute, tank,
                     tent, stash, fence, ammoBox, campFire, crashSite, animals,
                     players, zombies, stuff, hedgehog, invalid};

    type entityType;

    EntityData();
    EntityData(QString n, QPointF c, type t = type::stuff);

    QString shortDescription()const;
    QString fullDescription()const;
    QPointF getCoords() const {return coords;}
private:
    QString name;
    QPointF coords;
    QMap<QString, QString> additionalFields;
};
#endif // ENTITYDATA_H
