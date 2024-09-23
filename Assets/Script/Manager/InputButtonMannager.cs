using Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

class InputButtonMannager : Singleton<InputButtonMannager>
{
    public PlayerInputControls inputControls;
    public bool walk = false;
    public PlayerController controller;
    bool isPressed;

    private bool isUseWeapon = false;

    public void Init()
    {

    }

    private void Awake()
    {
        isPressed = true;
        inputControls = new PlayerInputControls();
        inputControls.GamePlayer.Button.started += ctx =>
        {
            switch (ctx.control.name)
            {
                case "b":
                    OnBag();
                    break;
                case "leftAlt":
                    OnWalk();
                    break;
                case "j":
                    OnWeapon();
                    break;
                default:
                    break; // 如果有其他按键，可以在这里处理
            }
        };
    }

    private void OnWeaponAttack(InputAction.CallbackContext context)
    {
        double pressDuration = context.duration;

        if (context.control.name == "k")
        {
            if (pressDuration >= 0.7f)
            {
                //重击
                EventHandler.CallWeaponAttact(Attack_State.Thump);
            }
            else if (pressDuration >= 0.3f)
            {
                //轻击
                EventHandler.CallWeaponAttact(Attack_State.Flick);
            }
        }
    }

    private void OnWeaponContinueDefense()
    {
        if (isPressed)
        {
            EventHandler.CallWeaponDefense(true);
            isPressed = false;
        }
    }

    private void OnWeaponloosenDefense(InputAction.CallbackContext context)
    {
        EventHandler.CallWeaponDefense(false);
        isPressed = true;
    }

    private void OnWeapon()
    {
        EventHandler.CallWeaponUse();
        if (!isUseWeapon)
        {
            inputControls.GamePlayer.Weapon.performed += OnWeaponAttack;
            inputControls.GamePlayer.Weapon.canceled += OnWeaponloosenDefense;
            inputControls.GamePlayer.Weapon.started += ctx =>
            {
                switch (ctx.control.name)
                {
                    case "l":
                        OnWeaponContinueDefense();
                        break;
                    default:
                        break; // 如果有其他按键，可以在这里处理
                }
            };
        }
        else
        {
            inputControls.GamePlayer.Weapon.performed -= OnWeaponAttack;
            inputControls.GamePlayer.Weapon.canceled -= OnWeaponloosenDefense;
            inputControls.GamePlayer.Weapon.started -= ctx => 
            {
                switch (ctx.control.name)
                {
                    case "l":
                        OnWeaponContinueDefense();
                        break;
                    default:
                        break; // 如果有其他按键，可以在这里处理
                }
            };

        }
    }

    private void OnBag()
    {
        EventHandler.CallBagButtonOpen();
    }

    private void OnWalk()
    {
        if (walk)
        {
            walk = false;
            controller.WallOrRunSpeed = Settings.runspeed;
        }
        else
        {
            walk = true;
            controller.WallOrRunSpeed = Settings.wallspeed;
        }
    }

    private void OnEnable()
    {
        inputControls.Enable();
    }

    private void OnDisable()
    {
        inputControls.Disable();
    }
}
