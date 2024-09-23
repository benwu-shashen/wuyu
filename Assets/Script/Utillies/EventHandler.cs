using Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class EventHandler
{
    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;
    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> List)
    {
        UpdateInventoryUI?.Invoke(location, List);
    }

    public static event Action<int, int, Vector3> InstantiateItemInScene;
    public static void CallInstantiateItemInScene(int ID, int count, Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(ID, count, pos);
    }

    public static event Action<int, Vector3, ItemType> DropItemEvent;
    public static void CallDropItemEvent(int ID, Vector3 pos, ItemType itemType)
    {
        DropItemEvent?.Invoke(ID, pos, itemType);
    }

    public static event Action<ItemDetails, bool> ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }

    public static event Action<int, int> GameMinuteEvent;
    public static void CallGameMinuteEvent(int minute, int hour)
    {
        GameMinuteEvent?.Invoke(minute, hour);
    }

    public static event Action<int, Season> GameDayEvent;
    public static void CallGameDayEvent(int day, Season season)
    {
        GameDayEvent?.Invoke(day, season);
    }

    public static event Action<int, int, int, int, Season> GameDateEvent;
    public static void CallGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        GameDateEvent?.Invoke(hour, day, month, year, season);
    }

    public static event Action GamePause;
    public static void CallGamePause()
    {
        GamePause?.Invoke();
    }

    public static event Action<string, Vector3> TransitionEvent;
    public static void CallTransitionEvent(string sceneName, Vector3 pos)
    {
        TransitionEvent?.Invoke(sceneName, pos);
    }

    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    public static event Action AfterSceneUnloadEvent;
    public static void CallAfterSceneUnloadEvent()
    {
        AfterSceneUnloadEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPosition;
    public static void CallMoveToPosition(Vector3 targetPosition)
    {
        MoveToPosition?.Invoke(targetPosition);
    }

    public static event Action SceneNameDraw;
    public static void CallSceneNameDraw()
    {
        SceneNameDraw?.Invoke();
    }

    public static event Action<Vector3, ItemDetails> MouseClickedEvent;
    public static void CallMouseClickedEvent(Vector3 pos, ItemDetails itemDetails)
    {
        MouseClickedEvent?.Invoke(pos, itemDetails);
    }

    public static event Action<Vector3, ItemDetails> ExecuteActionAfterAnimation;
    public static void CallExecuteActionAfterAnimation(Vector3 pos, ItemDetails itemDetails)
    {
        ExecuteActionAfterAnimation?.Invoke(pos, itemDetails);
    }

    public static event Action<int, TileDetails> PlantSeedEvent;
    public static void CallPlantSeedEvent(int ID, TileDetails tile)
    {
        PlantSeedEvent?.Invoke(ID, tile);
    }

    public static event Action<int> HarvestAtPlayerPosition;
    public static void CallHarvestAtPlayerPosition(int ID)
    {
        HarvestAtPlayerPosition?.Invoke(ID);
    }

    public static event Action RefreshCurrentMap;
    public static void CallRefreshCurrentMap()
    {
        RefreshCurrentMap?.Invoke();
    }

    public static event Action<ParticaleEffectType, Vector3> ParticleEffectEvent;
    public static void CallParticleEffectEvent(ParticaleEffectType effectType, Vector3 pos)
    {
        ParticleEffectEvent?.Invoke(effectType, pos);
    }

    public static event Action GenerateCropEvent;
    public static void CallGenerateCropEvent()
    {
        GenerateCropEvent?.Invoke();
    }

    //打开背包
    public static event Action BagButtonOpen;
    public static void CallBagButtonOpen()
    {
        BagButtonOpen?.Invoke();
    }

    //切换武器
    public static event Action WeaponUse;
    public static void CallWeaponUse()
    {
        WeaponUse?.Invoke();
    }

    //武器攻击
    public static event Action<Attack_State> WeaponAttact;
    public static void CallWeaponAttact(Attack_State attack_State)
    {
        WeaponAttact?.Invoke(attack_State);
    }

    //武器防御
    public static event Action<bool> WeaponDefense;
    public static void CallWeaponDefense(bool Defense)
    {
        WeaponDefense?.Invoke(Defense);
    }

    //角色恢复移动
    public static event Action PlayerMove;
    public static void CallPlayerMove()
    {
        PlayerMove?.Invoke();
    }

    //武器攻击回退
    public static event Action AttackRollBack;
    public static void CallAttackRollBack()
    {
        AttackRollBack?.Invoke();
    }
}
