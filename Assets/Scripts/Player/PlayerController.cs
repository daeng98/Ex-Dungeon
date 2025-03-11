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
        // 씬 로드 시 자동으로 커서 숨겨주고 타임스케일 1
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
        // 카메라가 플레이어를 바라보도록 설정 + 오프셋을 활용하여 줌인 줌아웃 가능
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

            // 혹시 몰라 코루틴이 있으면 멈춰줌
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
        // 마우스 휠 값 받아옴 (위아래 스크롤 사용을 위해 y 값 받아옴)
        float scrollInput = context.ReadValue<Vector2>().y;
        cameraDistance -= scrollInput * zoomSpeed;

        // 줌 거리 제한
        cameraDistance = Mathf.Clamp(cameraDistance, minZoom, maxZoom);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // 땅에 있고 스태미나가 충분하면 실행
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
        // CheckSphere를 통해 구 형태로 체크
        float checkRadius = 1f;
        bool groundCheck = Physics.CheckSphere(transform.position, checkRadius, groundLayerMask);
        // 더블 점프 방지를 위해 속도에 대한 조건문 추가했음
        return groundCheck && _rigidbody.velocity.y <= 0f;
    }

    private IEnumerator UseStaminaTime()
    {
        // 키 눌르고 있는 동안 계속 스태미나 소비하게 설정
        while (isRun)
        {
            CharacterManager.Instance.Player.condition.UseStamina(runStamina);
            yield return new WaitForSeconds(0.1f);
        }
    }
}