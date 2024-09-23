using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
[CreateAssetMenu(fileName = "InventoryBag_SO", menuName = "Inventory/InventoryBag_SO")]
public class InventoryBag_SO : ScriptableObject
{
    public List<InventoryItem> itemList;
}
# endif