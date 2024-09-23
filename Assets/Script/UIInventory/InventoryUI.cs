using DG.Tweening;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public ItemTooltip itemTooltip;
        [Header("玩家背包UI")]
        public UnityEngine.Object[] objectSlot;
        [SerializeField] public GameObject bagUI;
        [Header("拖拽图片")]
        public SlotUI[] playerSlots;
        public Image dragItem;
        private bool bagOpend;
        public SlotUI[] slotUIs1;
        SlotUI[] slotUIs2;
        public int currentIndex = -1;

        private void Awake()
        {
            GameObject gameObject1 = (GameObject)objectSlot[0];
            GameObject gameObject2 = (GameObject)objectSlot[1];
            slotUIs1 = gameObject1.GetComponentsInChildren<SlotUI>();
            slotUIs2 = gameObject2.GetComponentsInChildren<SlotUI>();

            playerSlots = new SlotUI[slotUIs1.Length + slotUIs2.Length];
            // 将数组 A 的元素复制到新数组中
            Array.Copy(slotUIs1, playerSlots, slotUIs1.Length);

            // 将数组 B 的元素复制到新数组中
            Array.Copy(slotUIs2, 0, playerSlots, slotUIs1.Length, slotUIs2.Length);
        }

        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.BagButtonOpen += OnBagButtonOpen;
        }

        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.BagButtonOpen -= OnBagButtonOpen;
        }

        private void OnBagButtonOpen()
        {
            OpenBagUI();
        }

        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch(location)
            {
                case InventoryLocation.Player:
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].itemAmount > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                            playerSlots[i].UpdateEmptySlot();
                    }
                    break;
                case InventoryLocation.Box:
                    break;
            }
        }

        private void OnBeforeSceneUnloadEvent()
        {
            UpdateSlotHightLight(-1);
        }

        private void Start()
        {
            for (int i = 0;i < playerSlots.Length;i++)
                playerSlots[i].slotIndex = i;
            bagOpend = bagUI.activeInHierarchy;
        }

        public void OpenBagUI()
        {
            bagOpend = !bagOpend;
            bagUI.SetActive(bagOpend);
        }

        public void UpdateSlotHightLight(int index)
        {
            if (currentIndex != -1)
            {
                playerSlots[currentIndex].Highlight.SetActive(false);
                currentIndex = index;
                if (index != -1)
                    playerSlots[index].Highlight.SetActive(true);
            }
            else
            {
                if (index != -1)
                {
                    currentIndex = index;
                    playerSlots[index].Highlight.SetActive(true);
                }
            }
        }
    }
}

