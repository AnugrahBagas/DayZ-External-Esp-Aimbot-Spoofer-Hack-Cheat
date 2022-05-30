#include "settingsmanager.h"

SettingsManager::SettingsManager()
{
    QStringList list = settings.allKeys();

    for(QStringList::iterator it = list.begin();it!=list.end();++it)
    {
        data[*it] = settings.value(*it);
    }
}

SettingsManager::~SettingsManager()
{
    for(QMap<QString, QVariant>::iterator it = data.begin(); it!=data.end(); ++it)
    {
        settings.setValue(it.key(), it.value());
    }
}

void SettingsManager::setValue(const QString &key, const QVariant &value)
{
    data[key] = value;
}

QVariant SettingsManager::value(const QString &key, const QVariant &defaultValue)
{
    if(!data.contains(key))
    {
        QVariant s = settings.value(key,defaultValue);
        data.insert(key, s);
    }
    return data[key];
}

SettingsManager& SettingsManager::instance()
{
    static SettingsManager set;
    return set;
}
