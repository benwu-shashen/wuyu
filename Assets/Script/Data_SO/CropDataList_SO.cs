using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CropDataList_SO", menuName = "Inventory/CropDataList_SO")]
public class CropDataList_SO : ScriptableObject
{
    public List<CropDetails> cropDetailsList;
}
