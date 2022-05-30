#ifndef SIDEPANEL_H
#define SIDEPANEL_H

#include "entitydata.h"
#include <QWidget>
#include <QGroupBox>
#include <QSignalMapper>
#include <QVBoxLayout>
#include <QCheckBox>
#include <QPlainTextEdit>

class FilterCheckBox : public QCheckBox
{
    Q_OBJECT
public:
    explicit FilterCheckBox(QWidget *parent = nullptr);
    FilterCheckBox(QString name, QString et, QWidget *parent = nullptr);
    FilterCheckBox(QString name, EntityData::type et, QWidget *parent = nullptr);
    QString filterType;

    void loadSetting();

public slots:
    void updateSetting(bool value);
};

class SidePanel : public QWidget
{
    Q_OBJECT
public:
    explicit SidePanel(QWidget *parent = nullptr);
    QPlainTextEdit* closeObjects;
private:
    void mapCheckBoxes(QVector<FilterCheckBox *> &v, QVBoxLayout* bl);

public slots:
    void updateMapS(){ emit updateMap(); }

signals:
    void updateMap();
};

#endif // SIDEPANEL_H
