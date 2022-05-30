#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "interactivemap.h"

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
private:
    InteractiveMap* map;

public slots:
    void loadDump();
    void loadState();
    void saveWorldState();
    void closeWorldState();
};

#endif // MAINWINDOW_H
