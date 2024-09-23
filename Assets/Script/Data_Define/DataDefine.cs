using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR

namespace DataDefine
{
    public class WeaponDefine
    {
        public int TID { get; set; }
        public string Name { get; set; }
        public Weapon_Class Class { get; set; }
        public string Harm { get; set; }
        public string Icon { get; set; }
        public string OnWorldSprite { get; set; }
    }

    public class ItemDefine
    {
        public int itemID { get; set; }
        public string itemName { get; set; }
        public ItemType itemType { get; set; }
        public string itemIcon { get; set; }
        public string itemOnWorldSprite { get; set; }
        public string itemDescription { get; set; }
        public int itemUseRadius { get; set; }
        public int itemWeight { get; set; }
        public bool canPickedup { get; set; }
        public bool canDroppend { get; set; }
        public bool canCarried { get; set; }
        public int itemPrice { get; set; }
        public float sellPercentage { get; set; }
    }
}
#endif