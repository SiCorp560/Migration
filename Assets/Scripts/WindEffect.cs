using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    // The direction that the wind should be applied in
    public Vector3 direction = Vector3.left;
    // The force of the wind
    public int windForce = 3;
    // Max speed of player when being entirely pushed by wind
    public int maxWindSpeed = 5;

    // Whether player is currently within zone and should be affected
    private bool inWind = false;

    void FixedUpdate()
    {
        if (inWind)
        {
            // For convenience, shorter name
            PlayerFlyController player = PlayerFlyController.player;
            if (player != null)
            {
                // The acceleration force to apply to the player
                Vector2 windVector = direction * windForce;

                // Apply the force directly to the player's rigidbody
                player.rb.velocity += windVector * Time.fixedDeltaTime;

                // Cap the player's speed to prevent too much acceleration

                // TODO: Different versions for when player is flying with wind vs against wind?
            }
        }
    }
  
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inWind = true;
        }
    }
  
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inWind = false;
        }
    }
}
