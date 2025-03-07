using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUse : MonoBehaviour
{
    public ItemData healingPotion;
    public ItemData speedPotion;
    private float heal;
    private float speed;

    public void UseHealingPotion()
    {
        if(healingPotion != null && healingPotion.Available.Length > 0)
        {
            heal = healingPotion.Available[0].value;
        }

        CharacterManager.Instance.Player.uiItem.UseItem(healingPotion);
        CharacterManager.Instance.Player.condition.Heal(heal);
    }

    public void UseSpeedPotion()
    {
        if (speedPotion != null && speedPotion.Available.Length > 0)
        {
            speed = speedPotion.Available[0].value;
        }

        CharacterManager.Instance.Player.controller.isUsePotion = true;

        CharacterManager.Instance.Player.uiItem.UseItem(speedPotion);
        StartCoroutine(SpeedUpTime(speed));

        CharacterManager.Instance.Player.controller.isUsePotion = false;
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Keyboard.current.eKey.isPressed)
            {
                UseHealingPotion();
            }
            else if (Keyboard.current.rKey.isPressed && !CharacterManager.Instance.Player.controller.isUsePotion)
            {
                UseSpeedPotion();
            }
        }
    }

    IEnumerator SpeedUpTime(float amout)
    {
        CharacterManager.Instance.Player.controller.moveSpeed += amout;
        CharacterManager.Instance.Player.controller.runSpeed += amout;

        yield return new WaitForSeconds(10f);

        CharacterManager.Instance.Player.controller.moveSpeed -= amout;
        CharacterManager.Instance.Player.controller.runSpeed -= amout;
    }
}
