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
                Debug.Log($"{data.displayName} ȹ�� : {slot.stackCount} ��");
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
                Debug.Log($"{data.displayName} ��� : {slot.stackCount} ��");
                return true;
            }
        }
        return false;
    }
}
