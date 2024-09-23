using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData_SO", menuName = "Inventory/MapData_SO")]
public class MapData_SO : ScriptableObject
{
    [Header("地图信息")]
    public int gridWidth;
    public int gridHeight;
    [Header("左下角原点")]
    public int originX;
    public int originY;
    public List<TileSO> tileSO;
}
