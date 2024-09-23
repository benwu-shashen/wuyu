using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using DataDefine;

#if UNITY_EDITOR
public class DataManager
{
    private static WeaponDataList_SO weaponDataBase;
    private static ItemDetaList_SO itemDataBase;
    private static Sprite defaultIcon;

    [MenuItem("DataUpdate/WeaponData/Clear")]
    public static void WeaponClear()
    {
        var dataArray = AssetDatabase.FindAssets("WeaponDataList_SO");

        if (dataArray.Length >= 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            weaponDataBase = AssetDatabase.LoadAssetAtPath(path, typeof(WeaponDataList_SO)) as WeaponDataList_SO;
        }

        weaponDataBase.weaponDetailsLists.Clear();
        Debug.Log("清除成功");
    }

    [MenuItem("DataUpdate/ItemData/Clear")]
    public static void ItemClear()
    {
        var dataArray = AssetDatabase.FindAssets("ItemDetaList_SO");

        if (dataArray.Length >= 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            itemDataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDetaList_SO)) as ItemDetaList_SO;
        }

        itemDataBase.itemDetailsLists.Clear();
        Debug.Log("清除成功");
    }

    [MenuItem("DataUpdate/WeaponData/Read")]
    public static void WeaponRead()
    {
        var dataArray = AssetDatabase.FindAssets("WeaponDataList_SO");
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        if (dataArray.Length >= 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            weaponDataBase = AssetDatabase.LoadAssetAtPath(path, typeof(WeaponDataList_SO)) as WeaponDataList_SO;
        }

        string DataPath = "Assets/Data/";
        string json = File.ReadAllText(DataPath + "WeaponDefine.txt");
        Dictionary<int, WeaponDefine> Weapon = JsonConvert.DeserializeObject<Dictionary<int, WeaponDefine>>(json);

        foreach (var weapon in Weapon.Values)
        {
            WeaponDetails weaponDetails = new WeaponDetails()
            {
                ID = weapon.TID,
                Name = weapon.Name,
                Class = weapon.Class,
                Harm = weapon.Harm,
                Icon = weapon.Icon == null ? defaultIcon : AssetDatabase.LoadAssetAtPath<Sprite>(weapon.Icon),
                OnWorldSprite = weapon.OnWorldSprite == null ? defaultIcon : AssetDatabase.LoadAssetAtPath<Sprite>(weapon.OnWorldSprite),
            };
            weaponDataBase.weaponDetailsLists.Add(weaponDetails);
        }

        //如果不标记则无法保存数据
        EditorUtility.SetDirty(weaponDataBase);
        Debug.Log("读取成功");
    }

    [MenuItem("DataUpdate/ItemData/Read")]
    public static void ItemRead()
    {
        var dataArray = AssetDatabase.FindAssets("ItemDetaList_SO");
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        if (dataArray.Length >= 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            itemDataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDetaList_SO)) as ItemDetaList_SO;
        }

        string DataPath = "Assets/Data/";
        string json = File.ReadAllText(DataPath + "ItemDefine.txt");
        Dictionary<int, ItemDefine> Item = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);

        foreach (var item in Item.Values)
        {
            ItemDetails itemDetails = new ItemDetails()
            {
                itemID = item.itemID,
                itemName = item.itemName,
                itemType = item.itemType,
                itemIcon = item.itemIcon == null ? defaultIcon : AssetDatabase.LoadAssetAtPath<Sprite>(item.itemIcon),
                itemOnWorldSprite = item.itemOnWorldSprite == null ? defaultIcon : AssetDatabase.LoadAssetAtPath<Sprite>(item.itemOnWorldSprite),
                itemDescription = item.itemDescription,
                itemUseRadius = item.itemUseRadius,
                itemWeight = item.itemWeight,
                canPickedup = item.canPickedup,
                canCarried = item.canCarried,
                canDroppend = item.canDroppend,
                sellPercentage = item.sellPercentage,
            };
            itemDataBase.itemDetailsLists.Add(itemDetails);
        }

        //如果不标记则无法保存数据
        EditorUtility.SetDirty(itemDataBase);
        Debug.Log("读取成功");
    }
}
#endif