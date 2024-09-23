using System.Collections;
using UnityEngine;

public class WeaponFader : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private AnimatorRecorderMode currentMode;
    private void Start()
    {
        // 检查当前的 Recorder 模式
        currentMode = animator.recorderMode;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemFader[] itemFaders = other.GetComponentsInChildren<ItemFader>();
        if (itemFaders.Length > 0)
        {
            EventHandler.CallAttackRollBack();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }
}