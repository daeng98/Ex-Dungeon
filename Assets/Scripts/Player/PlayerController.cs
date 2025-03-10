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
    public float runStamina;
    public float jumpPower;
    public float needStamina;
    public float fallMultiplier;
    public float gravityMultiplier;    
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraTransform;
    public float cameraDistance;
    public float cameraHeight;
    public float cameraSensitivity;

    private float camYaw;
    private float camPitch;

    private Vector2 lookInput;
    private Vector2 moveInput;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private Coroutine staminaCoroutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
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
        CameraRotate();
    }

    private void Move()
    {
        if (IsGrounded())
        {
            _animator.SetBool("isJump", false);
        }

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;

        // 입력값을 바탕으로 이동 방향 계산
        Vector3 moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        // 플레이어가 이동할 때 그 방향을 바라보도록 회전
        if (moveDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * 10f);
        }

        // 속도 적용 (걷기 / 뛰기)
        float speed = isRun ? runSpeed : moveSpeed;
        Vector3 velocity = moveDirection * speed;
        velocity.y = _rigidbody.velocity.y;

        _rigidbody.velocity = velocity;
    }

    void CameraRotate()
    {
        camYaw += lookInput.x * cameraSensitivity;
        camPitch -= lookInput.y * cameraSensitivity;
        camPitch = Mathf.Clamp(camPitch, -30f, 60f);
        camPitch = Mathf.Max(camPitch, -8.6f);

        Quaternion rotation = Quaternion.Euler(camPitch, camYaw, 0);
        Vector3 offset = new Vector3(0, cameraHeight, -cameraDistance);

        cameraTransform.position = transform.position + rotation * offset;
        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
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
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            if (CharacterManager.Instance.Player.condition.curStamina() >= needStamina)
            {
                _animator.SetBool("isJump", true);
                _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

                CharacterManager.Instance.Player.condition.UseStamina(needStamina);
            }
        }
    }

    bool IsGrounded()
    {
        float checkRadius = 0.8f;
        Vector3 checkPosition = transform.position + Vector3.down * 0.5f;

        bool groundCheck = Physics.CheckSphere(checkPosition, checkRadius, groundLayerMask);
        return groundCheck && _rigidbody.velocity.y <= 0f;
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
