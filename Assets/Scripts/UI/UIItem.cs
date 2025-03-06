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
            Debug.LogError("AddItem() : itemSlots�� null �Դϴ�!");
            return;
        }

        foreach (var slot in itemSlots)
        {
            if (slot == null)
            {
                Debug.LogError("AddItem() : slot�� null �Դϴ�!");
                continue;
            }

            if (slot.itemData == null)
            {
                Debug.LogWarning($"{slot.gameObject.name}�� itemData�� null �Դϴ�.");
                continue;
            }

            if (slot.itemData == data)
            {
                slot.stackCount = Mathf.Min(slot.stackCount + amount, data.maxStackAmount);
                slot.UpdateUI();
                Debug.Log($"{data.displayName} ���� ����: {slot.stackCount}");
                return;
            }
        }
        Debug.LogWarning($"{data.displayName}�� �߰��� ������ ������ ã�� ���߽��ϴ�.");
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
