using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUse : MonoBehaviour
{
    public ItemData healingPotion;
    public ItemData speedPotion;

    public void UseHealingPotion()
    {
        CharacterManager.Instance.Player.uiItem.UseItem(healingPotion);
    }

    public void UseSpeedPotion()
    {
        CharacterManager.Instance.Player.uiItem.UseItem(speedPotion);
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Keyboard.current.eKey.isPressed)
            {
                UseHealingPotion();
            }
            else if (Keyboard.current.rKey.isPressed)
            {
                UseSpeedPotion();
            }
        }
    }
}
