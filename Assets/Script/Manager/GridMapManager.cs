using Inventory;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GridMapManager : Singleton<GridMapManager>
{
    [Header("地图信息")]
    public MapData_SO SO;
    private Grid currentGrid;

    [Header("种地瓦片切换信息")]
    public RuleTile digTile;
    public RuleTile waterTile;
    private Tilemap digTilemap;
    private Tilemap waterTilemap;

    private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();

    private Season currentSeason;

    private void Start()
    {
        foreach (var tileSOs in SO.tileSO)
        {
            foreach (var tileTypess in tileSOs.tileTypes)
            {
                InitTileDetailsDict(tileTypess, tileSOs.SceneName, tileTypess.gridType);
            }
        }
    }

    private void OnEnable()
    {
        EventHandler.ExecuteActionAfterAnimation += OnExecuteActionAfterAnimation;
        EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
        EventHandler.GameDayEvent += OnGameDayEvent;
        EventHandler.RefreshCurrentMap += RefreshMap;
    }

    private void OnDisable()
    {
        EventHandler.ExecuteActionAfterAnimation -= OnExecuteActionAfterAnimation;
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
        EventHandler.GameDayEvent -= OnGameDayEvent;
        EventHandler.RefreshCurrentMap -= RefreshMap;
    }

    private void OnExecuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
        var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);

        if (currentTile != null)
        {
            Crop currentCrop = GetCropObject(mouseWorldPos);
            switch (itemDetails.itemType)
            {
                case ItemType.Seed:
                    EventHandler.CallPlantSeedEvent(itemDetails.itemID, currentTile);
                    EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos, itemDetails.itemType);
                    break;
                case ItemType.Commodity:
                    EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos, itemDetails.itemType);
                    break;
                case ItemType.HoeTool:
                    SetDigGround(currentTile);
                    currentTile.daysSinceDug = 0;
                    currentTile.canDig = false;
                    currentTile.canDropItem = false;
                    break;
                case ItemType.WaterTool:
                    SetWaterGround(currentTile);
                    currentTile.daysSinceWatered = 0;
                    break;
                case ItemType.ChopTool:
                    currentCrop?.ProcessToolAction(itemDetails, currentCrop.tileDetails);
                    break;
                case ItemType.CollectTool:
                    currentCrop.ProcessToolAction(itemDetails, currentTile);
                    break;
            }
            UpdateTileDetails(currentTile);
        }
    }

    public Crop GetCropObject(Vector3 mouseWorldPos)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(mouseWorldPos);

        Crop currentCrop = null;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Crop>())
                currentCrop = colliders[i].GetComponent<Crop>();
        }
        return currentCrop;
    }

    private void OnAfterSceneUnloadEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
        digTilemap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
        waterTilemap = GameObject.FindWithTag("Water").GetComponent<Tilemap>();

        EventHandler.CallGenerateCropEvent();
        RefreshMap();
    }

    private void OnGameDayEvent(int day, Season season)
    {
        currentSeason = season;

        foreach (var tile in tileDetailsDict)
        {
            if (tile.Value.daysSinceWatered > -1) 
                tile.Value.daysSinceWatered = -1;
            if (tile.Value.daysSinceDug > -1)
                tile.Value.daysSinceDug++;
            if (tile.Value.daysSinceDug > 5 && tile.Value.seedItemID == -1)
            {
                tile.Value.daysSinceDug = -1;
                tile.Value.canDig = true;
                tile.Value.growthDays = -1;
            }
            if (tile.Value.seedItemID != -1)
            {
                tile.Value.growthDays++;
            }
        }

        RefreshMap();
    }

    private void InitTileDetailsDict(TileType tileType, string sceneName, GridType gridType)
    {
        foreach (var tileGridData in tileType.tileGridDatas)
        {
            TileDetails tileDetails = new TileDetails
            {
                gridX = tileGridData.tileCoordinate.x,
                gridY = tileGridData.tileCoordinate.y,
            };

            string key = tileDetails.gridX  + "x" + tileDetails.gridY + "y" + sceneName;

            if (GetTileDetails(key) != null )
            {
                tileDetails = GetTileDetails(key);
            }

            switch (gridType)
            {
                case GridType.Diggable:
                    tileDetails.canDig = tileGridData.boolTypeValue;
                    break;
                case GridType.DropItem:
                    tileDetails.canDropItem = tileGridData.boolTypeValue;
                    break;
                case GridType.PlaceFurniture:
                    tileDetails.canPlaceFurniture = tileGridData.boolTypeValue;
                    break;
                case GridType.NPCObstacle:
                    tileDetails.isNPCObstacle = tileGridData.boolTypeValue;
                    break;
            }

            if (GetTileDetails(key) != null)
                tileDetailsDict[key] = tileDetails;
            else
                tileDetailsDict.Add(key, tileDetails);
        }
    }

    private TileDetails GetTileDetails(string key)
    {
        if (tileDetailsDict.ContainsKey(key))
        {
            return tileDetailsDict[key];
        }
        return null;
    }

    public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
    {
        string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;
        return GetTileDetails(key);
    }

    private void SetDigGround(TileDetails tile)
    {
        Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
        if (digTilemap != null)
            digTilemap.SetTile(pos, digTile);
    }

    private void SetWaterGround(TileDetails tile)
    {
        Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
        if (waterTilemap != null)
            waterTilemap.SetTile(pos, waterTile);
    }

    public void UpdateTileDetails(TileDetails tileDetails)
    {
        string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + SceneManager.GetActiveScene().name;
        if (tileDetailsDict.ContainsKey(key))
        {
            tileDetailsDict[key] = tileDetails;
        }
        else
        {
            tileDetailsDict.Add(key, tileDetails);
        }
    }

    private void RefreshMap()
    {
        if (digTilemap != null)
            digTilemap.ClearAllTiles();
        if (waterTilemap != null)
            waterTilemap.ClearAllTiles();

        foreach (var crop in FindObjectsOfType<Crop>())
        {
            Destroy(crop.gameObject);
        }

        DisplayMap(SceneManager.GetActiveScene().name);
    }

    private void DisplayMap(string sceneName)
    {
        foreach (var tile in tileDetailsDict)
        {
            var key = tile.Key;
            var tileDetails = tile.Value;

            if (key.Contains(sceneName))
            {
                if (tileDetails.daysSinceDug > -1)
                    SetDigGround(tileDetails);
                if (tileDetails.daysSinceWatered > -1)
                    SetWaterGround(tileDetails);
                if (tileDetails.seedItemID > -1)
                    EventHandler.CallPlantSeedEvent(tileDetails.seedItemID, tileDetails);
            }
        }
    }

    /// <summary>
    /// 根据场景名字构建网格范围，输出范围和原点
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    /// <param name="gridDimensions">网格范围</param>
    /// <param name="gridOrigin">网格原点</param>
    /// <returns>是否有当前场景的信息</returns>
    //public bool GetGridDimensions(string sceneName, out Vector2Int gridDimensions, out Vector2Int gridOrigin)
    //{
    //    gridDimensions = Vector2Int.zero;
    //    gridOrigin = Vector2Int.zero;

    //    foreach (var mapData in mapDataList)
    //    {
    //        if (mapData.sceneName == sceneName)
    //        {
    //            gridDimensions.x = mapData.gridWidth;
    //            gridDimensions.y = mapData.gridHeight;

    //            gridOrigin.x = mapData.gridOriginX;
    //            gridOrigin.y = mapData.gridOriginY;

    //            return true;
    //        }
    //    }
    //    return false;
    //}
}
