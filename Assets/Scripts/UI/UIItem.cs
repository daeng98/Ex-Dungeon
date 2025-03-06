using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItem : MonoBehaviour
{
    public UIItemSlot[] itemSlots;

    public void AddItem(ItemData data, int amount)
    {
        if (itemSlots == null)
        {
            Debug.LogError("AddItem() : itemSlots가 null 입니다!");
            return;
        }

        foreach (var slot in itemSlots)
        {
            if (slot == null)
            {
                Debug.LogError("AddItem() : slot이 null 입니다!");
                continue;
            }

            if (slot.itemData == null)
            {
                Debug.LogWarning($"{slot.gameObject.name}의 itemData가 null 입니다.");
                continue;
            }

            if (slot.itemData == data)
            {
                slot.stackCount = Mathf.Min(slot.stackCount + amount, data.maxStackAmount);
                slot.UpdateUI();
                Debug.Log($"{data.displayName} 스택 증가: {slot.stackCount}");
                return;
            }
        }
        Debug.LogWarning($"{data.displayName}을 추가할 적절한 슬롯을 찾지 못했습니다.");
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
