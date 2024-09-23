using DG.Tweening;
using Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    [RequireComponent(typeof(SlotUI))]
    public class ActionBarButton : MonoBehaviour
    {
        public KeyCode key;
        private SlotUI slotUI;
        private InventoryUI inventoryUI;
        SlotUI[] slotUIs1;

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
            GameObject parentObject = transform.parent.parent.gameObject;
            inventoryUI = parentObject.GetComponent<InventoryUI>();
            slotUIs1 = inventoryUI.slotUIs1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(key))
            {
                if (slotUI.itemDetails == null) return;
                slotUI.isSelected = !slotUI.isSelected;
                foreach (SlotUI item in slotUIs1)
                {
                    if (this.slotUI != item)
                    {
                        item.isSelected = false;
                    }
                }
                if (slotUI.isSelected)
                    slotUI.inventoryUI.UpdateSlotHightLight(slotUI.slotIndex);
                else
                    slotUI.inventoryUI.UpdateSlotHightLight(-1);
                EventHandler.CallItemSelectedEvent(slotUI.itemDetails, slotUI.isSelected);
            }
        }
    }
}

