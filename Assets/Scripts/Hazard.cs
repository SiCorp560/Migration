using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If you hit the butterfly, stun it
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerFlyController flyPlayer = collision.gameObject.GetComponent<PlayerFlyController>();
            if (flyPlayer != null)
            {
                flyPlayer.KnockDown();
            }
        }

        // Always destroy the water when it collides with something
        Destroy(gameObject);
    }
}
