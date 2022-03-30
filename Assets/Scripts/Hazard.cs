using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    // The radius within which the player can hear water droplet sounds
    public float soundRadius = 20.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If you hit the butterfly, stun it
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerFlyController flyPlayer = collision.gameObject.GetComponent<PlayerFlyController>();
            if (flyPlayer != null)
                flyPlayer.KnockDown();
        }

        // Play the sound of water droplet colliding
        if (PlayerFlyController.player != null 
            && Vector3.Distance(PlayerFlyController.player.transform.position, transform.position) < soundRadius)
        {
            if (AudioManager.S != null)
                AudioManager.S.Play("Droplet");
        }

        // Always destroy the water when it collides with something
        Destroy(gameObject);
    }
}
