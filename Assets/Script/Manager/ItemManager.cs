using Inventory;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : Singleton<ItemManager>
{
    public Item itemPrefab;
    private Transform itemParent;
    public SceneItemList_SO SO;
    public Item bounceItemPrefab;

    private Dictionary<string, List<SceneItem>> dicSceneItem;
    public List<SceneItem> currentSceneItemList;
    private string lastScene;

    private Transform PlayerTransform => FindObjectOfType<PlayerController>().transform;

    private void Awake()
    {
        dicSceneItem = new Dictionary<string, List<SceneItem>>();
        if (SO.sceneItemLists.Count != 0)
        {
            for (int i = 0; i < SO.sceneItemLists.Count; i++)
            {
                dicSceneItem[SO.sceneItemLists[i].sceneName] = SO.sceneItemLists[i].itemList;
            }
        }
    }

    public void Init()
    {

    }

    private void OnEnable()
    {
        EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
        EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
        EventHandler.DropItemEvent += OnDropItemEvent;
    }

    private void OnDisable()
    {
        EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
        EventHandler.DropItemEvent -= OnDropItemEvent;
    }

    private void OnInstantiateItemInScene(int ID, int count, Vector3 pos)
    {
        var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
        item.itemID = ID;
        item.itemCount = count;
    }

    private void OnAfterSceneUnloadEvent()
    {
        itemParent = GameObject.FindWithTag("ItemParent").transform;
        SetSceneItemList();
        GetAllSceneItems();
    }

    private void OnDropItemEvent(int ID, Vector3 mousePos, ItemType itemType)
    {
        if (itemType == ItemType.Seed) return;

        var item = Instantiate(bounceItemPrefab, PlayerTransform.position, Quaternion.identity, itemParent);
        item.itemID = ID;
        item.itemCount = 1;
        var dir = (mousePos - PlayerTransform.position).normalized;
        item.GetComponent<ItemBounce>().InitBounceItem(mousePos, dir);
        StartCoroutine(AddSceneItem(ID, 1, mousePos));
    }

    private void SetSceneItemList()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        
        if (dicSceneItem.TryGetValue(activeScene, out List<SceneItem> list))
        {
            for (int i = 0; i < SO.sceneItemLists.Count; i++)
            {
                if (SO.sceneItemLists[i].sceneName == lastScene)
                    SO.sceneItemLists[i].itemList = currentSceneItemList;
            }
            lastScene = SceneManager.GetActiveScene().name;
            currentSceneItemList = list;
        }
        else
        {
            List<SceneItem> sceneItems = new List<SceneItem>();
            dicSceneItem[activeScene] = sceneItems;
            SerializableVector3 serializableVector3 = new SerializableVector3
            {
                sceneName = activeScene,
            };
            SO.sceneItemLists.Add(serializableVector3);
        }
    }

    private void GetAllSceneItems()
    {
        for (int i = 0; i < currentSceneItemList.Count; i++)
            EventHandler.CallInstantiateItemInScene(currentSceneItemList[i].itemID, currentSceneItemList[i].itemCount, currentSceneItemList[i].ToVector3());
    }

    private IEnumerator AddSceneItem(int itemID, int itemCount, Vector3 pos)
    {
        string activeScene = SceneManager.GetActiveScene().name;
        for (int i = 0; i < SO.sceneItemLists.Count; i++)
        {
            if (SO.sceneItemLists[i].sceneName == activeScene)
            {
                SceneItem sceneItem = new SceneItem(pos)
                {
                    itemID = itemID,
                    itemCount = itemCount,
                };
                SO.sceneItemLists[i].itemList.Add(sceneItem);
            }
        }
        yield return null;
    }
}
