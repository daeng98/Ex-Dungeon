using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �±װ� �÷��̾�� showResult
        if (other.CompareTag("Player"))
        {
            CharacterManager.Instance.Player.ShowResult();
        }
    }
}
