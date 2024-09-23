using System.Collections;
using UnityEngine;

public class CropGenerator : MonoBehaviour
{
    private Grid currentGrid;

    public int seedItemID;
    public int growthDays;
    private void Awake()
    {
        currentGrid = FindObjectOfType<Grid>();
        GenerateCrop();
    }

    private void OnEnable()
    {
        EventHandler.GenerateCropEvent += GenerateCrop;
    }

    private void OnDisable()
    {
        EventHandler.GenerateCropEvent -= GenerateCrop;
    }

    private void GenerateCrop()
    {
        Vector3Int cropGridPos = currentGrid.WorldToCell(transform.position);

        if (seedItemID != 0)
        {
            var tile = GridMapManager.Instance.GetTileDetailsOnMousePosition(cropGridPos);
            if (tile == null)
            {
                tile = new TileDetails();
            }

            tile.daysSinceWatered = -1;
            tile.seedItemID = seedItemID;
            tile.growthDays = growthDays;

            GridMapManager.Instance.UpdateTileDetails(tile);
        }
    }
}