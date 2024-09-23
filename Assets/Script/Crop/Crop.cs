using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    public TileDetails tileDetails;
    private int harvestActionCount;
    private Animator anim;
    private Transform PlayerTransform => FindObjectOfType<PlayerController>().transform;
    public bool CanHarvest => tileDetails.growthDays >= cropDetails.TotalGrowthDays;

    public void ProcessToolAction(ItemDetails tool, TileDetails tile)
    {
        tileDetails = tile;

        int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
        if (requireActionCount == -1) return;

        anim = GetComponentInChildren<Animator>();

        if (harvestActionCount < requireActionCount)
        {
            harvestActionCount++;

            if (anim != null && cropDetails.hasAnimation)
            {
                if (PlayerTransform.position.x < transform.position.x)
                    anim.SetTrigger("RotateRight");
                else
                    anim.SetTrigger("RotateLeft");
            }

            if (cropDetails.hasParticalEffect)
                EventHandler.CallParticleEffectEvent(cropDetails.effectType, transform.position + cropDetails.effectPos);
        }

        if (harvestActionCount >= requireActionCount)
        {
            if (cropDetails.generateAtPlayerPosition || !cropDetails.hasAnimation)
            {
                SpawnHarvestItems();
            }
            else if (cropDetails.hasAnimation)
            {
                if (PlayerTransform.position.x < transform.position.x)
                    anim.SetTrigger("FallingRight");
                else
                    anim.SetTrigger("FallingLeft");
                StartCoroutine(HarvestAfterAnimation());
            }
        }
    }

    private IEnumerator HarvestAfterAnimation()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("END"))
        {
            yield return null;
        }

        SpawnHarvestItems();

        if (cropDetails.transferItemID > 0)
        {
            CreateTransferCrop();
        }
    }

    private void CreateTransferCrop()
    {
        tileDetails.seedItemID = cropDetails.transferItemID;
        tileDetails.daysSinceLastHarvest = -1;
        tileDetails.growthDays = 0;

        EventHandler.CallRefreshCurrentMap();
    }

    public void SpawnHarvestItems()
    {
        for (int i = 0; i < cropDetails.producedItemID.Length; i++)
        {
            int amountToProduce;

            if (cropDetails.producedMinAmount[i] == cropDetails.producedMaxAmount[i])
            {
                amountToProduce = cropDetails.producedMinAmount[i];
            }
            else
            {
                amountToProduce = Random.Range(cropDetails.producedMinAmount[i], cropDetails.producedMaxAmount[i] + 1);
            }

            for (int j = 0; j < amountToProduce; j++)
            {
                if (cropDetails.generateAtPlayerPosition)
                    EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID[i]);
                else
                {

                }

            }
        }
        if (tileDetails != null)
        {
            tileDetails.daysSinceLastHarvest++;

            if (cropDetails.daysToRegrow > 0 && tileDetails.daysSinceLastHarvest < cropDetails.regrowTimes)
            {
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                EventHandler.CallRefreshCurrentMap();
            }
            else
            {
                tileDetails.daysSinceLastHarvest = -1;
                tileDetails.seedItemID = -1;
                //自己设计
                //tileDetails.daysSinceDug = -1;
            }

            Destroy(gameObject);
        }
    }
}