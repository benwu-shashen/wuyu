using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneItemList_SO", menuName = "Inventory/SceneItemList_SO")]
public class SceneItemList_SO : ScriptableObject
{
    public List<SerializableVector3> sceneItemLists;
}
