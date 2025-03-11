using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUse : MonoBehaviour
{
    public UIItemSlot[] slots;
    public ItemData healingPotion;
    public ItemData speedPotion;

    private float heal;
    private float speed;
    private bool isUsePotion;

    public void UseHealingPotion()
    {
        isUsePotion = true;

        if (healingPotion != null && healingPotion.Available.Length > 0)
        {
            heal = healingPotion.Available[0].value;
        }

        // 체력 포션 사용 플레이어의 체력을 회복 시켜줌
        slots[0].UsePotion(healingPotion);
        CharacterManager.Instance.Player.uiItem.UseItem(healingPotion);
        CharacterManager.Instance.Player.condition.Heal(heal);

        isUsePotion = false;
    }

    public void UseSpeedPotion()
    {
        isUsePotion = true;

        if (speedPotion != null && speedPotion.Available.Length > 0)
        {
            speed = speedPotion.Available[0].value;
        }

        // 속도 포션 사용 코루틴을 통해 10초간 속도 증가됨
        slots[1].UsePotion(speedPotion);
        CharacterManager.Instance.Player.uiItem.UseItem(speedPotion);
        StartCoroutine(SpeedUpTime(speed));

        isUsePotion = false;
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        // 해당 키가 입력되고 조건문을 통과하면 포션 사용
        if (context.performed)
        {
            if (Keyboard.current.eKey.isPressed && !isUsePotion)
            {
                if (slots[0].stackCount > 0 && !slots[0].isCooldownActive)
                {
                    UseHealingPotion();
                }
            }
            else if (Keyboard.current.rKey.isPressed && !isUsePotion)
            {
                if (slots[1].stackCount > 0 && !slots[0].isCooldownActive)
                {
                    UseSpeedPotion();
                }
            }
        }
    }

    IEnumerator SpeedUpTime(float amout)
    {
        // 속도 증가 10초 지속 후 다시 원상태로 복구
        CharacterManager.Instance.Player.controller.moveSpeed += amout;
        CharacterManager.Instance.Player.controller.runSpeed += amout;

        yield return new WaitForSeconds(10f);

        CharacterManager.Instance.Player.controller.moveSpeed -= amout;
        CharacterManager.Instance.Player.controller.runSpeed -= amout;
    }
}
