using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItem : MonoBehaviour
{
    public UIItemSlot[] itemSlots;

    public void AddItem(ItemData data, int amount)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.itemData == data)
            {
                slot.stackCount = Mathf.Min(slot.stackCount + amount, data.maxStackAmount);
                slot.UpdateUI();
                return;
            }
        }
    }

    public bool UseItem(ItemData data)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.itemData == data && slot.stackCount > 0)
            {
                slot.stackCount--;
                slot.UpdateUI();
                return true;
            }
        }
        return false;
    }
}
