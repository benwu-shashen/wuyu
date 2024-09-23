using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory 
{
    public class ItemPickUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D coll)
        {
            Item item = coll.GetComponent<Item>();

            if (item != null)
            {
                if (item.itemDetails.canPickedup)
                {
                    InventoryManager.Instance.AddItem(item, true);
                    for (int i = 0; i < ItemManager.Instance.currentSceneItemList.Count; i++)
                    {
                        if (ItemManager.Instance.currentSceneItemList[i].itemID == item.itemID)
                        {
                            ItemManager.Instance.currentSceneItemList[i].itemCount -= item.itemCount;
                            if (ItemManager.Instance.currentSceneItemList[i].itemCount <= 0)
                            {
                                SceneItem targetItem = ItemManager.Instance.currentSceneItemList[i];
                                ItemManager.Instance.currentSceneItemList.Remove(targetItem);
                            }
                            return;
                        }
                    }
                }
            }
        }
    }

}

