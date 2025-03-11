using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [Header("Zoom")]
    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �� �ε� �� �ڵ����� Ŀ�� �����ְ� Ÿ�ӽ����� 1
        Time.timeScale = 1;
        Cursor.visible = false;
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
        if (Time.timeScale == 0) return;

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

        // �Է°��� �������� �̵� ���� ���
        Vector3 moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        // �÷��̾ �̵��� �� �� ������ �ٶ󺸵��� ȸ��
        if (moveDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * 10f);
        }

        // �ӵ� ���� (�ȱ� / �ٱ�)
        float speed = isRun ? runSpeed : moveSpeed;
        Vector3 velocity = moveDirection * speed;
        velocity.y = _rigidbody.velocity.y;

        _rigidbody.velocity = velocity;
    }

    void CameraRotate()
    {
        // ī�޶� �÷��̾ �ٶ󺸵��� ���� + �������� Ȱ���Ͽ� ���� �ܾƿ� ����
        camYaw += lookInput.x * cameraSensitivity;
        camPitch -= lookInput.y * cameraSensitivity;
        camPitch = Mathf.Clamp(camPitch, -8.6f, 60f);

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

            // Ȥ�� ���� �ڷ�ƾ�� ������ ������
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

    public void OnZoom(InputAction.CallbackContext context)
    {
        // ���콺 �� �� �޾ƿ� (���Ʒ� ��ũ�� ����� ���� y �� �޾ƿ�)
        float scrollInput = context.ReadValue<Vector2>().y;
        cameraDistance -= scrollInput * zoomSpeed;

        // �� �Ÿ� ����
        cameraDistance = Mathf.Clamp(cameraDistance, minZoom, maxZoom);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // ���� �ְ� ���¹̳��� ����ϸ� ����
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
        // CheckSphere�� ���� �� ���·� üũ
        float checkRadius = 1f;
        bool groundCheck = Physics.CheckSphere(transform.position, checkRadius, groundLayerMask);
        // ���� ���� ������ ���� �ӵ��� ���� ���ǹ� �߰�����
        return groundCheck && _rigidbody.velocity.y <= 0f;
    }

    private IEnumerator UseStaminaTime()
    {
        // Ű ������ �ִ� ���� ��� ���¹̳� �Һ��ϰ� ����
        while (isRun)
        {
            CharacterManager.Instance.Player.condition.UseStamina(runStamina);
            yield return new WaitForSeconds(0.1f);
        }
    }
}