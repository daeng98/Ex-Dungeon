using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;

    private void OnTriggerEnter(Collider other)
    {
        // Interactable �±� �Ǹ� �����ؽ�Ʈ ���
        if (other.gameObject.CompareTag("Interactable"))
        {
            curInteractable = other.GetComponent<IInteractable>();

            if (curInteractable != null)
            {
                SetPromptText();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �������� ����
        if (curInteractable != null)
        {
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        // Ű �Է� �� �ش� ������Ʈ ȹ��
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}