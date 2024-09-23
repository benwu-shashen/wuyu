using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    private Tilemap currentTilemap;

    public void GetTile(List<TileGridData> tileGridDatas)
    {
        currentTilemap = GetComponent<Tilemap>();
        currentTilemap.CompressBounds();

        Vector3Int startPos = currentTilemap.cellBounds.min;
        Vector3Int endPos = currentTilemap.cellBounds.max;

        for (int x = startPos.x; x < endPos.x; x++)
        {
            for (int y = startPos.x; y < endPos.y; y++)
            {
                TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));

                if (tile != null)
                {
                    TileGridData newTile = new TileGridData
                    {
                        tileCoordinate = new Vector2Int(x, y),
                        boolTypeValue = true,
                    };
                    tileGridDatas.Add(newTile);
                }
            }
        }
    }
//    private void OnEnable()
//    {
//        if (!Application.IsPlaying(this))
//        {

//        }
//    }

//    private void OnDisable()
//    {
//        if (!Application.IsPlaying(this))
//        {
//            currentTilemap = GetComponent<Tilemap>();
//            UpdateTileProperties();

//# if UNITY_EDITOR
//            if (mapDate != null)
//                EditorUtility.SetDirty(mapDate);
//# endif
//        }
//    }

//    private void UpdateTileProperties()
//    {
//        currentTilemap.CompressBounds();

//        if (!Application.IsPlaying(this))
//        {
//            if (mapDate != null)
//            {
//                Vector3Int startPos = currentTilemap.cellBounds.min;
//                Vector3Int endPos = currentTilemap.cellBounds.max;

//                for (int x = 0; x < endPos.x; x++)
//                {
//                    for (int y = 0; y < endPos.y; y++)
//                    {
//                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));

//                        if (tile != null)
//                        {
//                            TileProperty newTile = new TileProperty
//                            {
//                                tileCoordinate = new Vector2Int(x, y),
//                                gridType = this.gridType,
//                                boolTypeValue = true,
//                            };

//                            //mapDate.tileProperties.Add(newTile);
//                        }
//                    }
//                }
//            }
//        }
//    }
}