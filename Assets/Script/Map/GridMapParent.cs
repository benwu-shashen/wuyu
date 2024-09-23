using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

# if UNITY_EDITOR
public class GridMapParent : MonoBehaviour
{
    public static MapData_SO mapDate;
    private static GameObject[] gridProperties;

    [MenuItem("Map/CreatMap")]
    public static void GetTile()
    {
        mapDate = AssetDatabase.LoadAssetAtPath<MapData_SO>("Assets/GameData/Inventory/MapData_SO.asset");
        gridProperties = GameObject.FindGameObjectsWithTag("GridProperties");
        GridType[] gridType = (GridType[])Enum.GetValues(typeof(GridType));
        mapDate.tileSO.Clear();
        foreach (GameObject go in gridProperties)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(go.scene.buildIndex);
            // 从路径中提取场景名称
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            TileSO tileSO = new TileSO()
            {
                SceneName = sceneName,
                tileTypes = new List<TileType>(),
            };

            foreach (GridType type in gridType)
            {
                TileType tileType = new TileType()
                {
                    gridType = type,
                    tileGridDatas = new List<TileGridData>(),
                };
                tileSO.tileTypes.Add(tileType);
            }
            mapDate.tileSO.Add(tileSO);
            GetTileSceneName(mapDate.tileSO[mapDate.tileSO.Count - 1].tileTypes, go);
        }
        Debug.Log("加载完成");
    }

    private static void GetTileSceneName(List<TileType> tileTypes, GameObject go)
    {
        List<GridMap> gridMaps = go.GetComponentsInChildren<GridMap>().ToList();
        for (int i = 0; i < gridMaps.Count; i++)
        {
            gridMaps[i].GetTile(tileTypes[i].tileGridDatas);
        }
    }
}
#endif