#define FULL_VERSION

using D3DMenu;
using D3DMenu.MenuTypes;
using ExternalD3D11;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZMultiHack
{
    public class Menu
    {
        #region Globals

        private UI directXUI;

        #endregion

        #region  Menu Globals

        #region General

        public Back generalBackOption;
        public BoolSwitch SpeedhackOption;
        public BoolSwitch SpeedhackOnlyOnWalkingOption;
        public BoolSwitch flyModeOption;
        public ValueSelection SpeedOption;
        public BoolSwitch MurderModeOption;
        public MurderModeType MurderModeTypeOption;
        public MurderModePosition MurderModePositionOption;
        public BoolSwitch MurderModeUsePlayerSelection;
        public PlayerSelection MurderModeSelectedPlayer;
        public BoolSwitch MurderModeOnKeyOption;
        public BoolSwitch AnonymousBulletsOption;
        public BoolSwitch ItemMagnetOption;
        public BoolSwitch NoGrassOption;
        public BoolSwitch FitDayZToScreenOption;
        public BoolSwitch saveSettingsOption;
        public ExitApp ExitOption;

        #endregion

        #region UI

        public Back uiBackOption;
        public BoolSwitch UIShowScoreboardOption;
        public BoolSwitch UILocalPlayerValuesOption;

        public BoolSwitch TopHUDEnabledOption;
        public BoolSwitch TopHUDShowFPSOption;
        public BoolSwitch TopHUDShowServerNameOption;
        public BoolSwitch TopHUDShowLocalPlayerPosition;
        public BoolSwitch TopHUDShowEnemyCount;
        public BoolSwitch TopHUDShowNearestPlayers;
        public BoolSwitch TopHUDShowItemStealerEntity;

        public BoolSwitch CrosshairEnabledOption;
        public ValueSelection CrosshairSizeOption;
        public ValueSelection CrosshairThicknessOption;
        public CrosshairType CrosshairTypeOption;
        public ColorSelection CrosshairColorOption;

        #endregion

        #region Radar

        public Back radarBackOption;
        public BoolSwitch ShowRadarOption;
        public ValueSelection RadarZoomOption;
        public ValueSelection RadarSizeOption;
        public ValueSelection RadarOpacityOption;
        public BoolSwitch RadarEllipseOption;
        public BoolSwitch RadarShowDistance;
        public BoolSwitch RadarEnemiesOption;
        public BoolSwitch RadarEnemyLinesOption;
        public BoolSwitch RadarZombiesOption;
        public BoolSwitch RadarTNOption;
        public BoolSwitch RadarTNLinesOption;
        public BoolSwitch RadarOccupiedVehicle;
        public BoolSwitch RadarOccupiedVehicleLine;
        public BoolSwitch RadarCarsOption;
        public BoolSwitch RadarShipsOption;
        public BoolSwitch RadarHelicoptersOption;
        public BoolSwitch RadarPlanesOption;

        #endregion

        #region ESP

        public Back espBackOption;
        public BoolSwitch EspPlayersOption;
        public BoolSwitch EspOnlyTNPlayers;
        public BoolSwitch EspPlayersLineOption;
        public LinePosition EspPlayersLinePositionOption;
        public BoolSwitch EspSmallPlayerStats;
        public BoolSwitch EspPlayerTemparatureOption;
        public BoolSwitch EspDeadPlayerOption;
        public BoolSwitch EspZombiesOption;
        public BoolSwitch EspShowHeadMarker;
        public BoolSwitch EspRoundedBoxesOption;
        public BoolSwitch EspNearItemsOption;
        public BoolSwitch EspFarDroppedItemsOption;
        public BoolSwitch EspNearDroppedOption;
        public BoolSwitch EspAirfieldsOption;
        public BoolSwitch EspMainCitiesOption;
        public BoolSwitch EspShowOccupiedVehicles;
        public BoolSwitch EspShowOccupiedVehiclesLine;
        public LinePosition EspOccupiedVehiclesLinePosition;
        public BoolSwitch EspCarsOption;
        public BoolSwitch EspShipsOption;
        public BoolSwitch EspHelicopterOption;
        public BoolSwitch EspPlanesOption;
        public BoolSwitch EspWreckOption;

        public BoolSwitch EspBulletsOption;

        #endregion

        #region Inventories

        public Back inventoriesBackOption;
        public BoolSwitch InventoriesEnabledOption;
        public PlayerSelection InventoriesSelectedPlayerOption;
        public BoolSwitch InventriesShowPrimaryOption;
        public BoolSwitch InventriesShowMeleeOption;
        public BoolSwitch InventriesShowHelmetOption;
        public BoolSwitch InventriesShowMaskOption;
        public BoolSwitch InventriesShowGlassesOption;
        public BoolSwitch InventriesShowChestOption;
        public BoolSwitch InventriesShowPantsOption;
        public BoolSwitch InventriesShowShoesOption;
        public BoolSwitch InventriesShowVestOption;
        public BoolSwitch InventriesShowGlovesOption;
        public BoolSwitch InventriesShowBackpackOption;

        #endregion

        #region Item stealer

        public Back itemStealerBackOption;
        public BoolSwitch ItemStealerPrimaryOption;
        public BoolSwitch ItemStealerMeleeOption;
        public BoolSwitch ItemStealerHelmetOption;
        public BoolSwitch ItemStealerMaskOption;
        public BoolSwitch ItemStealerGlassesOption;
        public BoolSwitch ItemStealerChestOption;
        public BoolSwitch ItemStealerPantsOption;
        public BoolSwitch ItemStealerShoesOption;
        public BoolSwitch ItemStealerVestOption;
        public BoolSwitch ItemStealerGlovesOption;
        public BoolSwitch ItemStealerBackpackOption;

        #endregion

        #region Weather

        public Back weatherBackOption;
        public ValueSelection WeatherOvercastOption;
        public ValueSelection WeatherFogOption;
        public ValueSelection WeatherWindOption;
        public ValueSelection WeatherSunPositionOption;
        public BoolSwitch WeatherSunnyOption;
        public BoolSwitch WeatherStormyOption;
        public BoolSwitch WeatherApocalypticOption;

        #endregion

        #region General ESP Options

        public Back espSettingsBackOption;
        public ColorSelection PlayerColorOption;
        public ColorSelection SuspiciousPlayerColorOption;
        public ColorSelection PlayerLineColorOption;
        public ColorSelection ZombieColorOption;
        public ColorSelection TNColorOption;
        public ColorSelection TNLineColorOption;
        public ColorSelection PlayerLoggingOffColorOption;
        public ColorSelection PlayerLoggingOffLineColorOption;
        public ColorSelection AirfieldsColorOption;
        public ColorSelection OccupiedVehicleColorOption;
        public ColorSelection OccupiedVehicleLineColorOption;
        public ColorSelection CarsColorOption;
        public ColorSelection ShipsColorOption;
        public ColorSelection HelicopterColorOption;
        public ColorSelection PlanesColorOption;
        public ColorSelection WreckColorOption;

        #endregion

        #region Item ESP Settings

        public Back itemsBackOption;
        public BoolSwitch WeaponsOption;
        public BoolSwitch MagsOption;
        public BoolSwitch GrenadesOption;
        public BoolSwitch AttachmentsOption;
        public BoolSwitch AmmoOption;
        public BoolSwitch PistolsOption;
        public BoolSwitch MeeleOption;
        public BoolSwitch ConsumablesOption;
        public BoolSwitch VestsOption;
        public BoolSwitch EyewearOption;
        public BoolSwitch GlovesOption;
        public BoolSwitch HeadgearOption;
        public BoolSwitch MasksOption;
        public BoolSwitch PantsOption;
        public BoolSwitch ShoesOption;
        public BoolSwitch ShirtJacketsOption;
        public BoolSwitch BackpacksOption;
        public BoolSwitch ContainerOption;
        public BoolSwitch MedicalOption;
        public BoolSwitch DrinkOption;
        public BoolSwitch FoodOption;
        public BoolSwitch ToolsOption;
        public BoolSwitch CraftingOption;
        public BoolSwitch MiscOption;
        public BoolSwitch UnkownOption;

        #endregion

        #region Item Colors

        public Back itemColorsBackOption;
        public ColorSelection WeaponsColorOption;
        public ColorSelection AttachmentsColorOption;
        public ColorSelection MagsColorOption;
        public ColorSelection AmmoColorOption;
        public ColorSelection GrenadesColorOption;
        public ColorSelection PistolsColorOption;
        public ColorSelection MeeleColorOption;
        public ColorSelection ConsumablesColorOption;
        public ColorSelection VestsColorOption;
        public ColorSelection EyewearColorOption;
        public ColorSelection GlovesColorOption;
        public ColorSelection HeadgearColorOption;
        public ColorSelection MasksColorOption;
        public ColorSelection PantsColorOption;
        public ColorSelection ShoesColorOption;
        public ColorSelection ShirtJacketsColorOption;
        public ColorSelection BackpacksColorOption;
        public ColorSelection ContainerColorOption;
        public ColorSelection MedicalColorOption;
        public ColorSelection DrinkColorOption;
        public ColorSelection FoodColorOption;
        public ColorSelection ToolsColorOption;
        public ColorSelection CraftingColorOption;
        public ColorSelection MiscColorOption;
        public ColorSelection UnknownColorOption;

        #endregion

        #region Player

        public Back playerBackOption;
        public BoolSwitch NoRecoilOption;
        public BoolSwitch NoFatigueOption;


        #endregion

        #region Info

        //public Back infoBackOption;
        //public D3DLabelItem infoToggleRadar;
        //public D3DLabelItem infoZoomIn;
        //public D3DLabelItem infoZoomOut;
        //public D3DLabelItem infoSizeDown;
        //public D3DLabelItem infoSizeUp;
        //public D3DLabelItem infoToggleMenu;
        //public D3DLabelItem infoMurderModeAuto;

        #endregion

        #endregion

        #region Properties

        public DirectXMenu D3DMenu { get; set; }
        public float MenuPositionX { get; set; }
        public float MenuPositionY { get; set; }

        public float MenuTextY { get; set; }
        public float MenuTextYStart { get; set; }
        public float MenuTextLineHeight { get; set; }

        public float MenuSeparatorPadding { get; set; }
        public float MenuSeparatorHeight { get; set; }

        public float MenuTextX { get { return MenuPositionX + 18; } }
        public float MenuValueX { get { return MenuPositionX + 213; } }
        public float MenuColorX { get { return MenuPositionX + 220; } }
        public float MenuNavigatorX { get { return MenuPositionX + 5; } }

        public float MenuWidth { get; set; }
        public float MenuTitleHeight { get; set; }
        public float MenuItemHeight { get; set; }

        #endregion

        #region Constructor

        public Menu(UI ui)
        {
            directXUI = ui;
            D3DMenu = new DirectXMenu();

            CreateDefault();
            MenuItemHeight = 18;
            MenuWidth = 290;
            MenuTextLineHeight = 15;
            MenuTextYStart = 210;
            MenuTitleHeight = 23;
            MenuPositionX = 7;
            MenuPositionY = 178;
            MenuSeparatorPadding = 4;
            MenuSeparatorHeight = 0.5f;
        }

        #endregion

        #region Methods

        public void CreateDefault()
        {
            D3DMenu.SubMenus.Clear();

            #region General

            SubMenu generalMenu = new SubMenu("General");
            generalBackOption = new Back();
            SpeedhackOption = new BoolSwitch("speedhackEnabled", "Speedhack", false, false);
            SpeedhackOnlyOnWalkingOption = new BoolSwitch("speedhackOnlyOnMoving", "Speedhack only on moving", false, false);
            SpeedOption = new ValueSelection("speedhackSpeed", "Speed", 0.2f, 2f, 0.01f, 0.02f, true);
            flyModeOption = new BoolSwitch("speedhackFlyMode", "Fly Mode", false, false);
            MurderModeOption = new BoolSwitch("murderModeEnabled", "Enabled", false, false);
            MurderModeTypeOption = new MurderModeType("murderModeTargetType", "Target Type", MurderModeType.Type.Soldier, true);
            MurderModePositionOption = new MurderModePosition("murderModeBodyPosition", "Body position", MurderModePosition.Positions.Head, true);
            MurderModeUsePlayerSelection = new BoolSwitch("murderModeTargetSpecificPlayer", "Target specific Player", false, false);
            MurderModeSelectedPlayer = new PlayerSelection("Player");
            MurderModeOnKeyOption = new BoolSwitch("murderModeOnlyOnKey", "Murder Mode on Key", false, true);
            AnonymousBulletsOption = new BoolSwitch("anonymousBullets", "Anonymous Bullets", false, true);
            ItemMagnetOption = new BoolSwitch("itemMagnet", "Item Magnet", false, false);
            NoGrassOption = new BoolSwitch("noGrass", "No Grass", true, true);
            FitDayZToScreenOption = new BoolSwitch("fitDayZToScreen", "Fit DayZ to screen", false, false);
            saveSettingsOption = new BoolSwitch("saveSettings", "Save Settings", false, false);
            ExitOption = new ExitApp("Exit");
            generalMenu.Items.Add(generalBackOption);
#if FULL_VERSION
            generalMenu.Items.Add(SpeedhackOption);
            generalMenu.Items.Add(SpeedhackOnlyOnWalkingOption);
            generalMenu.Items.Add(SpeedOption);
            //generalMenu.Items.Add(flyModeOption);
            generalMenu.Items.Add(new Separator("Murder mode"));
            generalMenu.Items.Add(MurderModeOption);
            generalMenu.Items.Add(MurderModeTypeOption);
            //generalMenu.Items.Add(MurderModePositionOption);
            generalMenu.Items.Add(MurderModeUsePlayerSelection);
            generalMenu.Items.Add(MurderModeSelectedPlayer);
            generalMenu.Items.Add(MurderModeOnKeyOption);
#endif
            generalMenu.Items.Add(new Separator());
            generalMenu.Items.Add(AnonymousBulletsOption);
            generalMenu.Items.Add(ItemMagnetOption);
            generalMenu.Items.Add(NoGrassOption);
            generalMenu.Items.Add(new Separator());
            generalMenu.Items.Add(FitDayZToScreenOption);
            generalMenu.Items.Add(saveSettingsOption);
            generalMenu.Items.Add(ExitOption);
            D3DMenu.SubMenus.Add(generalMenu);

#endregion

            #region UI

            SubMenu uiMenu = new SubMenu("UI");
            uiBackOption = new Back();
            UIShowScoreboardOption = new BoolSwitch("uiShowScoreboard", "Scoreboard", true, true);
            UILocalPlayerValuesOption = new BoolSwitch("uiShowStats", "Show Stats", true, true);

            TopHUDEnabledOption = new BoolSwitch("tupHUDEnabled", "Enabled", true, true);
            TopHUDShowFPSOption = new BoolSwitch("topHUDShowFPS", "FPS", true, true);
            TopHUDShowServerNameOption = new BoolSwitch("topHUDShowServerName", "Server Name", true, true);
            TopHUDShowLocalPlayerPosition = new BoolSwitch("topHUDShowLocalPosition", "Local Player Position", true, true);
            TopHUDShowNearestPlayers = new BoolSwitch("topHUDShowNearestPlayers", "Nearest Players", true, true);
            TopHUDShowItemStealerEntity = new BoolSwitch("topHUDShowNexItemsStealerPlayer", "Next Item Stealer Player", true, true);
            TopHUDShowEnemyCount = new BoolSwitch("topHUDShowEnemyCount", "Enemy count", true, true);


            CrosshairEnabledOption = new BoolSwitch("crosshairEnabled", "Enabled", false, true);
            CrosshairSizeOption = new ValueSelection("crosshairSize", "Size", 10, 30, 1, 1, true);
            CrosshairThicknessOption = new ValueSelection("crosshairThickness", "Thickness", 1, 5, 1, 0.5f, true);
            CrosshairTypeOption = new CrosshairType("crosshairType", "Type", CrosshairType.Type.Type1, true);
            CrosshairColorOption = new ColorSelection("crosshairColor", "Color", ColorSelection.Colors.White, true);

            uiMenu.Items.Add(uiBackOption);
#if FULL_VERSION
            uiMenu.Items.Add(UIShowScoreboardOption);
#endif
            uiMenu.Items.Add(UILocalPlayerValuesOption);
            uiMenu.Items.Add(new Separator("Top HUD"));
            uiMenu.Items.Add(TopHUDEnabledOption);
            uiMenu.Items.Add(TopHUDShowFPSOption);
            uiMenu.Items.Add(TopHUDShowServerNameOption);
            uiMenu.Items.Add(TopHUDShowLocalPlayerPosition);
            uiMenu.Items.Add(TopHUDShowNearestPlayers);
            //uiMenu.Items.Add(TopHUDShowItemStealerEntity);
            uiMenu.Items.Add(TopHUDShowEnemyCount);
            uiMenu.Items.Add(new Separator("Crosshair"));
            uiMenu.Items.Add(CrosshairEnabledOption);
            uiMenu.Items.Add(CrosshairSizeOption);
            uiMenu.Items.Add(CrosshairThicknessOption);
            uiMenu.Items.Add(CrosshairTypeOption);
            uiMenu.Items.Add(CrosshairColorOption);

            D3DMenu.SubMenus.Add(uiMenu);

            #endregion

            #region ESP

            SubMenu espMenu = new SubMenu("ESP");
            espBackOption = new Back();
            EspPlayersOption = new BoolSwitch("espShowPlayers", "Show Players", true, true);
            EspOnlyTNPlayers = new BoolSwitch("espShowOnlyTNPlayers", "Only TN Players", false, true);
            EspPlayersLineOption = new BoolSwitch("espShowLinesToPlayers", "Lines to Players", true, true);
            EspPlayersLinePositionOption = new LinePosition("espLinePlayerPosition", "Line Position", LinePosition.Position.Bottom, true);
            EspPlayerTemparatureOption = new BoolSwitch("espShowPlayerTemperature", "Player Temperature", false, true);
            EspSmallPlayerStats = new BoolSwitch("espCollapsePlayerStats", "Collapse Player stats", false, true);
            EspDeadPlayerOption = new BoolSwitch("espShowDeadPlayer", "Dead Players", true, true);
            EspZombiesOption = new BoolSwitch("espShowZombies", "Zombies", true, true);
            EspShowHeadMarker = new BoolSwitch("espShowHeadMarker", "Show Head", true, true);
            EspRoundedBoxesOption = new BoolSwitch("espOnlyBoxEdges", "Only Box Edges", true, true);
            EspNearItemsOption = new BoolSwitch("espShowNearItems", "Near", false, true);
            EspFarDroppedItemsOption = new BoolSwitch("espShowFarDroppedItems", "Far Dropped", false, true);
            EspNearDroppedOption = new BoolSwitch("espShowNearDroppedItems", "Near Dropped", false, true);
            EspAirfieldsOption = new BoolSwitch("espShowAirfields", "Airfields", true, true);
            EspMainCitiesOption = new BoolSwitch("espShowCities", "Cities", false, true);
            EspShowOccupiedVehicles = new BoolSwitch("espShowOccupiedVehicles", "Show occupied vehicles", true, true);
            EspShowOccupiedVehiclesLine = new BoolSwitch("espShowOccupiedVehicleLines", "Show occupied vehicles Line", true, true);
            EspOccupiedVehiclesLinePosition = new LinePosition("espShowOccupiedVehicleLinesPosition", "Occupied vehicles Line Pos", LinePosition.Position.Bottom, true);
            EspCarsOption = new BoolSwitch("espShowCars", "Cars/Trucks", true, true);
            EspShipsOption = new BoolSwitch("espShowShips", "Ships/Boats", true, true);
            EspHelicopterOption = new BoolSwitch("espShowHelicopters", "Helicopter", true, true);
            EspPlanesOption = new BoolSwitch("espShowPlanes", "Planes", true, true);
            EspWreckOption = new BoolSwitch("espShowHeliCrashes", "Heli Crashes", true, true);
            EspBulletsOption = new BoolSwitch("espShowBullets", "Bullets", true, true);
            espMenu.Items.Add(espBackOption);
#if FULL_VERSION
            espMenu.Items.Add(new Separator("Items"));
            espMenu.Items.Add(EspNearItemsOption);
            espMenu.Items.Add(EspFarDroppedItemsOption);
            espMenu.Items.Add(EspNearDroppedOption);
#endif
            espMenu.Items.Add(new Separator("Entities"));
            espMenu.Items.Add(EspPlayersOption);
            espMenu.Items.Add(EspPlayersLineOption);
            espMenu.Items.Add(EspPlayersLinePositionOption);
            espMenu.Items.Add(EspPlayerTemparatureOption);
            espMenu.Items.Add(EspSmallPlayerStats);
            espMenu.Items.Add(EspDeadPlayerOption);
            espMenu.Items.Add(EspZombiesOption);
            //espMenu.Items.Add(EspOnlyTNPlayers);
            espMenu.Items.Add(new Separator("ESP Settings"));
            espMenu.Items.Add(EspShowHeadMarker);
            espMenu.Items.Add(EspRoundedBoxesOption);
            espMenu.Items.Add(new Separator("Locations"));
            espMenu.Items.Add(EspAirfieldsOption);
            espMenu.Items.Add(EspMainCitiesOption);
            espMenu.Items.Add(new Separator("Vehicles"));
            espMenu.Items.Add(EspCarsOption);
            espMenu.Items.Add(EspShipsOption);
            espMenu.Items.Add(EspHelicopterOption);
            espMenu.Items.Add(EspPlanesOption);
            espMenu.Items.Add(EspShowOccupiedVehicles);
            espMenu.Items.Add(EspShowOccupiedVehiclesLine);
            espMenu.Items.Add(EspOccupiedVehiclesLinePosition);
            espMenu.Items.Add(new Separator());
            espMenu.Items.Add(EspWreckOption);
            espMenu.Items.Add(EspBulletsOption);
            D3DMenu.SubMenus.Add(espMenu);

            #endregion

            #region ESP General Colors

            SubMenu generalESPSettings = new SubMenu("Entity Colors", 1);
            espSettingsBackOption = new Back();
            PlayerColorOption = new ColorSelection("colorPlayer", "Player Color", ColorSelection.Colors.Cyan, true);
            PlayerLineColorOption = new ColorSelection("colorPlayerLine", "Player Line Color", ColorSelection.Colors.Cyan, true);
            SuspiciousPlayerColorOption = new ColorSelection("colorSuspicousPlayer", "Suspicious Player Color", ColorSelection.Colors.Red, true);
            PlayerLoggingOffColorOption = new ColorSelection("colorLoggingOffPlayer", "Logging off Player Color", ColorSelection.Colors.Pink, true);
            PlayerLoggingOffLineColorOption = new ColorSelection("colorLoggingOffPlayerLine", "Logging off Player Line Color", ColorSelection.Colors.Pink, true);
            TNColorOption = new ColorSelection("colorTN", "TN Color", ColorSelection.Colors.Orange, true);
            TNLineColorOption = new ColorSelection("colorTNLine", "TN Line Color", ColorSelection.Colors.Orange, true);
            ZombieColorOption = new ColorSelection("colorZombie", "Zombie Color", ColorSelection.Colors.Red, true);
            AirfieldsColorOption = new ColorSelection("colorAirfields", "Airfields Color", ColorSelection.Colors.White, true);
            OccupiedVehicleColorOption = new ColorSelection("colorOccupiedVehicle", "Occupied Vehicle Color", ColorSelection.Colors.Red, true);
            OccupiedVehicleLineColorOption = new ColorSelection("colorOccupiedVehicleLine", "Occupied Vehicle Line Color", ColorSelection.Colors.Red, true);
            CarsColorOption = new ColorSelection("colorCar", "Car/Truck Color", ColorSelection.Colors.Yellow, true);
            ShipsColorOption = new ColorSelection("colorShip", "Ships/Boats Color", ColorSelection.Colors.Yellow, true);
            HelicopterColorOption = new ColorSelection("colorHelicopter", "Helicopter Color", ColorSelection.Colors.Yellow, true);
            PlanesColorOption = new ColorSelection("colorPlanes", "Planes Color", ColorSelection.Colors.Yellow, true);
            WreckColorOption = new ColorSelection("ColorHeliCrash", "Heli Crashes Color", ColorSelection.Colors.OrangeRed, true);
            generalESPSettings.Items.Add(espSettingsBackOption);
            generalESPSettings.Items.Add(PlayerColorOption);
            generalESPSettings.Items.Add(PlayerLineColorOption);
            generalESPSettings.Items.Add(SuspiciousPlayerColorOption);
            generalESPSettings.Items.Add(ZombieColorOption);
            generalESPSettings.Items.Add(new Separator());
            generalESPSettings.Items.Add(PlayerLoggingOffColorOption);
            generalESPSettings.Items.Add(PlayerLoggingOffLineColorOption);
            generalESPSettings.Items.Add(new Separator());
            generalESPSettings.Items.Add(TNColorOption);
            generalESPSettings.Items.Add(TNLineColorOption);
            generalESPSettings.Items.Add(new Separator());
            generalESPSettings.Items.Add(AirfieldsColorOption);
            generalESPSettings.Items.Add(new Separator());
            generalESPSettings.Items.Add(OccupiedVehicleColorOption);
            generalESPSettings.Items.Add(OccupiedVehicleLineColorOption);
            generalESPSettings.Items.Add(CarsColorOption);
            generalESPSettings.Items.Add(ShipsColorOption);
            generalESPSettings.Items.Add(HelicopterColorOption);
            generalESPSettings.Items.Add(PlanesColorOption);
            generalESPSettings.Items.Add(WreckColorOption);
            D3DMenu.SubMenus.Add(generalESPSettings);

            #endregion

            #region ESP Items

            SubMenu itemsMenu = new SubMenu("Items", 1);
            itemsBackOption = new Back();

            WeaponsOption = new BoolSwitch("espShowWeapons", "Weapons", true, true);
            AttachmentsOption = new BoolSwitch("espShowAttachments", "Attachments", true, true);
            MagsOption = new BoolSwitch("espShowMags", "Mags", true, true);
            AmmoOption = new BoolSwitch("espShowAmmo", "Ammo", true, true);
            GrenadesOption = new BoolSwitch("espShowGrenades", "Grenades", true, true);
            PistolsOption = new BoolSwitch("espShowPistols", "Pistols", false, true);
            MeeleOption = new BoolSwitch("espShowMeele", "Meele", false, true);
            ConsumablesOption = new BoolSwitch("espShowConsumables", "Consumables", false, true);
            VestsOption = new BoolSwitch("espShowVests", "Vests", true, true);
            EyewearOption = new BoolSwitch("espShowEyewear", "Eyewear", false, true);
            GlovesOption = new BoolSwitch("espShowGloves", "Gloves", false, true);
            HeadgearOption = new BoolSwitch("espShowHeadgear", "Headgear", false, true);
            MasksOption = new BoolSwitch("espShowMasks", "Masks", false, true);
            PantsOption = new BoolSwitch("espShowPants", "Pants", false, true);
            ShoesOption = new BoolSwitch("espShowShoes", "Shoes", false, true);
            ShirtJacketsOption = new BoolSwitch("espShowShirtJackets", "Shirts & Jackets", false, true);
            BackpacksOption = new BoolSwitch("espShowBackpacks", "Backpacks", false, true);
            ContainerOption = new BoolSwitch("espShowContainers", "Containers", false, true);
            MedicalOption = new BoolSwitch("espShowMedical", "Medical", false, true);
            DrinkOption = new BoolSwitch("espShowDrinks", "Drinks", false, true);
            FoodOption = new BoolSwitch("espShowFood", "Food", false, true);
            ToolsOption = new BoolSwitch("espShowTools", "Tools", false, true);
            CraftingOption = new BoolSwitch("espShowCrafting", "Crafting", false, true);
            MiscOption = new BoolSwitch("espShowMisc", "Misc", false, true);
            UnkownOption = new BoolSwitch("espShowUnknown", "Unknown", false, true);

            itemsMenu.Items.Add(itemsBackOption);
            itemsMenu.Items.Add(WeaponsOption);
            itemsMenu.Items.Add(AttachmentsOption);
            itemsMenu.Items.Add(MagsOption);
            itemsMenu.Items.Add(AmmoOption);
            itemsMenu.Items.Add(GrenadesOption);
            itemsMenu.Items.Add(PistolsOption);
            itemsMenu.Items.Add(MeeleOption);
            itemsMenu.Items.Add(ConsumablesOption);
            itemsMenu.Items.Add(VestsOption);
            itemsMenu.Items.Add(EyewearOption);
            itemsMenu.Items.Add(GlovesOption);
            itemsMenu.Items.Add(HeadgearOption);
            itemsMenu.Items.Add(MasksOption);
            itemsMenu.Items.Add(PantsOption);
            itemsMenu.Items.Add(ShoesOption);
            itemsMenu.Items.Add(ShirtJacketsOption);
            itemsMenu.Items.Add(BackpacksOption);
            itemsMenu.Items.Add(ContainerOption);
            itemsMenu.Items.Add(MedicalOption);
            itemsMenu.Items.Add(DrinkOption);
            itemsMenu.Items.Add(FoodOption);
            itemsMenu.Items.Add(ToolsOption);
            itemsMenu.Items.Add(CraftingOption);
            itemsMenu.Items.Add(MiscOption);
            itemsMenu.Items.Add(UnkownOption);
#if FULL_VERSION
            D3DMenu.SubMenus.Add(itemsMenu);
#endif

            #endregion

            #region ESP Item Colors

            itemColorsBackOption = new Back();
            SubMenu itemColorsMenu = new SubMenu("Item Colors", 1);
            WeaponsColorOption = new ColorSelection("espColorWeapons", "Weapons Color", ColorSelection.Colors.OrangeRed, true);
            AttachmentsColorOption = new ColorSelection("espColorAttachments", "Attachments Color", ColorSelection.Colors.Orange, true);
            MagsColorOption = new ColorSelection("espColorMags", "Mags Color", ColorSelection.Colors.LightBrown, true);
            AmmoColorOption = new ColorSelection("espColorAmmo", "Ammo Color", ColorSelection.Colors.YellowGreen, true);
            GrenadesColorOption = new ColorSelection("espColorGrenades", "Grenades Color", ColorSelection.Colors.GreenYellow, true);
            PistolsColorOption = new ColorSelection("espColorPistols", "Pistols Color", ColorSelection.Colors.LightGreen, true);
            MeeleColorOption = new ColorSelection("espColorMeele", "Meele Color", ColorSelection.Colors.LightGreen, true);
            ConsumablesColorOption = new ColorSelection("espColorConsumables", "Consumables Color", ColorSelection.Colors.LightGreen, true);
            VestsColorOption = new ColorSelection("espColorVests", "Vests Color", ColorSelection.Colors.PinkViolet, true);
            EyewearColorOption = new ColorSelection("espColorEyewear", "Eyewear Color", ColorSelection.Colors.Violet, true);
            GlovesColorOption = new ColorSelection("espColorGloves", "Gloves Color", ColorSelection.Colors.Violet, true);
            HeadgearColorOption = new ColorSelection("espColorHeadgear", "Headgear Color", ColorSelection.Colors.Violet, true);
            MasksColorOption = new ColorSelection("espColorMasks", "Masks Color", ColorSelection.Colors.Violet, true);
            PantsColorOption = new ColorSelection("espColorPants", "Pants Color", ColorSelection.Colors.Violet, true);
            ShoesColorOption = new ColorSelection("espColorShoes", "Shoes Color", ColorSelection.Colors.Violet, true);
            ShirtJacketsColorOption = new ColorSelection("espColorShirtJackets", "Shirts & Jackets Color", ColorSelection.Colors.Violet, true);
            BackpacksColorOption = new ColorSelection("espColorBackpacks", "Backpack Color", ColorSelection.Colors.Violet, true);
            ContainerColorOption = new ColorSelection("espColorContainer", "Container Color", ColorSelection.Colors.LightBlue, true);
            MedicalColorOption = new ColorSelection("espColorMedical", "Medical Color", ColorSelection.Colors.Blue, true);
            DrinkColorOption = new ColorSelection("espColorDrink", "Drink Color", ColorSelection.Colors.Green, true);
            FoodColorOption = new ColorSelection("espColorFood", "Food Color", ColorSelection.Colors.Green, true);
            ToolsColorOption = new ColorSelection("espColorTools", "Tools Color", ColorSelection.Colors.Pink, true);
            CraftingColorOption = new ColorSelection("espColorCrafting", "Crafting Color", ColorSelection.Colors.Pink, true);
            MiscColorOption = new ColorSelection("espColorMisc", "Misc Color", ColorSelection.Colors.LightGray, true);
            UnknownColorOption = new ColorSelection("espColorUnknown", "Unknown Color", ColorSelection.Colors.White, true);

            itemColorsMenu.Items.Add(itemColorsBackOption);
            itemColorsMenu.Items.Add(WeaponsColorOption);
            itemColorsMenu.Items.Add(AttachmentsColorOption);
            itemColorsMenu.Items.Add(MagsColorOption);
            itemColorsMenu.Items.Add(AmmoColorOption);
            itemColorsMenu.Items.Add(GrenadesColorOption);
            itemColorsMenu.Items.Add(PistolsColorOption);
            itemColorsMenu.Items.Add(MeeleColorOption);
            itemColorsMenu.Items.Add(VestsColorOption);
            itemColorsMenu.Items.Add(EyewearColorOption);
            itemColorsMenu.Items.Add(GlovesColorOption);
            itemColorsMenu.Items.Add(HeadgearColorOption);
            itemColorsMenu.Items.Add(MasksColorOption);
            itemColorsMenu.Items.Add(PantsColorOption);
            itemColorsMenu.Items.Add(ShoesColorOption);
            itemColorsMenu.Items.Add(ShirtJacketsColorOption);
            itemColorsMenu.Items.Add(BackpacksColorOption);
            itemColorsMenu.Items.Add(ContainerColorOption);
            itemColorsMenu.Items.Add(MedicalColorOption);
            itemColorsMenu.Items.Add(DrinkColorOption);
            itemColorsMenu.Items.Add(FoodColorOption);
            itemColorsMenu.Items.Add(ToolsColorOption);
            itemColorsMenu.Items.Add(CraftingColorOption);
            itemColorsMenu.Items.Add(MiscColorOption);
            itemColorsMenu.Items.Add(UnknownColorOption);
            
#if FULL_VERSION
            D3DMenu.SubMenus.Add(itemColorsMenu);
#endif

            #endregion

            #region Player Inventories

            SubMenu inventoriesMenu = new SubMenu("Inventories");
                        inventoriesBackOption = new Back();
                        InventoriesEnabledOption = new BoolSwitch("showInventories", "Show Inventories", false, true);
                        InventoriesSelectedPlayerOption = new PlayerSelection("Player");

                        ItemStealerPrimaryOption = new BoolSwitch("inventoriesStealPrimary", "Primary", false, false);
                        ItemStealerMeleeOption = new BoolSwitch("inventoriesStealMeele", "Meele", false, false);
                        ItemStealerHelmetOption = new BoolSwitch("inventoriesStealHelmet", "Helmet", false, false);
                        ItemStealerMaskOption = new BoolSwitch("inventoriesStealMask", "Mask", false, false);
                        ItemStealerGlassesOption = new BoolSwitch("inventoriesStealGlasses", "Glasses", false, false);
                        ItemStealerChestOption = new BoolSwitch("inventoriesStealChest", "Chest", false, false);
                        ItemStealerPantsOption = new BoolSwitch("inventoriesStealPants", "Pants", false, false);
                        ItemStealerShoesOption = new BoolSwitch("inventoriesStealShoes", "Shoes", false, false);
                        ItemStealerVestOption = new BoolSwitch("inventoriesStealVest", "Vest", false, false);
                        ItemStealerGlovesOption = new BoolSwitch("inventoriesStealGloves", "Gloves", false, false);
                        ItemStealerBackpackOption = new BoolSwitch("inventoriesStealBackpack", "Backpack", false, false);

                        InventriesShowPrimaryOption = new BoolSwitch("inventoriesShowPrimary", "Primary", true, true);
                        InventriesShowMeleeOption = new BoolSwitch("inventoriesShowMelee", "Melee", false, true);
                        InventriesShowHelmetOption = new BoolSwitch("inventoriesShowHelmet", "Helmet", false, true);
                        InventriesShowMaskOption = new BoolSwitch("inventoriesShowMask", "Mask", false, true);
                        InventriesShowGlassesOption = new BoolSwitch("inventoriesShowGlasses", "Glasses", false, true);
                        InventriesShowChestOption = new BoolSwitch("inventoriesShowChest", "Chest", true, true);
                        InventriesShowPantsOption = new BoolSwitch("inventoriesShowPants", "Pants", true, true);
                        InventriesShowShoesOption = new BoolSwitch("inventoriesShowShoes", "Shoes", false, true);
                        InventriesShowVestOption = new BoolSwitch("inventoriesShowVest", "Vest", true, true);
                        InventriesShowGlovesOption = new BoolSwitch("inventoriesShowGloves", "Gloves", false, true);
                        InventriesShowBackpackOption = new BoolSwitch("inventoriesShowBackpack", "Backpack", true, true);

                        inventoriesMenu.Items.Add(inventoriesBackOption);
                        inventoriesMenu.Items.Add(InventoriesEnabledOption);
                        inventoriesMenu.Items.Add(InventoriesSelectedPlayerOption);
                        inventoriesMenu.Items.Add(new Separator("Steal"));
                        inventoriesMenu.Items.Add(ItemStealerPrimaryOption);
                        inventoriesMenu.Items.Add(ItemStealerMeleeOption);
                        inventoriesMenu.Items.Add(ItemStealerHelmetOption);
                        inventoriesMenu.Items.Add(ItemStealerMaskOption);
                        inventoriesMenu.Items.Add(ItemStealerGlassesOption);
                        inventoriesMenu.Items.Add(ItemStealerChestOption);
                        inventoriesMenu.Items.Add(ItemStealerPantsOption);
                        inventoriesMenu.Items.Add(ItemStealerShoesOption);
                        inventoriesMenu.Items.Add(ItemStealerVestOption);
                        inventoriesMenu.Items.Add(ItemStealerGlovesOption);
                        inventoriesMenu.Items.Add(ItemStealerBackpackOption);
                        inventoriesMenu.Items.Add(new Separator("Slots to show"));
                        inventoriesMenu.Items.Add(InventriesShowPrimaryOption);
                        inventoriesMenu.Items.Add(InventriesShowMeleeOption);
                        inventoriesMenu.Items.Add(InventriesShowHelmetOption);
                        inventoriesMenu.Items.Add(InventriesShowMaskOption);
                        inventoriesMenu.Items.Add(InventriesShowGlassesOption);
                        inventoriesMenu.Items.Add(InventriesShowChestOption);
                        inventoriesMenu.Items.Add(InventriesShowPantsOption);
                        inventoriesMenu.Items.Add(InventriesShowShoesOption);
                        inventoriesMenu.Items.Add(InventriesShowVestOption);
                        inventoriesMenu.Items.Add(InventriesShowGlovesOption);
                        inventoriesMenu.Items.Add(InventriesShowBackpackOption);
            #if FULL_VERSION
                        D3DMenu.SubMenus.Add(inventoriesMenu);
            #endif

            #endregion

            #region Weather

                        SubMenu weatherMenu = new SubMenu("Weather");
                        weatherBackOption = new Back();
                        WeatherOvercastOption = new ValueSelection("weatherOvercast", "Overcast", 0, 1, 0, 0.1f, true);
                        WeatherFogOption = new ValueSelection("weatherFog", "Fog", 0, 1, 0, 0.1f, true);
                        WeatherWindOption = new ValueSelection("weatherWind", "Wind", 0, 1, -1, 0.1f, true);
                        WeatherSunPositionOption = new ValueSelection("weatherSunPosition", "Sun Position", 0, 1, -1, 0.1f, true);
                        WeatherSunnyOption = new BoolSwitch("weatherPresetSunny", "Sunny", false, false);
                        WeatherStormyOption = new BoolSwitch("weatherPresetStormy", "Stormy", false, false);
                        WeatherApocalypticOption = new BoolSwitch("weatherPresetApocalyptic", "Apocalyptic", false, false);
                        weatherMenu.Items.Add(weatherBackOption);
                        weatherMenu.Items.Add(WeatherOvercastOption);
                        weatherMenu.Items.Add(WeatherFogOption);
                        weatherMenu.Items.Add(WeatherWindOption);
                        weatherMenu.Items.Add(WeatherSunPositionOption);
                        weatherMenu.Items.Add(new Separator("Presets"));
                        weatherMenu.Items.Add(WeatherSunnyOption);
                        weatherMenu.Items.Add(WeatherStormyOption);
                        weatherMenu.Items.Add(WeatherApocalypticOption);
                        D3DMenu.SubMenus.Add(weatherMenu);

            #endregion

            #region Radar

            SubMenu radarMenu = new SubMenu("Radar");
            radarBackOption = new Back();
            ShowRadarOption = new BoolSwitch("radarEnables", "Show Radar", false, true);
            RadarZoomOption = new ValueSelection("radarZoom", "Radar Zoom", 0.7f, 2f, 0.1f, 0.02f, true);
            RadarSizeOption = new ValueSelection("radarSize", "Radar Size", 0.8f, 2f, 0.1f, 0.02f, true);
            RadarOpacityOption = new ValueSelection("radarOpacity", "Opacity", 0.1f, 1, 0.1f, 0.1f, true);
            RadarEllipseOption = new BoolSwitch("radarEllipse", "Radar Ellipse", false, true);
            RadarEnemiesOption = new BoolSwitch("radarShowEnemies", "Enemies", true, true);
            RadarEnemyLinesOption = new BoolSwitch("radarShowEnemyLines", "Enemy Lines", true, true);
            RadarZombiesOption = new BoolSwitch("radarShowZombies", "Zombies", true, true);
            RadarTNOption = new BoolSwitch("radarShowTN", "TN", true, true);
            RadarTNLinesOption = new BoolSwitch("radarShowTNLines", "TN Lines", true, true);
            RadarShowDistance = new BoolSwitch("radarShowDistance", "Show Distance", true, true);

            RadarOccupiedVehicle = new BoolSwitch("radarMarkOccupiedVehicles", "Mark occupied Vehicles", true, true);
            RadarOccupiedVehicleLine = new BoolSwitch("radarShowOccupiedVehicleLines", "Occupied Vehicle Lines", true, true);
            RadarCarsOption = new BoolSwitch("radarShowCars", "Cars", false, true);
            RadarShipsOption = new BoolSwitch("radarShowShips", "Ships", false, true);
            RadarHelicoptersOption = new BoolSwitch("radarShowHelicopters", "Helicopters", false, true);
            RadarPlanesOption = new BoolSwitch("radarShowPlanes", "Planes", false, true);
            radarMenu.Items.Add(radarBackOption);
            radarMenu.Items.Add(ShowRadarOption);
            radarMenu.Items.Add(RadarOpacityOption);
            radarMenu.Items.Add(RadarShowDistance);
            //radarMenu.Items.Add(RadarZoomOption);
            //radarMenu.Items.Add(RadarSizeOption);
            //radarMenu.Items.Add(RadarEllipseOption);
            radarMenu.Items.Add(new Separator());
            radarMenu.Items.Add(RadarEnemiesOption);
            radarMenu.Items.Add(RadarEnemyLinesOption);
            radarMenu.Items.Add(RadarZombiesOption);
            radarMenu.Items.Add(RadarTNOption);
            radarMenu.Items.Add(RadarTNLinesOption);
            radarMenu.Items.Add(RadarOccupiedVehicle);
            radarMenu.Items.Add(RadarOccupiedVehicleLine);
            radarMenu.Items.Add(RadarCarsOption);
            radarMenu.Items.Add(RadarShipsOption);
            radarMenu.Items.Add(RadarHelicoptersOption);
            radarMenu.Items.Add(RadarPlanesOption);
            D3DMenu.SubMenus.Add(radarMenu);

            #endregion

            #region Player

                        SubMenu playerMenu = new SubMenu("Player");
                        playerBackOption = new Back();
                        NoRecoilOption = new BoolSwitch("playerNoRecoil", "No Recoil", true, true);
                        NoFatigueOption = new BoolSwitch("playerNoFatigue", "No Fatigue", true, true);
                        playerMenu.Items.Add(playerBackOption);
                        playerMenu.Items.Add(NoRecoilOption);
                        playerMenu.Items.Add(NoFatigueOption);
                        D3DMenu.SubMenus.Add(playerMenu);

            #endregion

            #region Info

                        //SubMenu infoMenu = new SubMenu("Info");
                        //infoBackOption = new Back();
                        //infoToggleMenu = new D3DLabelItem("Insert  =  Toggle menu");
                        //infoToggleRadar = new D3DLabelItem("Num5  =  Toggle radar");
                        //infoZoomOut = new D3DLabelItem("Num2  =  Zoom radar out");
                        //infoZoomIn = new D3DLabelItem("Num8  =  Zoom radar in");
                        //infoSizeDown = new D3DLabelItem("Num3  =  Size radar down");
                        //infoSizeUp = new D3DLabelItem("Num9  =  Size radar up");
                        //infoMurderModeAuto = new D3DLabelItem("Strg  =  Auto Murder Mode");
                        //infoMenu.Items.Add(infoBackOption);
                        //infoMenu.Items.Add(infoToggleMenu);
                        //infoMenu.Items.Add(new Separator());
                        //infoMenu.Items.Add(infoToggleRadar);
                        //infoMenu.Items.Add(infoZoomOut);
                        //infoMenu.Items.Add(infoZoomIn);
                        //infoMenu.Items.Add(infoSizeDown);
                        //infoMenu.Items.Add(infoSizeUp);
                        //infoMenu.Items.Add(new Separator());
                        //infoMenu.Items.Add(infoMurderModeAuto);
                        //D3D.MenuItems.Add(infoMenu);

            #endregion
        }

        public void DrawMenu()
        {
           if (!D3DMenu.IsVisible)
                return;

            float menuBottom = 0;

            //Main Menu
            if (D3DMenu.SelectedSubMenuIndex == -1)
            {
                directXUI.DrawTransparentBox(MenuPositionX, MenuPositionY, MenuWidth, 60 + (D3DMenu.SubMenus.Count * MenuItemHeight), new Color(216, 216, 216), 238);
                directXUI.DrawBox(MenuPositionX, MenuPositionY, MenuWidth, 60 + (D3DMenu.SubMenus.Count * MenuItemHeight), 1, new Color(60, 60, 60));
                directXUI.DrawTransparentBox(MenuPositionX, MenuPositionY, MenuWidth, MenuTitleHeight, new Color(40, 40, 40), 248);
                directXUI.DrawBox(MenuPositionX, MenuPositionY, MenuWidth, MenuTitleHeight, 1, Color.Black);
                directXUI.DrawShadowText(directXUI.HeadlineFont, "Main", MenuPositionX + 5, MenuPositionY + 3, Color.White);

                foreach (SubMenu menuItem in D3DMenu.SubMenus)
                {
                    float x = MenuTextX + (menuItem.Layer * MenuTextLineHeight);

                    if (menuItem.Enabled)
                        directXUI.DrawBaseText(directXUI.BoldFont, "[ " + menuItem.Title + " ]", x, MenuTextY, new Color(53, 53, 53));
                    else
                        directXUI.DrawBaseText(directXUI.BoldFont, "[ " + menuItem.Title + "] DISABLED", x, MenuTextY, new Color(112, 112, 112));
                    MenuTextY = MenuTextY + MenuItemHeight;
                }
                MenuTextY = MenuTextYStart;

                menuBottom = MenuTextYStart + (D3DMenu.SubMenus.Count * MenuItemHeight) + 11;
            }
            else
            {
                //Sub Menu

                D3DMenu.SelectedSubMenuCount = D3DMenu.SubMenus[D3DMenu.SelectedSubMenuIndex].Items.Where(sub => sub.DrawMe).Count();

                float menuHeight = 60 + (D3DMenu.SelectedSubMenuCount * MenuItemHeight);

                SubMenu subMenu = D3DMenu.SubMenus[D3DMenu.SelectedSubMenuIndex];
                directXUI.DrawTransparentBox(MenuPositionX, MenuPositionY, MenuWidth, menuHeight, new Color(216, 216, 216), 238);
                directXUI.DrawBox(MenuPositionX, MenuPositionY, MenuWidth, menuHeight, 1, new Color(60, 60, 60));
                directXUI.DrawTransparentBox(MenuPositionX, MenuPositionY, MenuWidth, MenuTitleHeight, new Color(40, 40, 40), 248);
                directXUI.DrawBox(MenuPositionX, MenuPositionY, MenuWidth, MenuTitleHeight, 1, Color.Black);
                directXUI.DrawShadowText(directXUI.HeadlineFont, subMenu.Title, MenuPositionX + 5, MenuPositionY + 3, Color.White);
                //DrawLine(MenuTextX - 3, MenuTextY, MenuTextX - 3, menuHeight + MenuPositionY, 0.2f, Color.Black);

                foreach (MenuItemBase menuItem in subMenu.Items)
                {
                    if (menuItem.DrawMe)
                        directXUI.DrawBaseText(directXUI.BaseFont, menuItem.Title, MenuTextX, MenuTextY, new Color(20, 20, 20));

                    if (menuItem.GetType() == typeof(BoolSwitch))
                    {
                        BoolSwitch boolMenu = menuItem as BoolSwitch;
                        if (boolMenu.Enabled)
                        {
                            if (boolMenu.State)
                                directXUI.DrawShadowText(directXUI.BaseFont, "ON", MenuValueX, MenuTextY, Color.LightGreen);
                            else
                                directXUI.DrawShadowText(directXUI.BaseFont, "OFF", MenuValueX, MenuTextY, Color.Red);
                        }
                        else
                        {
                            directXUI.DrawShadowText(directXUI.BaseFont, "DISABLED", MenuValueX, MenuTextY, Color.Gray);
                        }
                    }
                    else if (menuItem.GetType() == typeof(ValueSelection))
                    {
                        ValueSelection valueMenu = menuItem as ValueSelection;
                        directXUI.DrawShadowText(directXUI.BaseFont, valueMenu.Value.ToString("0.00"), MenuValueX, MenuTextY, Color.LightGreen);
                    }
                    else if (menuItem.GetType() == typeof(ColorSelection))
                    {
                        ColorSelection colorMenu = menuItem as ColorSelection;
                        directXUI.DrawFilledBox(MenuColorX, MenuTextY + 2, 10, 10, colorMenu.GetColor());
                    }
                    else if (menuItem.GetType() == typeof(Separator))
                    {
                        if (menuItem.Title != "separator" && menuItem.Title != "")
                        {
                            Size2F titleSize = directXUI.MeasureText(menuItem.Title, directXUI.SeparatorFont);
                            float totalWidth = MenuWidth - MenuSeparatorPadding;
                            float singleLineWidth = (totalWidth / 2) - (titleSize.Width / 2) - MenuSeparatorPadding;

                            float line1X1 = MenuTextX;
                            float line1Y1 = MenuTextY;
                            float line1X2 = ((MenuWidth / 2) + MenuPositionX) - (titleSize.Width / 2) - MenuSeparatorPadding;
                            float line1Y2 = MenuTextY;

                            float line2X1 = line1X2 + (MenuSeparatorPadding * 2) + titleSize.Width;
                            float line2Y1 = MenuTextY;
                            float line2X2 = MenuPositionX + MenuWidth - MenuSeparatorPadding;
                            float line2Y2 = MenuTextY;

                            directXUI.DrawLine(line1X1, line1Y1 - 1, line1X2, line1Y2 - 1, MenuSeparatorHeight, Color.Black);
                            directXUI.DrawLine(line2X1, line2Y1 - 1, line2X2, line2Y2 - 1, MenuSeparatorHeight, Color.Black);
                            directXUI.DrawBaseText(directXUI.SeparatorFont, menuItem.Title, line1X2 + MenuSeparatorPadding, MenuTextY - 7, Color.Black);
                        }
                        else
                        {
                            directXUI.DrawLine(MenuTextX, MenuTextY - 1, MenuWidth - MenuSeparatorPadding, MenuTextY - 1, MenuSeparatorHeight, Color.Black);
                        }
                    }
                    else if (menuItem.GetType() == typeof(LinePosition))
                    {
                        LinePosition positionMenu = menuItem as LinePosition;
                        directXUI.DrawShadowText(directXUI.BaseFont, positionMenu.SelectedPosition.ToString(), MenuValueX, MenuTextY, Color.DimGray);
                    }
                    else if (menuItem.GetType() == typeof(MurderModeType))
                    {
                        MurderModeType murderModeTypeMenu = menuItem as MurderModeType;
                        if (murderModeTypeMenu.Enabled)
                            directXUI.DrawShadowText(directXUI.BaseFont, murderModeTypeMenu.SelectedType.ToString(), MenuValueX, MenuTextY, Color.DimGray);
                        else
                            directXUI.DrawShadowText(directXUI.BaseFont, "DISABLED", MenuValueX, MenuTextY, Color.Gray);
                    }
                    else if (menuItem.GetType() == typeof(MurderModePosition))
                    {
                        MurderModePosition murderModePositionMenu = menuItem as MurderModePosition;
                        if (murderModePositionMenu.Enabled)
                            directXUI.DrawShadowText(directXUI.BaseFont, murderModePositionMenu.SelectedPosition.ToString(), MenuValueX, MenuTextY, Color.DimGray);
                        else
                            directXUI.DrawShadowText(directXUI.BaseFont, "DISABLED", MenuValueX, MenuTextY, Color.Gray);
                    }
                    else if (menuItem.GetType() == typeof(PlayerSelection))
                    {
                        PlayerSelection itemStealerTypeMenu = menuItem as PlayerSelection;
                        if (itemStealerTypeMenu.Enabled)
                        {
                            if (itemStealerTypeMenu.SelectedEnemy == null)
                                directXUI.DrawShadowText(directXUI.BaseFont, "-", MenuValueX, MenuTextY, Color.DimGray);
                            else
                                directXUI.DrawShadowText(directXUI.BaseFont, itemStealerTypeMenu.SelectedEnemy.Name.ToString(), MenuValueX, MenuTextY, Color.DimGray);
                        }
                        else
                        {
                            directXUI.DrawShadowText(directXUI.BaseFont, "DISABLED", MenuValueX, MenuTextY, Color.Gray);
                        }
                    }
                    else if (menuItem.GetType() == typeof(CrosshairType))
                    {
                        CrosshairType crosshairTypeMenu = menuItem as CrosshairType;
                        directXUI.DrawShadowText(directXUI.BaseFont, crosshairTypeMenu.SelectedType.ToString(), MenuValueX, MenuTextY, Color.DimGray);
                    }
                    else if (menuItem.GetType() == typeof(Back))
                    {
                        directXUI.DrawBaseText(directXUI.BaseFont, "< Back", MenuTextX, MenuTextYStart, new Color(38, 119, 219));
                    }
                    else if (menuItem.GetType() == typeof(ExitApp))
                    {
                        directXUI.DrawBaseText(directXUI.BaseFont, "Exit", MenuTextX, MenuTextY, Color.DarkRed);
                    }

                    if (menuItem.DrawMe)
                        MenuTextY = MenuTextY + MenuItemHeight;
                }

                MenuTextY = MenuTextYStart;

                menuBottom = MenuTextYStart + (D3DMenu.SelectedSubMenuCount * MenuItemHeight) + 11;
            }

            float navigatorX = MenuNavigatorX;

            int menuItemIndex = 0;
            //Main menu
            if (D3DMenu.SelectedSubMenuIndex < 0)
            {
                menuItemIndex = D3DMenu.MarkedSubMenuIndex;
                navigatorX = (MenuNavigatorX) + (D3DMenu.SubMenus[D3DMenu.MarkedSubMenuIndex].Layer * MenuTextLineHeight);
            }
            else //Submenu
            {
                menuItemIndex = D3DMenu.SelectedSubMenuItem;
            }

            directXUI.DrawBaseText(directXUI.HeadlineFont, ">", navigatorX, MenuTextYStart + (MenuItemHeight * menuItemIndex)+2, Color.Red);

            //Credits
            directXUI.DrawBox(MenuPositionX, menuBottom, MenuWidth, 20, 1, new Color(60, 60, 60));
            directXUI.DrawFilledBox(MenuPositionX, menuBottom, MenuWidth, 20, new Color(60, 60, 60));
            //DrawCreditText("Private TN Hack by SuRTaiNx3 V3.0 (0.46 Cracked)", DirectXMenu.MenuPositionX, menuBottom, Color.Gray, DirectXMenu.MenuWidth);
        }

#endregion
    }
}
