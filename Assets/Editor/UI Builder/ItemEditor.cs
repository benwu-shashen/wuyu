using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml.Serialization;


public class ItemEditor : EditorWindow
{
    private ItemDetaList_SO dataBase;
    private List<ItemDetails> itemList = new List<ItemDetails>();
    private VisualTreeAsset itemRowTemptate;
    private ListView itemListView;
    private ScrollView itemDetailsSection;
    private ItemDetails activeItem;
    private VisualElement iconPreview;
    private Sprite defaultIcon;

    [MenuItem("M STUDIO/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // 拿到模板数据
        itemRowTemptate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemRowTemplate.uxml");

        // 获取ListView
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");

        // 获取默认图片
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        // 变量赋值
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");
        iconPreview = itemDetailsSection.Q<VisualElement>("Icon");

        //获得按键
        root.Q<Button>("AddButton").clicked += OnAddItemClicked;
        root.Q<Button>("DeleteButton").clicked += OnDeleteClicked;
        root.Q<Button>("UpdateButton").clicked += OnUpdateClicked;

        LoadDataBase();

        GenerateListView();

        if (itemList.Count != 0)
            itemListView.selectedIndex = 0;
    }

    private void LoadDataBase()
    {
        var dataArray = AssetDatabase.FindAssets("ItemDetaList_SO");

        if (dataArray.Length >= 1 )
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            dataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDetaList_SO)) as ItemDetaList_SO;
        }

        itemList = dataBase.itemDetailsLists;
        //如果不标记则无法保存数据
        EditorUtility.SetDirty(dataBase);
    }

    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => itemRowTemptate.CloneTree();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if (i < itemList.Count)
            {
                if (itemList[i].itemIcon != null)
                    e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture;
                e.Q<Label>("Name").text = itemList[i].itemName != null ? itemList[i].itemName : "NO ITEM";
            }
        };

        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;

        itemListView.selectionChanged += OnListSelectionChange;

        // 右侧信息面板不可见
        itemDetailsSection.visible = false;
    }

    private void OnListSelectionChange(IEnumerable<object> selectedItem)
    {
        activeItem = (ItemDetails)selectedItem.First();
        GetItemDetails();
        itemDetailsSection.visible = true;
    }

    private void OnAddItemClicked()
    {
        ItemDetails details = new ItemDetails();
        details.itemName = "NO ITEM";
        details.itemID = 1001 + itemList.Count;
        itemList.Add(details);
        itemListView.Rebuild();
        itemListView.selectedIndex = itemList.Count - 1;
        itemDetailsSection.visible = true;
    }

    private void OnDeleteClicked()
    {
        itemList.Remove(activeItem);
        if (itemListView.selectedIndex - 1 != -1)
            itemListView.selectedIndex = itemListView.selectedIndex - 1;
        else
            itemDetailsSection.visible = false;
        itemListView.Rebuild();
    }

    private void OnUpdateClicked()
    {
        itemListView.Rebuild();
        itemDetailsSection.visible = false;
    }

    private void GetItemDetails()
    {
        itemDetailsSection.MarkDirtyRepaint();

        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemID = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("ItemWeight").value = activeItem.itemWeight;
        itemDetailsSection.Q<IntegerField>("ItemWeight").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemWeight = evt.newValue;
        });

        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild();
        });

        iconPreview.style.backgroundImage = activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture;
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;

            iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
            itemListView.Rebuild();
        });

        itemDetailsSection.Q<ObjectField>("ItemOnWorldSprite").value = activeItem.itemOnWorldSprite;
        itemDetailsSection.Q<ObjectField>("ItemOnWorldSprite").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemOnWorldSprite = (Sprite)evt.newValue;
        });

        itemDetailsSection.Q<EnumField>("ItemType").Init(activeItem.itemType);
        itemDetailsSection.Q<EnumField>("ItemType").value = activeItem.itemType;
        itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemType = (ItemType)evt.newValue;
        });

        itemDetailsSection.Q<TextField>("ItemDescription").value = activeItem.itemDescription;
        itemDetailsSection.Q<TextField>("ItemDescription").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("ItemUseRadius").value = activeItem.itemUseRadius;
        itemDetailsSection.Q<IntegerField>("ItemUseRadius").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemUseRadius = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanPickedup").value = activeItem.canPickedup;
        itemDetailsSection.Q<Toggle>("CanPickedup").RegisterValueChangedCallback(evt =>
        {
            activeItem.canPickedup = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanDropped").value = activeItem.canDroppend;
        itemDetailsSection.Q<Toggle>("CanDropped").RegisterValueChangedCallback(evt =>
        {
            activeItem.canDroppend = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanCarried").value = activeItem.canCarried;
        itemDetailsSection.Q<Toggle>("CanCarried").RegisterValueChangedCallback(evt =>
        {
            activeItem.canCarried = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("Price").value = activeItem.itemPrice;
        itemDetailsSection.Q<IntegerField>("Price").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemPrice = evt.newValue;
        });

        itemDetailsSection.Q<Slider>("SellPercentage").value = activeItem.sellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellPercentage = evt.newValue;
        });
    }
}