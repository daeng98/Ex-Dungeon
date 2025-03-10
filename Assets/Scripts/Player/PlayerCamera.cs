using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform player; // �÷��̾� ������Ʈ
    public float distance = 3.0f; // �÷��̾�� ī�޶� �Ÿ�
    public float height = 1.5f;   // ī�޶� ����
    public float rotationSpeed = 0.1f; // ���콺 ȸ�� ���� (�۰� ����)

    private float currentX = 0f;
    private float currentY = 0f;
    private float minY = -20f, maxY = 60f; // Y�� ȸ�� ����
    private Vector2 mouseDelta; // ���콺 �Է°� ����

    // ���ο� �Է� �ý��ۿ��� ���콺 �Է��� �޴� �޼���
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        // ���콺 �Է°��� ����� ȸ�� ����
        currentX += mouseDelta.x * rotationSpeed;
        currentY -= mouseDelta.y * rotationSpeed;
        currentY = Mathf.Clamp(currentY, minY, maxY); // Y�� ȸ�� ����

        // ī�޶� ��ġ �� ȸ�� ����
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 offset = new Vector3(0, height, -distance); // �÷��̾� �������� �̵�

        // ī�޶� ��ġ ����
        transform.position = player.position + rotation * offset;
        transform.LookAt(player.position + Vector3.up * height * 0.5f); // �÷��̾� �ٶ󺸱�
    }
}
