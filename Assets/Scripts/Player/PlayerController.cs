using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public bool isRun;
    public bool isUsePotion;
    public float runStamina;
    public float jumpPower;
    public float needStamina;
    public float fallMultiplier;
    public float gravityMultiplier;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    private Rigidbody _rigidbody;
    private Coroutine staminaCoroutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();

        if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity += Vector3.down * gravityMultiplier * Time.fixedDeltaTime;
        }

        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.down * fallMultiplier * Time.fixedDeltaTime;
        }
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraRotate();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        if (isRun)
        {
            dir *= runSpeed;
        }
        else
        {
            dir *= moveSpeed;
        }

        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraRotate()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isRun = true;

            if (staminaCoroutine != null)
            {
                StopCoroutine(staminaCoroutine);
            }

            staminaCoroutine = StartCoroutine(UseStaminaTime());
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRun = false;

            if (staminaCoroutine != null)
            {
                StopCoroutine(staminaCoroutine);
                staminaCoroutine = null;
            }
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            if (CharacterManager.Instance.Player.condition.curStamina() >= needStamina)
            {
                _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

                CharacterManager.Instance.Player.condition.UseStamina(needStamina);
            }
        }
    }

    bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, 0.5f, Vector3.down, out _, 1.6f, groundLayerMask);
    }

    private IEnumerator UseStaminaTime()
    {
        while (isRun)
        {
            CharacterManager.Instance.Player.condition.UseStamina(runStamina);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
