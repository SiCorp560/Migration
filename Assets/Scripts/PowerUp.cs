using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // To prevent a double-trigger on collision
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collected)
        {
            // Set this power up as no longer collectible
            collected = true;

            // play the powerup collection sound
            // SoundManager.S.PlayPowerUpSound();

            // tell GameManager that player collected powerup
            GameManager.S.CollectLeaf();

            // destroy this object
            Destroy(gameObject);
        }
    }
}
