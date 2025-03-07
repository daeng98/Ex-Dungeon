using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public ItemData itemData;
    public TextMeshProUGUI stackText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI durationText;
    public Image itemImage;

    public int stackCount;
    public float cooldownTime;
    public float durationTime;
    public bool isCooldownActive = false;

    public Color cooldownColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    public void UsePotion(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("itemData가 null입니다");
            return;
        }

        if (isCooldownActive || stackCount <= 0)
        {
            Debug.LogError($"사용 불가 stackCount: {stackCount}, isCooldownActive: {isCooldownActive}");
            return;
        }

        foreach (var available in itemData.Available)
        {

            if (available.type == AvailableType.Health)
            {
                cooldownTime = 10f;
                StartCoroutine(CooldownTimer());
            }
            else if (available.type == AvailableType.Speed)
            {
                cooldownTime = 20f;
                durationTime = 10f;
                StartCoroutine(CooldownTimer());
                StartCoroutine(DurationTimer());
            }
        }

        stackCount--;
        UpdateUI();
    }

    IEnumerator CooldownTimer()
    {
        isCooldownActive = true;
        float maxCooldown = cooldownTime;

        while (cooldownTime > 0)
        {
            cooldownText.text = cooldownTime.ToString();

            float t = 1 - (cooldownTime / maxCooldown);
            itemImage.color = Color.Lerp(cooldownColor, Color.white, t);

            yield return new WaitForSeconds(1f);
            cooldownTime--;
        }

        cooldownText.text = "";
        itemImage.color = Color.white;
        isCooldownActive = false;
    }

    IEnumerator DurationTimer()
    {
        while (durationTime > 0)
        {
            durationText.text = durationTime.ToString();
            yield return new WaitForSeconds(1f);
            durationTime--;
        }

        durationText.text = "";
    }

    public void UpdateUI()
    {
        if (stackText != null)
        {
            stackText.text = stackCount.ToString();
        }
    }
}
