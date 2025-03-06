using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAcquire : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterManager.Instance.Player.uiItem.AddItem(itemData, 1);
            Destroy(gameObject);
        }
    }
}
