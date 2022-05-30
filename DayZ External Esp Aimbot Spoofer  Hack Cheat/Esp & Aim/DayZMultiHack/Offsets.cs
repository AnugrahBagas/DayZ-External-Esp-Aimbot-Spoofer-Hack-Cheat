using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZMultiHack
{
    public static class Offsets
    {
        public const int SCOREBOARD = 0x1075260; // 0.46

        public static class World
        {
            public const int BASE = 0x10810D0; // 0.46

            public const int INGAME_UI_BASE = 0x10; // 0.46

            public const int PLAYER_LOCAL = 0x15C4; // 0.46
            public const int PLAYER_TABLE = 0x0778; // 0.46

            public const int ANIMAL_TABLE = 0x0A74; // 0.46
            public const int FARANIMAL_TABLE = 0x0B1C; // 0.46

            public const int DROPPED_ITEMS_TABLE = 0x0FBC; // 0.46
            public const int FARDROPPED_ITEMS_TABLE = 0x1064; // 0.46
            public const int NEAR_ITEMS_TABLE = 0x11B4; // 0.46

            public const int BULLET_TABLE = 0x9C8; // 0.46

            public const int NO_GRASS = 0x758; // 0.46

            //public const int NO_RAIN = 0x1740; // 0.52
            public const int DAYTIME = 0x1774; // 0.52
            //public const int FOG = 0x174C; // 0.52

            public const int ACTUAL_OVERCAST = 0x1604; // 0.46
            public const int WANTED_OVERCAST = 0x1608; // 0.46
            public const int SPEED_OVERCAST = 0x160C; // 0.46

            public const int ACTUAL_FOG = 0x1610; // 0.46
            public const int WANTED_FOG = 0x1614; // 0.46

            public const int SPEEDCHANGE_WIND = 0x1618; // 0.46
            public const int ACTUAL_WIND = 0x161C; // 0.46
            public const int WANTED_WIND = 0x1620; // 0.46

            public const int WEATHER_TIME = 0x1644; // 0.46
            public const int NEXT_WEATHER_CHANGE = 0x1648; // 0.46
            public const int SUN_POSITION = 0x163C; // 0.46
        }

        public static class InGameUI
        {
            public const int TARGET_PLAYER = 0x18; // 0.46
            public const int CURSOR_POSITION = 0x20; // 0.46
        }

        public static class Bullet
        {
            public const int UNKNOWN = 0x0294; //Maybe lifetime?
            public const int PARENT = 0x0268;

            public static class VisualState
            {
                public const int BASE = 0x20; // 0.46

                public const int POSITION_VEC3_START = 0x28; // 0.46

                public const int SPEED_VEC3_START = 0x48;
                public const int DROP_VEC3_START = 0x44;
                public const int VELOCITY = 0x5C;
            }
        }

        public static class Transformations
        {
            public const int BASE = 0x109ECE4; // 0.46
            public const int DATA = 0x94; // 0.46
            public const int RIGHT = 0x4; // 0.46
            public const int UP = 0x10; // 0.46
            public const int FORWARD = 0x1C; // 0.46
            public const int MATRIX = 0x54; // 0.46
            public const int PROJD1 = 0xCC; // 0.46
            public const int PROJD2 = 0xD8; // 0.46
            public const int TRANSLATION = 0x28; // 0.46
            public const int FOV = 0x190; // 0.46
        }

        public static class Entity
        {
            public const int ID = 0x07E0; // 0.46

            public const int ISDEAD = 0x027C; // 0.46
            public const int ISLOCAL = 0x019C; // 0.46
            public const int STANCE = 0x12E0; // 0.46
            public const int VARIABLES = 0x0640; // 0.46
            public const int WEAPON = 0x0A78; // 0.46
            public const int STATE = 0x0CE0; // 0.46

            public const int NO_RECOIL = 0x0C34; // 0.46
            public const int NO_FATIGUE_BASE = 0x0C50; // Min 0.99f // 0.46
            public const int NO_FATIGUE_DIFF = 0x0C54; // Max 0.1f // 0.46
            public const int BREATH_HOLDING = 0xC84; // 0.52 10f

            public const int LAND_CONTACT = 0x199; // 0.46
            public const int OBJ_CONTACT = 0x198; // 0.45 ????
            public const int WATER_CONTACT = 0x19A; // 0.46

            public const int DRIVER_POINTER = 0xA00;


            public const int GHOST_MODE = 0x1B0; // Probably 0.50 | 0, 1 or 2 - seems to be an enum 0 or 1 will turn it on, 2 is back to normal
            //0x1B4]+0x4]
            //0x1B5
            //Possibly shoot pepole: 0x1b8
            //Invisible?: 0x1A5

            public static class Inventory
            {
                public const int PRIMARY = 0xA14; // 0.46
                public const int MELEE = 0xA18; // 0.46
                public const int HELMET = 0xA1C; // 0.46
                public const int MASK = 0xA20; // 0.46
                public const int GLASSES = 0xA24; // 0.46
                public const int CHEST = 0xA28; // 0.46
                public const int PANTS = 0xA2C; // 0.46
                public const int SHOES = 0xA30; // 0.46
                public const int VEST = 0xA34; // 0.46
                public const int GLOVES = 0xA38; // 0.46
                public const int BACKPACK = 0xA3C; // 0.46
                public const int FREE = 0xA40; // 0.46

            }

            public static class VisualState
            {
                public const int BASE = 0x20; // 0.46

                public const int POSITION_VEC3_START = 0x28; // 0.46
                public const int HEAD_POSITION_VEC3_START = 0x130; // 0.46

                public const int DIRECTION_X = 0x1C; // 0.46
                public const int DIRECTION_Y = 0x24; // 0.46

                public const int VELOCITY_VEC3_START = 0x4C;
            }

            public static class ObjectClass
            {
                public const int BASE = 0x70; // 0.46
                public const int TYPE = 0x70; // 0.46

                // TEST
                public const int NAME = 0x34; // 0.46
                public const int REAL_NAME = 0x588; // 0.46
            }
        }


        public static class Item_Animal
        {
            public const int ISDEAD = 0x2AC; // 0.52
            public const int CONTAINER = 0x28C; // 0.46
            public const int ISRUINED = 0x27C; // 0.46

            public static class VisualState
            {
                public const int BASE = 0x20; // 0.46

                public const int POSITION_VEC3_START = 0x28; // 0.46
                public const int HEAD_POSITION_VEC3_START = 0x130; // 0.46

                public const int DIRECTION_X = 0x1C; // 0.52
                public const int DIRECTION_Y = 0x24; // 0.52
            }

            public static class ObjectClass
            {
                public const int BASE = 0x70; // 0.46
                public const int TYPE = 0x70; // 0.46
                public const int NAME = 0x34; // 0.46
                public const int REAL_NAME = 0x588; // 0.46
            }
        }

        public static class Weapon
        {
            public static class ObjectClass
            {
                public const int BASE = 0x70; // 0.46
                public const int REAL_NAME = 0x58C; // 0.46
            }
        }
    }
}
