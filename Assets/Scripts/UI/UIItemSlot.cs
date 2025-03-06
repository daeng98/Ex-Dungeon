using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemSlot : MonoBehaviour
{
    public ItemData itemData;
    public TextMeshProUGUI stackText;
    public int stackCount;

    public void UpdateUI()
    {
        if (stackText != null)
        {
            stackText.text = stackCount.ToString();
        }
    }
}
