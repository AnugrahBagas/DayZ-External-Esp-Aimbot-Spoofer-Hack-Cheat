#include "mainwindow.h"
#include <QApplication>

int main(int argc, char *argv[])
{
    QCoreApplication::setOrganizationName(QStringLiteral("Luós.tm"));
    QCoreApplication::setApplicationName(QStringLiteral("DumpHack"));

    QApplication a(argc, argv);
    MainWindow w;
    w.show();

    return a.exec();
}
