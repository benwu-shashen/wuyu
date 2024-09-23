using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")]
        public ItemDetaList_SO itemDetaList_SO;

        [Header("背包数据")]
        public InventoryBag_SO playerBag;

        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        private void OnEnable()
        {
            EventHandler.DropItemEvent += OnDropItemEvent;
            EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
        }

        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
        }

        private void OnHarvestAtPlayerPosition(int ID)
        {
            var index = GetItemIndexInBag(ID);
            AddItemAtIndex(ID, index, 1);

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        private void OnDropItemEvent(int ID, Vector3 pos, ItemType itemType)
        {
            RemoveItem(ID, 1);
        }

        public ItemDetails GetItemDetails(int ID)
        {
            return itemDetaList_SO.itemDetailsLists.Find(i => i.itemID == ID);
        }

        public void Init()
        {

        }

        public void AddItem(Item item, bool toDestroy)
        {
            // 是否已经有该物品
            var index = GetItemIndexInBag(item.itemID);

            // 背包是否有空位
            AddItemAtIndex(item.itemID, index, 1);

            if (toDestroy)
                Destroy(item.gameObject);

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        private bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == 0)
                    return true;
            }
            return false;
        }

        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == ID)
                    return i;
            }
            return -1;
        }

        private void AddItemAtIndex(int ID, int index, int amount)
        {
            if (index == -1 && CheckBagCapacity())
            {
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item;
                        break;
                    }
                }
            }
            else
            {
                int currentAmount = playerBag.itemList[index].itemAmount + amount;
                var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };
                playerBag.itemList[index] = item;
            }
        }

        public void SwapItem(int fromIndex, int targetIndex)
        {
            InventoryItem curentItem = playerBag.itemList[fromIndex];
            InventoryItem targetItem = playerBag.itemList[targetIndex];

            if (targetItem.itemID != 0)
            {
                playerBag.itemList[fromIndex] = targetItem;
                playerBag.itemList[targetIndex] = curentItem;
            }
            else
            {
                playerBag.itemList[targetIndex] = curentItem;
                playerBag.itemList[fromIndex] = new InventoryItem();
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        private void RemoveItem(int ID, int removeAmount)
        {
            var index = GetItemIndexInBag(ID);

            if (playerBag.itemList[index].itemAmount > removeAmount)
            {
                var amount = playerBag.itemList[index].itemAmount - removeAmount;
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                playerBag.itemList[index] = item;
            }
            else if (playerBag.itemList[index].itemAmount == removeAmount)
            {
                var item = new InventoryItem();
                playerBag.itemList[index] = item;
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
    }
}
