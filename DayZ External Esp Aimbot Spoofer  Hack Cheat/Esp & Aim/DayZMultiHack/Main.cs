#define FULL_VERSION

using D3DMenu.MenuTypes;
using DayZMultiHack.Models;
using DayZMultiHack.Modules;
using ExternalD3D11;
using ExternalD3D11.ClickableMenu;
using ExternalD3D11.Console;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilsLibrary;
using UtilsLibrary.MathObjects;

namespace DayZMultiHack
{
    public class Main
    {
        #region Globals

        public ProcessMemory mem = new ProcessMemory("client.bin");
        private UI directXUI;
        private Menu menu;
        private LoadingScreen loadingScreen = new LoadingScreen();

        // Modules
        private NoGrass noGrassModule;
        private NoRecoil noRecoilModule;
        private NoFatigue noFatigueModule;
        private Weather weatherModule;
        private Speedhack speedhackModule;

        private LocalEntityValues localEntityValuesModule;
        private EntityESP entityESPModule;
        private ItemESP itemESPModule;
        private LocationESP locationESPModule;
        private Scoreboard scoreboardModule;
        private Crosshair crosshairModule;
        private InventoryDisplay inventoryDisplayModule;
        private BulletESP bulletESPModule;
        private TopHUD topHUDModule;
        private Radar radarModule;
        private ItemStealer itemStealModule;
        private MurderMode murderModeModule;
        private WaypointESP waypointESPModule;
        private Test testModule;

        private List<Task> taskGetData = new List<Task>();
        private List<Task> taskModuleUpdate = new List<Task>();
        private List<Task> tasksStep3 = new List<Task>();

        #endregion

        #region Properties

        public IntPtr WorldBase { get; set; }
        public Transformation TransformationData { get; set; }
        public ScoreboardTable ScoreboardTable { get; set; }
        public Entity LocalEntity{ get; set; }

        public IntPtr LocalEntityBasePointer { get; set; }
        public EntitiesList EntitiesList { get; set; }
        public List<Entity> FilteredEntities { get; set; }
        public Entity NearestEntityToCursor { get; set; }
        public UtilsLibrary.MathObjects.Vector3 InGameCrosshairPosition { get; set; }
        public List<string> SuspiciousPlayers { get; set; }

        public List<Item> Items { get; set; }
        #endregion

        #region Constructor

        public Main()   
        {
            
            loadingScreen.UpdateText("Initialize DirectX...");
            loadingScreen.Show();

            directXUI = new UI();
            directXUI.OnOverlayLoaded += Ui_OnOverlayLoaded;
            directXUI.OnKeyPressed += DirectXUI_OnRadarSettingChanged;

            menu = new Menu(directXUI);
            directXUI.Menu = menu.D3DMenu;

            loadingScreen.UpdateText("Loading Modules...");

            // Modules
            noGrassModule = new NoGrass(this, directXUI, menu);
            noRecoilModule = new NoRecoil(this, directXUI, menu);
            noFatigueModule = new NoFatigue(this, directXUI, menu);
            weatherModule = new Weather(this, directXUI, menu);
            localEntityValuesModule = new LocalEntityValues(this, directXUI, menu);
            entityESPModule = new EntityESP(this, directXUI, menu);
            locationESPModule = new LocationESP(this, directXUI, menu);
            crosshairModule = new Crosshair(this, directXUI, menu);
            bulletESPModule = new BulletESP(this, directXUI, menu);
            topHUDModule = new TopHUD(this, directXUI, menu);
            radarModule = new Radar(this, directXUI, menu);
            waypointESPModule = new WaypointESP(this, directXUI, menu);
            testModule = new Test(this, directXUI, menu);

#if FULL_VERSION

            speedhackModule = new Speedhack(this, directXUI, menu);
            itemESPModule = new ItemESP(this, directXUI, menu);
            scoreboardModule = new Scoreboard(this, directXUI, menu);
            inventoryDisplayModule = new InventoryDisplay(this, directXUI, menu);
            itemStealModule = new ItemStealer(this, directXUI, menu);
            murderModeModule = new MurderMode(this, directXUI, menu);
#endif

            Items = new List<Item>();
            SuspiciousPlayers = new List<string>();

            loadingScreen.UpdateText("Loading Overlay...");

            // Console Commands
            directXUI.Console.Commands.Add(new ConsoleCommand("exitgame", new ConsoleCommand.OnCommandEventHandler(KillGameCommandExecuted), "Kills the game process.", "exitgame"));
            directXUI.Initialize(); // Blocking
        }

        #endregion

        #region Methods

        private void Ui_OnOverlayLoaded(object sender)
        {
            loadingScreen.UpdateText("Loading Menu and starting Core...");

            Thread t = new Thread(CoreLoop);
            t.Priority = ThreadPriority.Highest;
            t.Start();
        }

        private void DirectXUI_OnRadarSettingChanged(object sender, int code)
        {
            if (code == Keys.NumPad5.GetHashCode())
                menu.ShowRadarOption.State = !menu.ShowRadarOption.State;
            else if (code == Keys.NumPad4.GetHashCode())
                menu.SpeedhackOnlyOnWalkingOption.State = !menu.SpeedhackOnlyOnWalkingOption.State;
            else if(code == Keys.NumPad6.GetHashCode())
            {
                menu.MurderModeOption.State = !menu.MurderModeOption.State;
                menu.MurderModeUsePlayerSelection.State = false;
            }
        }


        private void CoreLoop()
        {
            EntitiesList = new EntitiesList();

            Entity entity1 = new Entity();
            entity1.Pointer = (IntPtr)1;
            entity1.BasePointer = (IntPtr)1;
            entity1.Distance = 239;
            entity1.IsDead = false;
            entity1.IsTN = false;
            entity1.Name = "Test1";
            entity1.Stance = "Standing";
            entity1.Type = Entity.Types.Soldier;
            entity1.VisualStatePointer = (IntPtr)1;
            entity1.ScreenHeadPosition = new UtilsLibrary.MathObjects.Vector3(997, 553, 1);
            entity1.ScreenFootPosition = new UtilsLibrary.MathObjects.Vector3(997, 560, 1);

            Driver driver1 = new Driver();
            driver1.ID = 1;
            driver1.Pointer = (IntPtr)1;
            driver1.Name = "Homo";
            driver1.Stats = new EntityValues();

            Entity entity2 = new Entity();
            entity2.Pointer = (IntPtr)2;
            entity2.BasePointer = (IntPtr)2;
            entity2.Distance = 5;
            entity2.IsDead = false;
            entity2.IsTN = false;
            entity2.Position = new UtilsLibrary.MathObjects.Vector3(100, 100, 1);
            entity2.Name = "SUV";
            entity2.ObjectName = "SUVVV";
            entity2.Stance = "Standing";
            entity2.Type = Entity.Types.Soldier;
            entity2.VisualStatePointer = (IntPtr)2;
            entity2.ID = 2;
            //entity2.DriverPointer = (IntPtr)2;
            //entity2.Driver = driver1;
            entity2.ScreenFootPosition = new UtilsLibrary.MathObjects.Vector3(300, 800, 1);
            entity2.ScreenHeadPosition = new UtilsLibrary.MathObjects.Vector3(300, 600, 1);

            //EntitiesList.Entities.Add(entity1);
            //EntitiesList.Entities.Add(entity2);

            directXUI.Menu.Load();

            loadingScreen.CloseLoading();

            while (!menu.ExitOption.ExitApplication)
            {
                directXUI.BeginDraw();
                menu.DrawMenu();

                //testModule.Draw();

                // Waiting for game
                //if (!mem.GetProcess())
                //{
                //    directXUI.DrawWarningBox("Waiting for DayZ...", "Press 'insert' to exit.", 5, 255);
                //    directXUI.EndDraw();

                //    if (directXUI.Insert_Pressed)
                //        menu.ExitOption.ExitApplication = true;

                //    continue;
                //}

                //entityESPModule.Draw();
                //waypointESPModule.Draw();
                //FilteredEntities = EntitiesList.GetEnemySoldiers(LocalEntity.Pointer);
                //menu.MurderModeSelectedPlayer.Players = FilteredEntities.Select(e => new PlayerSelection.Player { Pointer = e.Pointer, Name = e.Name }).ToList();
                //NearestEntityToCursor = EntitiesList.GetNearestEntiyToCursor(Cursor.Position.X, Cursor.Position.Y, 2000);

                // World
                WorldBase = mem.Read<IntPtr>((IntPtr)Offsets.World.BASE);

                // Local Player Data
                IntPtr localPlayerMultiChainPtr = mem.Read<IntPtr>(WorldBase + Offsets.World.PLAYER_LOCAL);
                IntPtr localPlayerPointer = mem.Read<IntPtr>(localPlayerMultiChainPtr + 0x4);
                LocalEntityBasePointer = localPlayerMultiChainPtr;

                // Ingame check
                /*if (WorldBase == IntPtr.Zero || localPlayerPointer == IntPtr.Zero)
                {
                    directXUI.DrawWarningBox("Waiting for joining Server...", "No World Pointer detected.", 5, 255);
                    directXUI.EndDraw();
                    continue;
                }*/

                // Get all important data from memory
                //GetData(localPlayerPointer);


                // Update modules. These can be called async
                // and we don't need to wait for them to complete.
//                taskModuleUpdate.Clear();
//                taskModuleUpdate.Add(new Task(() => noGrassModule.Update()));
//                taskModuleUpdate.Add(new Task(() => noRecoilModule.Update()));
//                taskModuleUpdate.Add(new Task(() => noFatigueModule.Update()));
//                taskModuleUpdate.Add(new Task(() => weatherModule.Update()));

//#if FULL_VERSION

//                taskModuleUpdate.Add(new Task(() => menu.InventoriesSelectedPlayerOption.Players = FilteredEntities.Select(e => new PlayerSelection.Player{Pointer = e.Pointer, Name = e.Name}).ToList()));
//                taskModuleUpdate.Add(new Task(() => menu.MurderModeSelectedPlayer.Players = FilteredEntities.Select(e => new PlayerSelection.Player { Pointer = e.Pointer, Name = e.Name }).ToList()));
//                taskModuleUpdate.Add(new Task(() => speedhackModule.UpdateTransformationData(TransformationData)));
//#endif

//                foreach (Task task in taskModuleUpdate)
//                    task.Start();


                // Fit game window to screen
                if (menu.FitDayZToScreenOption.State)
                {
                    directXUI.FitGameToScreen("client.bin");
                    menu.FitDayZToScreenOption.State = false;
                }

                // Drawing
                // This needs to be called one by another, because
                // of DirectX drawing functions. Locks don't seem
                // to work everytime.

                // Get selected entities in the menu
//#if FULL_VERSION
//                Entity inventoryEntity = inventoryDisplayModule.Draw();
//                murderModeModule.Run();
//                itemStealModule.Run(inventoryEntity);
//                scoreboardModule.Draw();
//                itemESPModule.Draw();
//#endif
//                localEntityValuesModule.Draw();
//                radarModule.Draw();
//                locationESPModule.Draw();
//                crosshairModule.Draw();
//                bulletESPModule.Draw();
//                topHUDModule.Draw();
//                entityESPModule.Draw();
//                waypointESPModule.Draw();

                // Save settings
                if (menu.saveSettingsOption.State)
                {
                    directXUI.Menu.Save();
                    menu.saveSettingsOption.State = false;
                }

                directXUI.EndDraw();
            }

            directXUI.Dispose();
            System.Windows.Forms.Application.Exit();
        }

        private void GetData(IntPtr localPlayerPtr)
        {
            // This method gets all important data async.
            // All tasks are working async but this methods will only
            // return if all tasks are complete.

            taskGetData.Clear();

            // Transformation
            TransformationData = Data.GetTransformation(mem);

            // Scoreboard
            ScoreboardTable = Data.GetScoreboard(mem);

            // Local entity data
            LocalEntity = Data.GetLocalEntity(mem, TransformationData, ScoreboardTable, localPlayerPtr);

            // Get all entities
            EntitiesList = Data.GetEntities(mem, WorldBase, TransformationData, ScoreboardTable, LocalEntity.Position);

            // Get nearest to cursor
            int x = directXUI.Width / 2;
            int y = directXUI.Height / 2;

            Entity entity = EntitiesList.Entities.Aggregate((a, b) => EntitiesList.DistanceToPoint(a, x, y) > EntitiesList.DistanceToPoint(b, x, y) ? b : a);
            if (entity.Pointer != IntPtr.Zero)
                NearestEntityToCursor = entity;

            //Console.WriteLine(NearestEntityToCursor);

            // Get Ingame cursor
            InGameCrosshairPosition = Data.GetIngameCursorPosition(mem, WorldBase);

            // Items
            Items.Clear();

            if (menu.EspNearItemsOption.State)
                taskGetData.Add(new Task(() => { Items.AddRange(Data.GetNearItems(mem, WorldBase, TransformationData)); }));

            if (menu.EspNearDroppedOption.State)
                taskGetData.Add(new Task(() => { Items.AddRange(Data.GetNearDroppedItems(mem, WorldBase, TransformationData)); }));

            if (menu.EspFarDroppedItemsOption.State)
                taskGetData.Add(new Task(() => { Items.AddRange(Data.GetFarDroppedItems(mem, WorldBase, TransformationData)); }));

            foreach (Task task in taskGetData)
                task.Start();

            Task.WaitAll(taskGetData.ToArray());

            FilteredEntities = EntitiesList.GetEnemySoldiers(LocalEntity.Pointer);
        }

        private void KillGameCommandExecuted(ConsoleCommandArgs e)
        {
            mem.CurrentProcess.Kill();
        }

        #endregion
    }
}
