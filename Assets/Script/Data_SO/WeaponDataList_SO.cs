using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
[CreateAssetMenu(fileName = "WeaponDataList_SO", menuName = "Inventory/WeaponDataList_SO")]
public class WeaponDataList_SO : ScriptableObject
{
    public List<WeaponDetails> weaponDetailsLists;
}
# endif