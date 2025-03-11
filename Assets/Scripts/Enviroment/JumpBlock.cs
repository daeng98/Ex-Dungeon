using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBlock : MonoBehaviour
{
    public float jumpForce = 300f;

    private void OnTriggerEnter(Collider other)
    {
        // 태그가 플레이어면 jumpForce 만큼 impulse
        if (other.CompareTag("Player"))
        {
            Rigidbody player = other.GetComponent<Rigidbody>();
            if (player != null)
            {
                player.velocity = new Vector3(player.velocity.x, 0f, player.velocity.z);
                player.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
