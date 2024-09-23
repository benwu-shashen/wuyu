using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList_SO", menuName = "Inventory/ItemDataList")]
public class ItemDetaList_SO : ScriptableObject
{
    public List<ItemDetails> itemDetailsLists;
}
