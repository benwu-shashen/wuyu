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
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Image slotHightLight;
        [SerializeField] private Button button;
        [Header("格子类型")]
        public SlotType slotType;
        public bool isSelected;
        public ItemDetails itemDetails;
        public int itemAmount;
        public int slotIndex;
        public GameObject Highlight;

        public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Start()
        {
            isSelected = false;
            if (itemDetails == null) 
                UpdateEmptySlot();
        }

        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            slotImage.enabled = true;
            amountText.text = amount.ToString();
            button.interactable = true;

        }

        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
                inventoryUI.UpdateSlotHightLight(-1);
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
            itemDetails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails == null) return;

            isSelected = !isSelected;
            if (inventoryUI.currentIndex != -1 && inventoryUI.currentIndex != slotIndex)
                inventoryUI.playerSlots[inventoryUI.currentIndex].isSelected = !inventoryUI.playerSlots[inventoryUI.currentIndex].isSelected;

            if (inventoryUI.currentIndex == -1 || inventoryUI.currentIndex != slotIndex)
            {
                inventoryUI.UpdateSlotHightLight(slotIndex);
            }
            else if (inventoryUI.currentIndex == slotIndex)
            {
                inventoryUI.currentIndex = -1;
                Highlight.SetActive(false);
            }

            if (slotType == SlotType.Bag)
            {
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;
                inventoryUI.dragItem.SetNativeSize();

                isSelected = true;
                inventoryUI.UpdateSlotHightLight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;
            
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                    return;

                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetSlot.slotIndex; ;

                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    inventoryUI.playerSlots[slotIndex].isSelected = false;
                    inventoryUI.playerSlots[targetIndex].isSelected = true;
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                    EventHandler.CallItemSelectedEvent(targetSlot.itemDetails, targetSlot.isSelected);
                }

                inventoryUI.UpdateSlotHightLight(targetIndex);
            }
            //else
            //{
            //    if (itemDetails.canPickedup)
            //    {
            //        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            //        EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
            //    }
            //}
        }
    }
}

