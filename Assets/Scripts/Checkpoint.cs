using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // The sprite renderer component which controls the player's sprite
    public SpriteRenderer sprite;

    public bool active = false;

    private void Start()
    {
        if (sprite == null)
        {
            Debug.LogError("Player doesn't have sprite renderer component.");
            sprite = GetComponent<SpriteRenderer>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !active)
        {
            active = true;
            sprite.color = Color.white;

            // tell GameManager that player reached a checkpoint
            GameManager.S.TriggerCheckpoint(transform);
        }
    }

    public bool IsActive()
    {
        return active;
    }
}
