using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Manager : MonoBehaviour
    {
        private void Start()
        {
            InputButtonMannager.Instance.Init();
            ItemManager.Instance.Init();
            InventoryManager.Instance.Init();
            CropManager.Instance.Init();
        }
    }
}
