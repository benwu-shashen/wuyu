using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEditor.UIElements;
using Newtonsoft.Json;
using System.IO;

#if UNITY_EDITOR
public class WeaponEditor : EditorWindow
{
    private WeaponDataList_SO dataBase;
    private List<WeaponDetails> weaponList = new List<WeaponDetails>();
    private VisualTreeAsset weaponRowTemptate;
    private ListView weaponListView;
    private ScrollView weaponDetailsSection;
    private WeaponDetails activeItem;
    private VisualElement iconPreview;
    private Sprite defaultIcon;

    [MenuItem("M STUDIO/WeaponEditor")]
    public static void ShowExample()
    {
        WeaponEditor wnd = GetWindow<WeaponEditor>();
        wnd.titleContent = new GUIContent("WeaponEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/WeaponBuilder/WeaponEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // 拿到模板数据
        weaponRowTemptate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/WeaponBuilder/WeaponRowTemplate.uxml");

        // 获取ListView
        weaponListView = root.Q<VisualElement>("WeaponList").Q<ListView>("ListView");

        // 获取默认图片
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        // 变量赋值
        weaponDetailsSection = root.Q<ScrollView>("WeaponDetails");
        iconPreview = weaponDetailsSection.Q<VisualElement>("Icon");

        //获得按键
        root.Q<Button>("AddButton").clicked += OnAddItemClicked;
        root.Q<Button>("DeleteButton").clicked += OnDeleteClicked;
        root.Q<Button>("UpdateButton").clicked += OnUpdateClicked;

        LoadDataBase();

        GenerateListView();

        if (weaponList.Count != 0)
            weaponListView.selectedIndex = 0;
    }

    private void LoadDataBase()
    {
        var dataArray = AssetDatabase.FindAssets("WeaponDataList_SO");

        if (dataArray.Length >= 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            dataBase = AssetDatabase.LoadAssetAtPath(path, typeof(WeaponDataList_SO)) as WeaponDataList_SO;
        }

        weaponList = dataBase.weaponDetailsLists;
        //如果不标记则无法保存数据
        EditorUtility.SetDirty(dataBase);
    }

    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => weaponRowTemptate.CloneTree();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if (i < weaponList.Count)
            {
                if (weaponList[i].Icon != null)
                    e.Q<VisualElement>("Icon").style.backgroundImage = weaponList[i].Icon.texture;
                e.Q<Label>("WeaponName").text = weaponList[i].Name != null ? weaponList[i].Name : "NO ITEM";
            }
        };

        weaponListView.itemsSource = weaponList;
        weaponListView.makeItem = makeItem;
        weaponListView.bindItem = bindItem;

        weaponListView.selectionChanged += OnListSelectionChange;

        // 右侧信息面板不可见
        weaponDetailsSection.visible = false;
    }

    private void OnListSelectionChange(IEnumerable<object> selectedItem)
    {
        activeItem = (WeaponDetails)selectedItem.First();
        GetItemDetails();
        weaponDetailsSection.visible = true;
    }

    private void OnAddItemClicked()
    {
        WeaponDetails details = new WeaponDetails()
        {
            Name = "NO ITEM",
            ID = 1001 + weaponList.Count,
        };
        weaponList.Add(details);
        weaponListView.Rebuild();
        weaponListView.selectedIndex = weaponList.Count - 1;
        weaponDetailsSection.visible = true;
    }

    private void OnDeleteClicked()
    {
        weaponList.Remove(activeItem);
        if (weaponListView.selectedIndex - 1 != -1)
            weaponListView.selectedIndex = weaponListView.selectedIndex - 1;
        else
            weaponDetailsSection.visible = false;
        weaponListView.Rebuild();
    }

    private void OnUpdateClicked()
    {
        weaponListView.Rebuild();
        weaponDetailsSection.visible = false;
    }

    private void GetItemDetails()
    {
        weaponDetailsSection.MarkDirtyRepaint();

        weaponDetailsSection.Q<IntegerField>("WeaponID").value = activeItem.ID;
        weaponDetailsSection.Q<IntegerField>("WeaponID").RegisterValueChangedCallback(evt =>
        {
            activeItem.ID = evt.newValue;
        });

        weaponDetailsSection.Q<TextField>("WeaponName").value = activeItem.Name;
        weaponDetailsSection.Q<TextField>("WeaponName").RegisterValueChangedCallback(evt =>
        {
            activeItem.Name = evt.newValue;
            weaponListView.Rebuild();
        });

        iconPreview.style.backgroundImage = activeItem.Icon == null ? defaultIcon.texture : activeItem.Icon.texture;
        weaponDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.Icon;
        weaponDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.Icon = newIcon;

            iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
            weaponListView.Rebuild();
        });

        weaponDetailsSection.Q<ObjectField>("ItemOnWorldSprite").value = activeItem.OnWorldSprite;
        weaponDetailsSection.Q<ObjectField>("ItemOnWorldSprite").RegisterValueChangedCallback(evt =>
        {
            activeItem.OnWorldSprite = (Sprite)evt.newValue;
        });

        weaponDetailsSection.Q<EnumField>("WeaponClass").Init(activeItem.Class);
        weaponDetailsSection.Q<EnumField>("WeaponClass").value = activeItem.Class;
        weaponDetailsSection.Q<EnumField>("WeaponClass").RegisterValueChangedCallback(evt =>
        {
            activeItem.Class = (Weapon_Class)evt.newValue;
        });

        weaponDetailsSection.Q<TextField>("WeaponHarm").value = activeItem.Harm;
        weaponDetailsSection.Q<TextField>("WeaponHarm").RegisterValueChangedCallback(evt =>
        {
            activeItem.Harm = evt.newValue;
        });
    }
}
# endif