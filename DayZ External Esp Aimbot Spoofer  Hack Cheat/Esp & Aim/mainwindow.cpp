#include "mainwindow.h"
#include "sidepanel.h"
#include "settingsmanager.h"
#include <QPainter>
#include <QPixmap>
#include <QMenuBar>
#include <QHBoxLayout>
#include <QSplitter>
#include <QMenu>
#include <QFileDialog>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent)
{
    setMinimumSize(800,600);
    SettingsManager initSettings;

    map = new InteractiveMap;

    QMenu *pFile = new QMenu("File");
    QMenu *pHelp = new QMenu("Help");

    pFile->addAction(QStringLiteral("Load dump"),                       this, &MainWindow::loadDump);
    pFile->addAction(QStringLiteral("Load state"),                      this, &MainWindow::loadState);

    QAction* saveAct = pFile->addAction(QStringLiteral("Save state"),   this, &MainWindow::saveWorldState);
    QAction* closeAct = pFile->addAction(QStringLiteral("Close state"), this, &MainWindow::closeWorldState);

    saveAct->setEnabled(false);
    closeAct->setEnabled(false);

    pFile->addAction(QStringLiteral("Exit"), QCoreApplication::instance(),    &QCoreApplication::quit);

    menuBar()->addMenu(pFile);
    menuBar()->addMenu(pHelp);

    QSplitter* spl = new QSplitter(Qt::Horizontal);
    SidePanel* panel = new SidePanel;

    panel->setMinimumWidth(250);
    panel->setMaximumWidth(panel->minimumWidth());
    panel->resize(panel->minimumWidth(), panel->height());

    spl->addWidget(map);
    spl->addWidget(panel);

    setCentralWidget(spl);

    connect(panel,  &SidePanel::updateMap,              map,                    &InteractiveMap::updateCache);
    connect(map,    &InteractiveMap::saveStateChanged,  saveAct,                &QAction::setEnabled);
    connect(map,    &InteractiveMap::saveStateChanged,  closeAct,               &QAction::setEnabled);
    connect(map,    &InteractiveMap::showCloseObjects,  panel->closeObjects,    &QPlainTextEdit::setPlainText);
}

MainWindow::~MainWindow()
{
}

void MainWindow::loadDump()
{
    QString dump = QFileDialog::getOpenFileName(nullptr, QStringLiteral("Load .dmp file")),
            idx  = QFileDialog::getOpenFileName(nullptr, QStringLiteral("Load .idx file"));

    if(!dump.isEmpty() && !idx.isEmpty())
        map->loadDump(dump, idx);
}

void MainWindow::loadState()
{
    QString st = QFileDialog::getOpenFileName(nullptr, QStringLiteral("Load state file"));

    if(!st.isEmpty())
        map->loadState(st);
}

void MainWindow::saveWorldState()
{
    QString st = QFileDialog::getSaveFileName(nullptr, QStringLiteral("Save state"));

    if(!st.isEmpty())
        map->saveState(st);
}

void MainWindow::closeWorldState()
{
    map->closeState();
}
