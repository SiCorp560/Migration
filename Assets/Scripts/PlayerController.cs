using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // To provide static access to the player (only one per scene)
    public static PlayerController player;

    // The rigidbody component which controls the player's physics
    public Rigidbody2D rb;
    // The sprite renderer component which controls the player's sprite
    public SpriteRenderer sprite;
    // The animator component which animates the player's sprite
    public Animator animator;

    // The CharacterController2D script which moves the player
    public CharacterController2D controller;

    // The speed of movement for the player (set in editor)
    public float moveSpeed;

    // Used to keep track of player movement during gameplay
    private float xMove;
    private bool jump;


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
        if (animator == null)
        {
            Debug.LogError("Player doesn't have animator component.");
            animator = GetComponent<Animator>();
        }
    }

    public void GetBigger()
    {
        // Increase the scale of this game object
        transform.localScale = transform.localScale * 1.3f;
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

        // Check for horizontal movement input
        xMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (Input.GetButtonDown("Jump"))
            jump = true;

        // Update the player's animation
        animator.SetInteger("Speed", (int)Mathf.Abs(xMove));
    }

    private void FixedUpdate()
    {
        // Update the character's movement during fixed update
        controller.Move(xMove * Time.fixedDeltaTime, false, jump);
        
        // Reset the jump flag
        if (jump)
            jump = false;
    }
}

/*
// Used to flip the player's sprite with direction of motion
private bool left = true;

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
*/