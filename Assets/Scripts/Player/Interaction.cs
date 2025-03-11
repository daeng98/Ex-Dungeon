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
        // Interactable 태그 되면 정보텍스트 출력
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
        // 가날때는 꺼줌
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
        // 키 입력 시 해당 오브젝트 획득
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}