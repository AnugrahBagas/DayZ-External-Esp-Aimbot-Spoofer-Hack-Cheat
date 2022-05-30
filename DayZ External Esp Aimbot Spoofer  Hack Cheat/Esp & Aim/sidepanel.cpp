#include "sidepanel.h"
#include <QGroupBox>
#include <QVBoxLayout>
#include <QHBoxLayout>
#include <QCheckBox>
#include <QSignalMapper>
#include <QDebug>
#include "settingsmanager.h"
#include <QLabel>

FilterCheckBox::FilterCheckBox(QString name, EntityData::type et, QWidget *parent): QCheckBox(parent)
{
    setText(name);
    filterType = QVariant(static_cast<int>(et)).toString();

    loadSetting();

    connect(this, &FilterCheckBox::clicked, this, &FilterCheckBox::updateSetting);
}

FilterCheckBox::FilterCheckBox(QString name, QString et, QWidget *parent): QCheckBox(parent)
{
    setText(name);
    filterType = et;

    loadSetting();

    connect(this, &FilterCheckBox::clicked, this, &FilterCheckBox::updateSetting);
}

void FilterCheckBox::loadSetting()
{
    setChecked(SettingsManager::instance().value(QVariant(filterType).toString(), false).toBool());
}

void FilterCheckBox::updateSetting(bool value)
{
    SettingsManager::instance().setValue(filterType, value);
}

SidePanel::SidePanel(QWidget *parent) : QWidget(parent)
{
////////////////////////////////////VEHICLES///////////////////////////////
    QVBoxLayout*    panelLayout = new QVBoxLayout;

    QGroupBox*  vehBox = new QGroupBox(QStringLiteral("Vehicles"));
    QVBoxLayout*    vehLayout = new QVBoxLayout;

    QVector<FilterCheckBox*> veh;
    veh.append(new FilterCheckBox(QStringLiteral("Cars"),               EntityData::type::car));
    veh.append(new FilterCheckBox(QStringLiteral("Motorcycles"),        EntityData::type::motorcycle));
    veh.append(new FilterCheckBox(QStringLiteral("Airplanes"),          EntityData::type::airplane));
    veh.append(new FilterCheckBox(QStringLiteral("Ships"),              EntityData::type::ship));
    veh.append(new FilterCheckBox(QStringLiteral("Helicopters"),        EntityData::type::helicopter));
    veh.append(new FilterCheckBox(QStringLiteral("Parachutes"),         EntityData::type::parachute));
    veh.append(new FilterCheckBox(QStringLiteral("Tanks"),              EntityData::type::tank));

    mapCheckBoxes(veh, vehLayout);

    vehBox->setLayout(vehLayout);
    panelLayout->addWidget(vehBox);

////////////////////////////////BUILDINGS///////////////////////////////////
    QGroupBox*      buildBox    = new QGroupBox(QStringLiteral("Buildings"));
    QVBoxLayout*    buildLayout = new QVBoxLayout;

    QVector<FilterCheckBox*> build;
    build.append(new FilterCheckBox(QStringLiteral("Tents"),            EntityData::type::tent));
    build.append(new FilterCheckBox(QStringLiteral("Stashes"),          EntityData::type::stash));
    build.append(new FilterCheckBox(QStringLiteral("Hedgehog"),         EntityData::type::hedgehog));
    build.append(new FilterCheckBox(QStringLiteral("Wooden Fences"),    EntityData::type::fence));
    build.append(new FilterCheckBox(QStringLiteral("Ammo Boxes"),       EntityData::type::ammoBox));
    build.append(new FilterCheckBox(QStringLiteral("Camp Fires"),       EntityData::type::campFire));
    build.append(new FilterCheckBox(QStringLiteral("Crash Sites"),      EntityData::type::crashSite));
    mapCheckBoxes(build, buildLayout);

    buildBox->setLayout(buildLayout);
    panelLayout->addWidget(buildBox);
///////////////////////////////ENTITIES////////////////////////////////////////
    QGroupBox*      entBox      = new QGroupBox(QStringLiteral("Entities"));
    QVBoxLayout*    entLayout   = new QVBoxLayout;

    QVector<FilterCheckBox*> en;
    en.append(new FilterCheckBox(QStringLiteral("Animals"),             EntityData::type::animals));
    en.append(new FilterCheckBox(QStringLiteral("Players"),             EntityData::type::players));
    en.append(new FilterCheckBox(QStringLiteral("Useless Stuff"),       EntityData::type::stuff));

    mapCheckBoxes(en, entLayout);

    entBox->setLayout(entLayout);
    panelLayout->addWidget(entBox);
//////////////////////////Other/////////////////////////////////////////////
    QGroupBox*      othBox      = new QGroupBox(QStringLiteral("Other"));
    QVBoxLayout*    othLayout   = new QVBoxLayout;

    QVector<FilterCheckBox*> oth;
    oth.append(new FilterCheckBox(QStringLiteral("Object Names"), QStringLiteral("name")));

    mapCheckBoxes(oth, othLayout);

    closeObjects = new QPlainTextEdit(QStringLiteral("Press mid button on map"));
    closeObjects->setReadOnly(true);

    othLayout->addWidget(closeObjects);

    othBox->setLayout(othLayout);
    panelLayout->addWidget(othBox);
//////////////////////////////////////////////////////////////////////////////

    panelLayout->addStretch(1);
    setLayout(panelLayout);
}

void SidePanel::mapCheckBoxes(QVector<FilterCheckBox *> &v, QVBoxLayout *bl)
{
    for(QVector<FilterCheckBox*>::iterator it = v.begin(); it!=v.end(); ++it)
    {
        bl->addWidget(*it);
        connect((*it), &FilterCheckBox::clicked, this, &SidePanel::updateMapS);
    }
}
