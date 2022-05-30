#include "entitydata.h"

EntityData::EntityData()
{
    entityType = type::invalid;
}

EntityData::EntityData(QString n, QPointF c, type t)
{
    name = n;
    coords = c;
    entityType = t;
}

QString EntityData::shortDescription() const
{
    return name;
}

QString EntityData::fullDescription() const
{
    QString result = name;
    for(QMap<QString,QString>::const_iterator it = additionalFields.cbegin(); it!=additionalFields.cend();++it)
    {
        result+="\n";
        result+=it.value();
    }
    return result;
}
