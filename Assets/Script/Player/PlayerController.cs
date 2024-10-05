using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed;
    public float inputX;
    public float inputY;
    private bool isMoving;
    [SerializeField] private Animator[] animators;
    [SerializeField] private GameObject weapon;
    [SerializeField] private AnimatorWeapon animatorWeapon;
    [SerializeField] private RectTransform StaminaBar;
    [SerializeField] private Vector3 vector3;

    public bool inputDisable;
    public bool useWeaponAmin = false;

    private float mouseX;
    private float mouseY;

    private bool useTool;
    private bool useWeapon;
    private bool activeWeapon;
    private float floatSetWeapon = -1f;
    private float floatGetWeapon = 1f;

    public Vector2 movementInput;
    public float WallOrRunSpeed;

    private StaminaBar staminaBar;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
        WallOrRunSpeed = 1f;
        weapon.SetActive(false);
        weapon.transform.position = weapon.transform.position + new Vector3(1f, 0, 0);
        useWeapon = true;
        activeWeapon = true;
        vector3 = new Vector3(0, 1.5f, 0);
        staminaBar = StaminaBar.gameObject.GetComponent<StaminaBar>();
    }

    private void Update()
    {
        movementInput = InputButtonMannager.Instance.inputControls.GamePlayer.Move.ReadValue<Vector2>();
        if (!inputDisable)
        {
            if (weapon.activeSelf && floatGetWeapon != floatSetWeapon && !useWeaponAmin)
            { 
                floatGetWeapon = floatSetWeapon;
                WeaponForword(floatGetWeapon);
            }
            if (movementInput.x > 0 && floatSetWeapon < 0)
                floatSetWeapon = 1f;
            else if (movementInput.x < 0 && floatSetWeapon > 0)
                floatSetWeapon = -1f;
            PlayerInput();
        }
        else
            isMoving = false;

        foreach (var anim in animators)
        {
            // 检查动画组件名称是否是要排除的名称
            if (anim.gameObject.name == "Weapon")
            {
                continue; // 跳过此组件
            }
            anim.SetBool("isMoving", isMoving);
            anim.SetFloat("mouseX", mouseX);
            anim.SetFloat("mouseY", mouseY);
            if (isMoving)
            {
                anim.SetFloat("InputX", inputX);
                anim.SetFloat("InputY", inputY);
            }
        }
    }

    private void LateUpdate()
    {
    }

    private void FixedUpdate()
    {
        if (!inputDisable)
            Movement();
    }

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.MouseClickedEvent += OnMouseClickedEvent;
        EventHandler.WeaponUse += OnWeaponUse;
        EventHandler.WeaponAttact += OnWeaponAttact;
        EventHandler.PlayerMove += OnPlayerMove;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.MouseClickedEvent -= OnMouseClickedEvent;
        EventHandler.WeaponUse -= OnWeaponUse;
        EventHandler.WeaponAttact -= OnWeaponAttact;
        EventHandler.PlayerMove -= OnPlayerMove;
    }

    private void OnPlayerMove()
    {
        inputDisable = false;
    }

    private void OnWeaponAttact(Attack_State attack_State)
    {
        animatorWeapon.floatWeapon = floatSetWeapon;
        useWeaponAmin = true;
    }

    private void OnWeaponUse()
    {
        weapon.SetActive(activeWeapon);
        activeWeapon = !activeWeapon;
        WeaponForword(floatSetWeapon);
    }

    private void OnBeforeSceneUnloadEvent()
    {
        inputDisable = true;
    }
    private void OnAfterSceneUnloadEvent()
    {
        inputDisable = false;
    }
    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void OnMouseClickedEvent(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        if (itemDetails.itemType != ItemType.Seed &&itemDetails.itemType != ItemType.Commodity && itemDetails.itemType != ItemType.Furniture)
        {
            mouseX = mouseWorldPos.x - transform.position.x;
            mouseY = mouseWorldPos.y - (transform.position.y + 0.85f);

            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                mouseY = 0;
            else
                mouseX = 0;

            StartCoroutine(UseToolRoutine(mouseWorldPos, itemDetails));
        }
        else
        {
            EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
        }
    }

    private IEnumerator UseToolRoutine(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        useTool = true;
        inputDisable = true;
        yield return null;
        foreach (var anim in animators)
        {
            anim.SetTrigger("useTool");
            anim.SetFloat("InputX", mouseX);
            anim.SetFloat("InputY", mouseY);
        }
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
        yield return new WaitForSeconds(0.25f);

        useTool = false;
        inputDisable = false;
    }

    private void PlayerInput()
    {
        inputX = movementInput.x * WallOrRunSpeed;
        inputY = movementInput.y * WallOrRunSpeed;

        movementInput = new Vector2(inputX, inputY);
        isMoving = movementInput != Vector2.zero;

        StaminaBar.position = this.gameObject.transform.position + vector3;
    }

    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * speed * Time.deltaTime);
    }

    private void WeaponForword(float pos)
    {
        // 获取对象的当前缩放
        Vector3 scale = transform.localScale;
        // 翻转 X 轴的缩放值
        // 应用新的缩放
        transform.localScale = scale;
        if (pos > 0 && !useWeapon)
        {
            scale.x *= 1;
            weapon.transform.localScale = scale;
            weapon.transform.position = weapon.transform.position + new Vector3(2f, 0, 0);
            useWeapon = !useWeapon;
        }
        else if  (pos < 0 && useWeapon)
        {
            scale.x *= -1;
            weapon.transform.localScale = scale;
            weapon.transform.position = weapon.transform.position + new Vector3(-2f, 0, 0);
            useWeapon = !useWeapon;
        }
    }
}
