using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ItemType
{
    Seed, Commodity, Furniture,
    HoeTool, ChopTool, BreakTool, ReapTool, WaterTool, CollectTool,
    ReapableScenery,
}

public enum SlotType
{
    Bag, Box, Shop
}

public enum InventoryLocation
{
    Player, Box
}

public enum PartType
{
    None, Carry, Water, Hoe, Collect, Chop, Break
}

public enum PartName
{
    Body, Hair, Arm, Tool, HoldItem,
}

public enum Season
{
    春天, 夏天, 秋天, 冬天,
}

public enum GridType
{
    Diggable, DropItem, PlaceFurniture, NPCObstacle
}

public enum ParticaleEffectType
{
    None, LeavesFalling01, LeavesFalling02, Rock, ReapableScenery
}

public enum Attack_State
{
    Thump,
    Flick
}

public enum Weapon_Class
{
    COMBAT,//近战
    REMOTE,//远程
}