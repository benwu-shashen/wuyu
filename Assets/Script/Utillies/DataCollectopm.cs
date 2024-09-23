using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    public Sprite itemOnWorldSprite;
    public string itemDescription;
    public int itemUseRadius;
    public int itemWeight;
    public bool canPickedup;
    public bool canDroppend;
    public bool canCarried;
    public int itemPrice;
    [Range(0f, 1f)]
    public float sellPercentage;
}

[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}

[System.Serializable]
public class AnimatorType
{
    public PartName partName;
    public AnimatorOverrideController overrideController;
}

[System.Serializable]
public class AnimatorTypeList
{
    public PartType partType;
    public List<AnimatorType> partAction;
}

[System.Serializable]
public class SerializableVector3
{
    [SceneName]
    public string sceneName;
    public List<SceneItem> itemList;
}

[System.Serializable]
public class SceneItem
{
    public int itemID;
    public int itemCount;
    public float x, y, z;

    public SceneItem(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

[System.Serializable]
public class TileSO
{
    public string SceneName;
    public List<TileType> tileTypes;
}

[System.Serializable]
public class TileType
{
    public GridType gridType;
    public List<TileGridData> tileGridDatas;
}

[System.Serializable]
public class TileGridData
{
    public Vector2Int tileCoordinate;
    public bool boolTypeValue;
}

[System.Serializable]
public class TileDetails
{
    public int gridX, gridY;
    public bool canDig;
    public bool canDropItem;
    public bool canPlaceFurniture;
    public bool isNPCObstacle;
    public int daysSinceDug = -1;
    public int daysSinceWatered = -1;
    public int seedItemID = -1;
    public int growthDays = -1;
    public int daysSinceLastHarvest = -1;
}

[System.Serializable]
public class WeaponDetails
{
    public int ID;
    public string Name;
    public Weapon_Class Class;
    public string Harm;
    public Sprite Icon;
    public Sprite OnWorldSprite;
}