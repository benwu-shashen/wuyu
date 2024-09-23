using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorWeapon : MonoBehaviour
{
    private Animator animator;
    public float floatWeapon;
    [SerializeField]
    private PolygonCollider2D poly2D;
    [SerializeField]
    private GameObject Attack_Thump;
    [SerializeField]
    private PlayerController playerController;
    private bool isTransitioned = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        poly2D.enabled = false;
        Attack_Thump.SetActive(false);
    }

    private void Update()
    {
        // 检查倒播是否完成
        if (isTransitioned && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0)
        {
            // 倒播完成后，切换到下一个动画状态
            OnDisableThump();
            OnDisablePolygonCollider2D();
            animator.Play("Idle");
            animator.SetFloat("AttackSpeed", 1);
            isTransitioned = false;
        }
    }

    private void OnEnable()
    {
        EventHandler.WeaponAttact += OnWeaponAttact;
        EventHandler.AttackRollBack += OnAttackRollBack;
        EventHandler.WeaponDefense += OnWeaponDefense;
    }

    private void OnDisable()
    {
        EventHandler.WeaponAttact -= OnWeaponAttact;
        EventHandler.AttackRollBack -= OnAttackRollBack;
        EventHandler.WeaponDefense -= OnWeaponDefense;
    }

    private void OnAttackRollBack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("AttackRight1"))
        {
            animator.SetFloat("AttackSpeed", -1);
            EventHandler.CallPlayerMove();

            isTransitioned = true;
        }
    }

    private void OnWeaponAttact(Attack_State attack_State)
    {
        switch (attack_State)
        {
            case Attack_State.Thump:
                Attack_Thump.SetActive(true);
                break;
            case Attack_State.Flick:
                break;
        }
        if (floatWeapon > 0)
            animator.SetBool("AttackMirror", true);
        else
            animator.SetBool("AttackMirror", false);
        animator.SetTrigger("Attack");
    }

    private void OnWeaponDefense(bool attack_State)
    {
        if (attack_State)
        {
            if (floatWeapon > 0)
                animator.SetBool("AttackMirror", true);
            else
                animator.SetBool("AttackMirror", false);
            animator.SetBool("Defense", true);
        }
        else if (!attack_State)
        {
            if (floatWeapon > 0)
                animator.SetBool("AttackMirror", true);
            else
                animator.SetBool("AttackMirror", false);
            animator.SetBool("Defense", false);
        }
    }

    private void PlayerMove()
    {
        playerController.useWeaponAmin = false;
    }

    private void OnEnablePolygonCollider2D()
    {
        poly2D.enabled = true;
    }

    private void OnDisablePolygonCollider2D()
    {
        poly2D.enabled = false;
    }

    private void OnDisableThump()
    {
        Attack_Thump.SetActive(false);
    }
}
