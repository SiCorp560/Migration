using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyController : MonoBehaviour
{
    // To provide static access to the player (only one per scene)
    public static PlayerFlyController player;

    // The rigidbody component which controls the player's physics
    public Rigidbody2D rb;
    // The sprite renderer component which controls the player's sprite
    public SpriteRenderer sprite;

    // The speed of movement for the player (set in editor)
    public float moveSpeed;

    // Used to flip the player's sprite with direction of motion
    private bool left = true;

    // Used to mark the stunned state after hit by water droplet
    private bool stunned = false;


    private void Awake()
    {
        player = this;
    }

    private void Start()
    {
        if (rb == null)
        {
            Debug.LogError("Player doesn't have rigidbody component.");
            rb = GetComponent<Rigidbody2D>();
        }
        if (sprite == null)
        {
            Debug.LogError("Player doesn't have sprite renderer component.");
            sprite = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        // Move the player character
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        // TODO: Decide the best physics for controlling the player
        // Right now GetAxisRaw and directly set velocity, not at all drifty

        // When stunned, you can only move left and right
        if (stunned)
        {
            // Check for horizontal movement input
            float xMove = Input.GetAxisRaw("Horizontal");

            // Directly update the player's velocity
            rb.velocity = new Vector2(xMove * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Check for horizontal & vertical movement input
            float xMove = Input.GetAxisRaw("Horizontal");
            float yMove = Input.GetAxisRaw("Vertical");

            // Make sure moving diagonally doesn't give you extra speed
            Vector2 moveVector = new Vector2(xMove, yMove);

            // Directly update the player's velocity
            rb.velocity = moveVector.normalized * moveSpeed;
        }

        // Flip the sprite when player moves other way (assumes sprite faces left)
        if (left && rb.velocity.x < 0)
        {
            left = false;
            sprite.flipX = false;
        }
        else if (!left && rb.velocity.x > 0)
        {
            left = true;
            sprite.flipX = true;
        }
    }

    public void KnockDown()
    {
        // Temporarily turn on gravity maybe?
        // Apply a strong downward force and lock controls?
        StartCoroutine(StunTimer());
    }

    private IEnumerator StunTimer()
    {
        // We could introduce visuals/animation here?

        // Change to the stunned state (changes movement controls)
        stunned = true;

        // Play sound effect
        if (AudioManager.S != null)
            AudioManager.S.Play("Chime");

        // Turn the sprite blue (to indicate wet?)
        sprite.color = new Color(0.8f, 1.0f, 1.0f);

        // Turn on gravity for the rigidbody
        rb.gravityScale = 2.0f;

        // Player stays stunned for a little while
        yield return new WaitForSeconds(3.0f);

        // Goes back to normal
        sprite.color = Color.white;
        rb.gravityScale = 0.0f;
        stunned = false;
    }

    public bool isStunned()
    {
        return stunned;
    }
}
