#include "worldstate.h"
#include <QDebug>
#include <algorithm>
#include <QCoreApplication>
#include <QProgressDialog>
#include <QWidget>
#include <QtXml>

WorldState::WorldState(const QString& dumpFile, const QString& idxFile)
{
    initOffsets();

    QProgressDialog progress;
    progress.setCancelButton(nullptr);
    progress.setLabelText(QStringLiteral("Loading dump..."));
    progress.setModal(true);
    progress.setMinimum(0);

    progress.setMaximum(masterOffsets.length()+2);
    progress.show();

    MemoryAPI mem(dumpFile,idxFile);
    progress.setValue(1);

    for(auto mO = masterOffsets.begin(); mO != masterOffsets.end(); ++mO)
    {
        quint32 entityTableBasePtr = mem.readPtr(objTableAddress) + (*mO);
        for(auto tO = tableOffsets.begin(); tO != tableOffsets.end(); ++tO)
        {
            qint32 size = mem.readInt(entityTableBasePtr + 0x4 +(*tO));

            for(qint32 i = 0; i!=size; ++i)
            {
                quint32 fPtr = mem.readPtr(entityTableBasePtr + (*tO));
                quint32 entityAddress = mem.readPtr(fPtr + 4 * i);

                handleEntity(entityAddress, mem);

                QCoreApplication::processEvents();
            }
        }
        progress.setValue(progress.value()+1);
    }
    initRanges();
    worldName = QStringLiteral("chernarus");
    progress.setValue(progress.value()+1);
}

WorldState::WorldState(const QString &stateFile)
{
    initOffsets();

    QProgressDialog progress;
    progress.setCancelButton(nullptr);
    progress.setLabelText(QStringLiteral("Loading state..."));
    progress.setModal(true);
    progress.setMinimum(0);

    progress.setMaximum(masterOffsets.length()+2);
    progress.show();

    QDomDocument domDoc;
    QFile        file(stateFile);

    if(file.open(QIODevice::ReadOnly)) {
        if(domDoc.setContent(&file)) {
            QDomElement rootElement = domDoc.documentElement();
            worldName = rootElement.attribute(QStringLiteral("name"), QStringLiteral("chernarus"));
            progress.setMaximum(rootElement.childNodes().count());
            //Iterate entities
            for(QDomNode domNode = rootElement.firstChild(); !domNode.isNull(); domNode = domNode.nextSibling())
            {
                if(domNode.isElement()) {
                    EntityData ent;
                    QDomElement entity = domNode.toElement();
                    for(QDomNode entNode = entity.firstChild(); !entNode.isNull(); entNode = entNode.nextSibling())
                    {
                        if(entNode.isElement())
                        {
                            QDomElement entElem = entNode.toElement();
                            if(entElem.tagName() == QStringLiteral("name"))
                            {
                                ent.name = entElem.text();
                            }else if(entElem.tagName() == QStringLiteral("type"))
                            {
                                ent.entityType = static_cast<EntityData::type>(entElem.text().toInt());
                            }
                            else if(entElem.tagName() == QStringLiteral("coords"))
                            {
                                for(QDomElement entText = entElem.firstChild().toElement(); !entText.isNull(); entText = entText.nextSibling().toElement())
                                {
                                    if(entText.tagName() == QStringLiteral("x"))
                                        ent.coords.setX(entText.text().toFloat());
                                    else if(entText.tagName() == QStringLiteral("y"))
                                        ent.coords.setY(entText.text().toFloat());
                                }
                            }else if(entElem.tagName() == QStringLiteral("options"))
                            {
                                for(QDomElement entText = entElem.firstChild().toElement(); !entText.isNull(); entText = entText.nextSibling().toElement())
                                {
                                    ent.additionalFields[entText.tagName()] = entText.text();
                                }
                            }
                        }
                        QCoreApplication::processEvents();
                    }
                    entityArray.append(ent);
                }
                progress.setValue(progress.value()+1);
            }

        }
        file.close();
        initRanges();
    }
}

void WorldState::initOffsets()
{
    masterOffsets.append(0x880);
    masterOffsets.append(0xb24);
    masterOffsets.append(0xdc8);

    tableOffsets.append(0x8);
    tableOffsets.append(0xb0);
    tableOffsets.append(0x158);
    tableOffsets.append(0x200);

    objTableAddress = 0xDAD8C0;
}

void WorldState::saveState(const QString &stateFile)
{
    QDomDocument state(QStringLiteral("MapHackWorldState"));
    QDomElement world = makeElement(state, QStringLiteral("world"));
    world.setAttribute(QStringLiteral("name"), QStringLiteral("chernarus"));
    state.appendChild(world);

    for(QVector<EntityData>::const_iterator it = entityArray.cbegin(); it != entityArray.cend(); ++it)
    {
        QDomElement entity = makeElement(state, "entity");
        entity.appendChild(makeElement(state,   "name", it->shortDescription()));
        entity.appendChild(makeElement(state,   "type", QVariant(static_cast<int>(it->entityType)).toString()));
        QDomElement coords = makeElement(state, "coords");
        coords.appendChild(makeElement(state,   "x", QVariant(it->coords.x()).toString()));
        coords.appendChild(makeElement(state,   "y", QVariant(it->coords.y()).toString()));
        entity.appendChild(coords);

        if(!it->additionalFields.isEmpty())
        {
            QDomElement options = makeElement(state, QStringLiteral("options"));
            for(QMap<QString,QString>::const_iterator i = it->additionalFields.cbegin(); i!=it->additionalFields.cend(); ++i)
            {
                options.appendChild(makeElement(state, i.key(), i.value()));
            }
            entity.appendChild(options);
        }

        world.appendChild(entity);
    }

    QFile file(stateFile);
    if(file.open(QIODevice::WriteOnly))
    {
        QTextStream(&file) << state.toString();
        file.close();
    }
}

QDomElement WorldState::makeElement(QDomDocument &domDoc, const QString &name, const QString &strData)
{
    QDomElement domElement = domDoc.createElement(name);
    if(!strData.isEmpty())
    {
        QDomText domText = domDoc.createTextNode(strData);
        domElement.appendChild(domText);
    }
    return domElement;
}

void WorldState::handleEntity(quint32 entityAddress, MemoryAPI &mem)
{
    QString objType;
    QString objName;
    float coordX;
    float coordY;

    try{
        quint32 obj1 = entityAddress;
        quint32 pCfgVehicle = mem.readPtr(obj1 + 0x3C);
        quint32 obj3 = mem.readPtr(pCfgVehicle + 0x30);
        quint32 pObjType = mem.readPtr(pCfgVehicle + 0x6C);

        objType = mem.readArmaString(pObjType);
        objName = mem.readStringAscii(obj3 + 0x8, 25);

        quint32 pEntityVisualState = mem.readPtr(obj1 + 0x18);
        coordX = mem.readFloat(pEntityVisualState + 0x28);
        coordY = mem.readFloat(pEntityVisualState + 0x30);
    }catch(int a)
    {
        qDebug() << "Error virtual address not found.";
        return;
    }


    EntityData ed(objName, QPointF(coordX, coordY));

    if(objType == QStringLiteral("car"))
        ed.entityType = EntityData::type::car;
    else if(objType == QStringLiteral("motorcycle"))
        ed.entityType = EntityData::type::motorcycle;
    else if(objType == QStringLiteral("airplane"))
        ed.entityType = EntityData::type::airplane;
    else if(objType == QStringLiteral("helicopter"))
        ed.entityType = EntityData::type::helicopter;
    else if(objType == QStringLiteral("ship"))
        ed.entityType = EntityData::type::ship;
    else if(objType == QStringLiteral("tank"))
        ed.entityType = EntityData::type::tank;
    else if(objType == QStringLiteral("parachute"))
        ed.entityType = EntityData::type::parachute;
    else if(objName.indexOf(QStringLiteral("TentStorage"))!=-1)
        ed.entityType = EntityData::type::tent;
    else if(objName.indexOf(QStringLiteral("Stash"))!=-1)
        ed.entityType = EntityData::type::stash;
    else if(objName.indexOf(QStringLiteral("WoodenGate"))!=-1 || objName.indexOf(QStringLiteral("WoodenFence"))!=-1)
        ed.entityType = EntityData::type::fence;
    else if(objName.indexOf(QStringLiteral("DZ_MedBox"))!=-1 || objName.indexOf(QStringLiteral("DZ_AmmoBox"))!=-1)
        ed.entityType = EntityData::type::ammoBox;
    else if(objName.indexOf(QStringLiteral("Hedgehog_DZ"))!=-1)
        ed.entityType = EntityData::type::hedgehog;
    else if(objName.indexOf(QStringLiteral("Land_Camp_Fire_DZ"))!= -1)
        ed.entityType = EntityData::type::campFire;
    else if(objName.indexOf(QStringLiteral("CrashSite"))!= -1)
        ed.entityType = EntityData::type::crashSite;
    else if(objName.indexOf(QStringLiteral("WildBoar"))== 0 || objName.indexOf(QStringLiteral("Rabbit"))== 0 ||
            objName.indexOf(QStringLiteral("Cow"))== 0 || objName.indexOf(QStringLiteral("Sheep"))== 0 ||
            objName.indexOf(QStringLiteral("Goat"))== 0 || objName.indexOf(QStringLiteral("Hen"))== 0)
        ed.entityType = EntityData::type::animals;
    else if(objName.indexOf(QStringLiteral("Survivor2_DZ"))!= -1 || objName.indexOf(QStringLiteral("Sniper1_DZ"))!=-1 ||
            objName.indexOf(QStringLiteral("Camo1_DZ"))!=-1 || objName.indexOf(QStringLiteral("Survivor3_DZ"))!=-1 ||
            objName.indexOf(QStringLiteral("Bandit1_DZ"))!= -1 || objName.indexOf(QStringLiteral("Soldier1_DZ"))!= -1)
        ed.entityType = EntityData::type::players;
    else
        ed.entityType = EntityData::type::stuff;

    entityArray.append(ed);
}

void WorldState::initRanges()
{
    std::sort(entityArray.begin(),entityArray.end(), [](const EntityData& lhs, const EntityData& rhs){ return lhs.entityType < rhs.entityType; });
    EntityData::type currentType = EntityData::type::invalid;
    EntityRange currentRange;
    for(QVector<EntityData>::const_iterator it = entityArray.cbegin(); it != entityArray.cend(); ++it)
    {
        if(currentType == (*it).entityType)
            ++currentRange.end;
        else
        {
            if(currentType != EntityData::type::invalid)
                entityRanges[currentType] = currentRange;

            currentRange = EntityRange(it, it+1);
            currentType = (*it).entityType;
        }
    }
    if(!entityRanges.contains(currentType))
        entityRanges[currentType] = currentRange;
}
