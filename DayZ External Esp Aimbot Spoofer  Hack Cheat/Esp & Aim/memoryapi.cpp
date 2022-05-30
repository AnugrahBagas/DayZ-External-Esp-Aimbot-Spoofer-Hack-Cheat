#include "memoryapi.h"
#include <limits>
#include <QDebug>

MemoryAPI::MemoryAPI(QString pathDump, QString pathIDX)
{    
    loadDump(pathDump);
    loadIDX(pathIDX);
}

quint32 MemoryAPI::convertVirtToPhys(const quint32 virt) const
{
    for(auto it = memoryRelations.begin(); it != memoryRelations.end(); ++it)
    {
        if((*it).inRange(virt))
        {
            const quint32& phBase = (*it).getPhysicalAddress(), vrBase = (*it).getVirtualAddress();

            if(phBase>vrBase)
                return virt + (phBase - vrBase);
            else
                return virt - (vrBase - phBase);
        }
    }
    throw 1;
}

void MemoryAPI::loadIDX(QString path)
{
    QFile file(path);
    if(file.open(QIODevice::ReadOnly))
    {
        QTextStream stream(&file);

        for(quint8 i = 0; i<2; ++i)
            stream.readLine();

        while(!stream.atEnd())
        {
            quint64 fileAddress, length, virtualAddress;
            stream >> fileAddress >> length >> virtualAddress;
            if(fileAddress > std::numeric_limits<quint32>::max() || virtualAddress > std::numeric_limits<quint32>::max())
                continue;
            memoryRelations.append(MemoryRange(virtualAddress, fileAddress, length));
        }
    }else
        qDebug() << "IDX файл не открывается";
}

void MemoryAPI::loadDump(QString path)
{
    dumpFile.setFileName(path);
    dumpFile.open(QIODevice::ReadOnly);
}

QByteArray MemoryAPI::readVirtMem(const quint32 baseAddr, const quint32 size)
{
    QByteArray result;

    quint32 addr = convertVirtToPhys(baseAddr);
    dumpFile.seek(addr);
    result = dumpFile.read(size);

    return result;
}

qint32 MemoryAPI::readInt(const quint32 offset)
{
    qint32 a;
    QDataStream ds(readVirtMem(offset, 4));
    ds.setByteOrder(QDataStream::LittleEndian);
    ds >> a;
    return a;
}

quint32 MemoryAPI::readPtr(const quint32 offset)
{
    quint32 a;
    QDataStream ds(readVirtMem(offset, 4));
    ds.setByteOrder(QDataStream::LittleEndian);

    ds >> a;
    return a;
}

QString MemoryAPI::readStringAscii(const quint32 offset, const quint32 size)
{
    return QString(readVirtMem(offset, size));
}

float MemoryAPI::readFloat(const quint32 offset)
{
    float a;
    QDataStream ds(readVirtMem(offset, 4));
    ds.setByteOrder(QDataStream::LittleEndian);
    ds.setFloatingPointPrecision(QDataStream::SinglePrecision);

    ds >> a;
    return a;
}

QString MemoryAPI::readArmaString(quint32 offset)
{
    const int maxStringLength = 0x40;
    int absoluteLength = readPtr(offset + 0x4);
    if (absoluteLength > maxStringLength)
    {
        return QString("");
    }

    return readStringAscii(offset + 8, absoluteLength);
}
