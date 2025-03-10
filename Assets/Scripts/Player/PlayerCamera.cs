using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform player; // 플레이어 오브젝트
    public float distance = 3.0f; // 플레이어와 카메라 거리
    public float height = 1.5f;   // 카메라 높이
    public float rotationSpeed = 0.1f; // 마우스 회전 감도 (작게 설정)

    private float currentX = 0f;
    private float currentY = 0f;
    private float minY = -20f, maxY = 60f; // Y축 회전 제한
    private Vector2 mouseDelta; // 마우스 입력값 저장

    // 새로운 입력 시스템에서 마우스 입력을 받는 메서드
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        // 마우스 입력값을 사용해 회전 조정
        currentX += mouseDelta.x * rotationSpeed;
        currentY -= mouseDelta.y * rotationSpeed;
        currentY = Mathf.Clamp(currentY, minY, maxY); // Y축 회전 제한

        // 카메라 위치 및 회전 설정
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 offset = new Vector3(0, height, -distance); // 플레이어 뒤쪽으로 이동

        // 카메라 위치 조정
        transform.position = player.position + rotation * offset;
        transform.LookAt(player.position + Vector3.up * height * 0.5f); // 플레이어 바라보기
    }
}
