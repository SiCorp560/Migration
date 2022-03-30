using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    // The direction that the wind should be applied in
    public Vector2 direction = Vector2.left;
    // The force of the wind
    public float windForce = 3;

    // Whether player is currently within zone and should be affected
    private bool inWind = false;

  
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !inWind)
        {
            // Activate this wind effect
            inWind = true;

            if (PlayerFlyController.player != null)
            {
                // The acceleration force to apply to the player
                Vector2 windVector = direction * windForce;

                // Tell the player controller to apply wind force
                PlayerFlyController.player.StartWind(windVector);
            }
        }
    }
  
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && inWind)
        {
            // De-activate this wind effect
            inWind = false;

            if (PlayerFlyController.player != null)
            {
                // Tell the player controller to stop applying wind force
                PlayerFlyController.player.StopWind();
            }
        }
    }
}
