using DayZMultiHack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLibrary;
using UtilsLibrary.MathObjects;

namespace DayZMultiHack
{
    public static class Data
    {
        private static Vector3 buf_forward, buf_up, buf_right, buf_fov, buf_translation, buf_viewport, buf_proj1, buf_proj2;
        private static Dictionary<IntPtr, EntityValues> entityValuesCache = new Dictionary<IntPtr, EntityValues>();


        public static Entity GetEntity(ProcessMemory mem, Transformation transformation, ScoreboardTable scoreboard, IntPtr entityPtr)
        {
            Entity entity = new Entity();
            
            entity.ID = mem.Read<int>(entityPtr + Offsets.Entity.ID);
            entity.Pointer = entityPtr;

            IntPtr visualState = mem.Read<IntPtr>(entityPtr + Offsets.Entity.VisualState.BASE);
            entity.VisualStatePointer = visualState;
            entity.Position = mem.ReadVector3(visualState + Offsets.Entity.VisualState.POSITION_VEC3_START);
            entity.HeadPosition = mem.ReadVector3(visualState + Offsets.Entity.VisualState.HEAD_POSITION_VEC3_START);

            entity.DirectionX = mem.Read<float>(visualState + Offsets.Entity.VisualState.DIRECTION_Y);
            entity.DirectionY = mem.Read<float>(visualState + Offsets.Entity.VisualState.DIRECTION_X);

            entity.ScreenFootPosition = Geometry.W2SN(entity.Position, transformation);
            entity.ScreenHeadPosition = Geometry.W2SN(entity.HeadPosition, transformation);

            //Entity Type
            IntPtr objectClass = mem.Read<IntPtr>(entityPtr + Offsets.Entity.ObjectClass.BASE); // 0.50 changed from 0x70 to 0x50
            IntPtr typePtr = mem.Read<IntPtr>(objectClass + Offsets.Entity.ObjectClass.TYPE);
            entity.TypeString = mem.ReadString(typePtr + 0x8, 8, Encoding.ASCII);
            entity.Type = Entity.ParseType(entity.TypeString);

            // Object name
            IntPtr itemObjectNamePtr = mem.Read<IntPtr>(objectClass + Offsets.Entity.ObjectClass.NAME);
            int itemObjectNameSize = mem.Read<int>(itemObjectNamePtr + 0x4);
            entity.ObjectName = mem.ReadString(itemObjectNamePtr + 0x8, itemObjectNameSize, Encoding.ASCII);

            //// Real Name
            //IntPtr itemRealNamePtr = mem.Read<IntPtr>(objectClass + Offsets.Item_Animal.ObjectClass.REAL_NAME);
            //int itemRealNameSize = mem.Read<int>(itemRealNamePtr + 0x4);
            //string itemRealName = mem.ReadString(itemRealNamePtr + 0x8, itemRealNameSize, Encoding.ASCII);


            //IsDead
            byte isDead = mem.Read<byte>(entityPtr + Offsets.Entity.ISDEAD);
            entity.IsDead = (isDead == 1);

            switch (entity.Type)
            {
                case Entity.Types.Airplane:
                case Entity.Types.Car:
                case Entity.Types.Helicopter:
                case Entity.Types.Ship:
                    entity.DriverPointer = mem.Read<IntPtr>(entityPtr + Offsets.Entity.DRIVER_POINTER);
                    entity.Driver = GetDriver(mem, transformation, scoreboard, entity.DriverPointer);
                    return entity;
                case Entity.Types.Zombie:
                    return entity;

            }
                

            //Weapon In Hands 
            IntPtr weaponBase = mem.Read<IntPtr>(entityPtr + Offsets.Entity.WEAPON);
            weaponBase = mem.Read<IntPtr>(weaponBase + 0x4);
            entity.WeaponPointer = weaponBase;

            IntPtr weaponObjectClass = mem.Read<IntPtr>(weaponBase + Offsets.Weapon.ObjectClass.BASE); // 0.50 changed from 0x70 to 0x50
            weaponObjectClass = mem.Read<IntPtr>(weaponObjectClass + Offsets.Weapon.ObjectClass.REAL_NAME);
            entity.WeaponObjectClassPointer = weaponObjectClass;

            int weaponNameSize = mem.Read<int>(weaponObjectClass + 0x4);
            string weaponName = mem.ReadString(weaponObjectClass + 0x8, weaponNameSize, Encoding.ASCII);

            if (weaponNameSize == 0)
                weaponName = "None";

            entity.WeaponInHand = weaponName;

            //Stance
            byte entityStance = (byte)mem.Read<int>(entityPtr + Offsets.Entity.STANCE);//0 - standing, 1 - crouching, 2 - laying down
            string curStance = "Unknown";

            if (entityStance == 0)
                curStance = "Standing";
            if (entityStance == 1)
                curStance = "Crouching";
            if (entityStance == 2)
                curStance = "Laying";
            if (entityStance == 3)
                entity.IsDead = true;

            entity.Stance = curStance;

            //Playername
            entity.Name = GetEntityName(scoreboard, entity.ID);

            entity.IsTN = (entity.Name.StartsWith("[TN]") || 
                            entity.Name.Contains("SuRTaiNx3") || 
                            entity.Name.Contains("Ronok") || 
                            entity.Name.Contains("Daniel") || 
                            entity.Name.Contains("Dutchman") || 
                            entity.Name.Contains("Neo"));


            entity.Stats = GetEntityValues(mem, entityPtr);

            return entity;
        }

        public static Driver GetDriver(ProcessMemory mem, Transformation transformation, ScoreboardTable scoreboard, IntPtr entityPtr)
        {
            Driver entity = new Driver();

            entity.ID = mem.Read<int>(entityPtr + Offsets.Entity.ID);
            entity.Pointer = entityPtr;

            IntPtr visualState = mem.Read<IntPtr>(entityPtr + Offsets.Entity.VisualState.BASE);
            entity.VisualStatePointer = visualState;
            entity.Position = mem.ReadVector3(visualState + Offsets.Entity.VisualState.POSITION_VEC3_START);
            entity.HeadPosition = mem.ReadVector3(visualState + Offsets.Entity.VisualState.HEAD_POSITION_VEC3_START);

            entity.DirectionX = mem.Read<float>(visualState + Offsets.Entity.VisualState.DIRECTION_Y);
            entity.DirectionY = mem.Read<float>(visualState + Offsets.Entity.VisualState.DIRECTION_X);

            entity.ScreenFootPosition = Geometry.W2SN(entity.Position, transformation);

            //Entity Type
            IntPtr objectClass = mem.Read<IntPtr>(entityPtr + Offsets.Entity.ObjectClass.BASE); // 0.50 changed from 0x70 to 0x50

            // Object name
            IntPtr itemObjectNamePtr = mem.Read<IntPtr>(objectClass + Offsets.Entity.ObjectClass.NAME);
            int itemObjectNameSize = mem.Read<int>(itemObjectNamePtr + 0x4);
            entity.ObjectName = mem.ReadString(itemObjectNamePtr + 0x8, itemObjectNameSize, Encoding.ASCII);

            //// Real Name
            //IntPtr itemRealNamePtr = mem.Read<IntPtr>(objectClass + Offsets.Item_Animal.ObjectClass.REAL_NAME);
            //int itemRealNameSize = mem.Read<int>(itemRealNamePtr + 0x4);
            //string itemRealName = mem.ReadString(itemRealNamePtr + 0x8, itemRealNameSize, Encoding.ASCII);


            //IsDead
            byte isDead = mem.Read<byte>(entityPtr + Offsets.Entity.ISDEAD);
            entity.IsDead = (isDead == 1);

            //Playername
            entity.Name = GetEntityName(scoreboard, entity.ID);
            entity.IsTN = (entity.Name.StartsWith("[TN]") || entity.Name.Contains("Ronok") || entity.Name.Contains("Daniel") || entity.Name.Contains("Dutchman") || entity.Name.Contains("Neo"));

            
            entity.Stats = GetEntityValues(mem, entityPtr);

            return entity;
        }

        public static Entity GetLocalEntity(ProcessMemory mem, Transformation transData, ScoreboardTable scoreboard, IntPtr localPlayerPtr)
        {
            Entity localEntity = GetEntity(mem, transData, scoreboard, localPlayerPtr);
            return localEntity;
        }

        public static EntityValues GetEntityValues(ProcessMemory mem, IntPtr entityPointer)
        {
            EntityValues values = new EntityValues();
            values.Blood = -1;
            values.Health = -1;
            values.Shock = -1;
            values.HeatComfort = -1;
            values.Temperature = -1;

            IntPtr gameVariables = mem.Read<IntPtr>(entityPointer + Offsets.Entity.VARIABLES);
            int gameVariableSize = mem.Read<int>(entityPointer + (Offsets.Entity.VARIABLES + 0x4));

            if (gameVariableSize > 128)
                return values;

            for (int n = 0; n < gameVariableSize; n++)
            {
                IntPtr firstArray = mem.Read<IntPtr>(gameVariables + 0x0C * n);
                int maxArraySize = mem.Read<int>(gameVariables + 0x0C * n + 4);

                if (maxArraySize > 100)
                    continue;

                for (int k = 0; k < maxArraySize; k++)
                {
                    IntPtr namePtr = mem.Read<IntPtr>(firstArray + k * 0x14 + 0x4);
                    int nameSize = mem.Read<int>(namePtr + 0x4);
                    IntPtr arrayEntity = mem.Read<IntPtr>(firstArray + k * 0x14 + 0xC);
                    if (nameSize > 0 && nameSize < 50)
                    {
                        string name = mem.ReadString(namePtr + 0x8, nameSize, Encoding.ASCII);
                        float value = mem.Read<float>(arrayEntity + 0xC);
                        if (name == "blood" && value > 0)
                            values.Blood = value;
                        else if (name == "health" && value > 0)
                            values.Health = value;
                        else if (name == "shock")
                            values.Shock = value;
                        else if (name == "bodytemperature")
                            values.Temperature = value;
                        else if (name == "heatcomfort")
                            values.HeatComfort = value;
                    }
                }
            }
            return values;
        }

        public static string GetEntityName(ScoreboardTable scoreboard, int entityID)
        {
            string playerName;
            bool success = scoreboard.TryGetValue(entityID, out playerName);

            if (success)
                return playerName;
            else
                return "PlayerName";
        }

        public static EntitiesList GetEntities(ProcessMemory mem, IntPtr worldBase, Transformation transformation, ScoreboardTable scoreboard, Vector3 localEntityPosition)
        {
            EntitiesList entityList = new EntitiesList();

            IntPtr playerArrayPtr = mem.Read<IntPtr>(worldBase + Offsets.World.PLAYER_TABLE);
            IntPtr playerArray = mem.Read<IntPtr>(playerArrayPtr);

            int playerArraySize = mem.Read<int>(playerArrayPtr + 0x4);

            for (int i = 0; i < playerArraySize; i++)
            {
                // Player Pointer
                IntPtr playerBasePtr = mem.Read<IntPtr>(playerArray + (i * 0x2C));
                IntPtr playerPtr = mem.Read<IntPtr>(playerBasePtr + 0x4);

                Entity entity = GetEntity(mem, transformation, scoreboard, playerPtr);

                entity.BasePointer = playerBasePtr;

                if (entity.Position.X > 1 || entity.Position.Y > 1)
                {
                    // Current Distance
                    float dx = (localEntityPosition.X - entity.Position.X);
                    float dy = (localEntityPosition.Y - entity.Position.Y);
                    float dz = (localEntityPosition.Z - entity.Position.Z);
                    entity.Distance = (int)Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));

                    if (entity.Distance < 1)
                        entity.Distance = 1;

                    if (entity.Position.Z > 0 && entity.Distance < 1300)
                        entityList.Entities.Add(entity);
                }
            }

            return entityList;
        }


        public static ScoreboardTable GetScoreboard(ProcessMemory mem)
        {
            ScoreboardTable scoreboard = new ScoreboardTable();

            IntPtr scoreboardOffset = mem.Read<IntPtr>((IntPtr)Offsets.SCOREBOARD + 0x28);
            int scoreboardSize = mem.Read<int>(scoreboardOffset + 0x10);
            IntPtr scoreboardPointer = mem.Read<IntPtr>(scoreboardOffset + 0xC);

            IntPtr servername = mem.Read<IntPtr>(scoreboardOffset + 0x2E0);
            int servernamesize = mem.Read<int>(servername + 0x4);
            scoreboard.ServerName = mem.ReadString(servername + 0x8, servernamesize, Encoding.ASCII);

            for (int i = 0; i < scoreboardSize; i++)
            {
                // Get iterated player ID
                int playerID = mem.Read<int>(scoreboardPointer + (i * 0xE8) + 0x4);

                IntPtr playerNamePointer = mem.Read<IntPtr>(scoreboardPointer + 0x80 + (i * 0xE8));
                int playerNameSize = mem.Read<int>(playerNamePointer + 0x4);
                string playerName = mem.ReadString(playerNamePointer + 0x8, playerNameSize, Encoding.ASCII);

                if (!scoreboard.ContainsKey(playerID))
                    scoreboard.Add(playerID, playerName);
            }

            return scoreboard;
        }


        public static Transformation GetTransformation(ProcessMemory mem)
        {
            Transformation data = GetTransformationData(mem);

            if (!data.IsValid())
                return null;

            bool isFucked =
                data.Right == new Vector3(1, 0, 0) &&
                data.Up == new Vector3(0, 0, 1) &&
                data.Forward == new Vector3(0, 1, 0);

            bool isFucked2 = (data.Fov == new Vector3(0.30f, 0.19f, 0));

            if (!isFucked && !isFucked2)
            {

                buf_right = data.Right;
                buf_up = data.Up;
                buf_forward = data.Forward;
                buf_fov = data.Fov;
                buf_translation = data.Translation;
                buf_viewport = data.Viewport;
                buf_proj1 = data.Proj1;
                buf_proj2 = data.Proj2;
            }

            return new Transformation(buf_right, buf_up, buf_forward, buf_translation, buf_viewport, buf_fov, buf_proj1, buf_proj2);
        }

        private static Transformation GetTransformationData(ProcessMemory mem)
        {
            IntPtr transformationBase = mem.Read<IntPtr>((IntPtr)Offsets.Transformations.BASE);
            IntPtr transformationDataPtr = mem.Read<IntPtr>(transformationBase + Offsets.Transformations.DATA);

            Transformation transData = new Transformation();
            transData.Right = mem.ReadVector3(transformationDataPtr + Offsets.Transformations.RIGHT);
            transData.Up = mem.ReadVector3(transformationDataPtr + Offsets.Transformations.UP);
            transData.Forward = mem.ReadVector3(transformationDataPtr + Offsets.Transformations.FORWARD);
            transData.Translation = mem.ReadVector3(transformationDataPtr + Offsets.Transformations.TRANSLATION);
            transData.Viewport = mem.ReadVector3(transformationDataPtr + Offsets.Transformations.MATRIX);
            transData.Proj1 = mem.ReadVector3(transformationDataPtr + Offsets.Transformations.PROJD1);
            transData.Proj2 = mem.ReadVector3(transformationDataPtr + Offsets.Transformations.PROJD2);
            transData.Fov = mem.ReadVector3(transformationDataPtr + Offsets.Transformations.FOV);

            return transData;
        }


        private static List<Item> GetItems(ProcessMemory mem, Transformation transformation, IntPtr table, int tableSize)
        {
            List<Item> items = new List<Item>();

            for (int it = 0; it < tableSize; it++)
            {
                IntPtr itemPtr = mem.Read<IntPtr>(table + (it * 0x4));

                if (itemPtr != IntPtr.Zero)
                {
                    IntPtr visualState = mem.Read<IntPtr>(itemPtr + Offsets.Item_Animal.VisualState.BASE);
                    Vector3 pos = mem.ReadVector3(visualState + Offsets.Item_Animal.VisualState.POSITION_VEC3_START);
                    Vector3 screenPosition = Geometry.W2SN(pos, transformation);

                    if (pos.Z > 0.1f)
                    {
                        IntPtr objectClass = mem.Read<IntPtr>(itemPtr + Offsets.Item_Animal.ObjectClass.BASE);
                        IntPtr itemRealNamePtr = mem.Read<IntPtr>(objectClass + Offsets.Item_Animal.ObjectClass.REAL_NAME);
                        int itemRealNameSize = mem.Read<int>(itemRealNamePtr + 0x4);

                        IntPtr itemObjectNamePtr = mem.Read<IntPtr>(objectClass + Offsets.Item_Animal.ObjectClass.NAME);
                        int itemObjectNameSize = mem.Read<int>(itemObjectNamePtr + 0x4);

                        if (itemRealNamePtr != IntPtr.Zero && itemRealNameSize < 80 && itemRealNameSize > 0)
                        {
                            string itemRealName = mem.ReadString(itemRealNamePtr + 0x8, itemRealNameSize, Encoding.ASCII);
                            string itemObjectName = mem.ReadString(itemObjectNamePtr + 0x8, itemObjectNameSize, Encoding.ASCII);

                            if (itemObjectName == "Land_A_FuelStation_Feed")
                                continue;

                            IntPtr itemTypePtr = mem.Read<IntPtr>(objectClass + Offsets.Item_Animal.ObjectClass.TYPE); // 0.50 changed from 0x70
                            int itemTypeSize = mem.Read<int>(itemTypePtr + 0x4);
                            string itemType = mem.ReadString(itemTypePtr + 0x8, 64, Encoding.ASCII);

                            // Is Ruined
                            bool isRuined = false;
                            Int16 isRuinedVal = mem.Read<Int16>(itemPtr + Offsets.Item_Animal.ISRUINED);
                            if (isRuinedVal == 1)
                                isRuined = true;

                            //if (ItemFilter.Crashes.Contains(itemObjectName))
                                //_crashes.Add(new HeliCrash(pos, itemRealName));

                            items.Add(new Item(itemPtr, itemRealName, itemObjectName, visualState, pos, screenPosition, itemType, false, isRuined));
                        }
                    }
                }
            }

            return items;
        }

        public static List<Item> GetFarDroppedItems(ProcessMemory mem, IntPtr worldBase, Transformation transformation)
        {
            IntPtr farTable = mem.Read<IntPtr>(worldBase + Offsets.World.FARDROPPED_ITEMS_TABLE);
            int farTableSize = mem.Read<int>(worldBase + (Offsets.World.FARDROPPED_ITEMS_TABLE + 0x4));

            return GetItems(mem, transformation, farTable, farTableSize);
        }

        public static List<Item> GetNearDroppedItems(ProcessMemory mem, IntPtr worldBase, Transformation transformation)
        {
            IntPtr nearDroppedTable = mem.Read<IntPtr>(worldBase + Offsets.World.DROPPED_ITEMS_TABLE);
            int nearDroppedTableSize = mem.Read<int>(worldBase + (Offsets.World.DROPPED_ITEMS_TABLE + 0x4));

            return GetItems(mem, transformation, nearDroppedTable, nearDroppedTableSize);
        }

        public static List<Item> GetNearItems(ProcessMemory mem, IntPtr worldBase, Transformation transformation)
        {
            IntPtr nearTable = mem.Read<IntPtr>(worldBase + Offsets.World.NEAR_ITEMS_TABLE);
            int nearTableSize = mem.Read<int>(worldBase + (Offsets.World.NEAR_ITEMS_TABLE + 0x4));

            return GetItems(mem, transformation, nearTable, nearTableSize);
        }


        public static Vector3 GetIngameCursorPosition(ProcessMemory mem, IntPtr worldBase)
        {
            IntPtr inGameUIBase = mem.Read<IntPtr>(worldBase + Offsets.World.INGAME_UI_BASE);
            return mem.ReadVector3(inGameUIBase + Offsets.InGameUI.CURSOR_POSITION);
        }
    }
}
