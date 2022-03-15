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

    // The CharacterController2D script which moves the player
    public CharacterController2D controller;

    // The speed of movement for the player (set in editor)
    public float moveSpeed;

    // Used to keep track of player movement during gameplay
    private float xMove;
    private float yMove;
    private bool jump;

    // Used to flip the player's sprite with direction of motion
    private bool left = true;

    // The player's flying state
    private bool flying = false;

    // Parameters used to time
    private bool startFlyTimer = false;
    private int defaultStamina = 3;
    public int maxStamina;

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

        maxStamina = defaultStamina;
    }

    private void Update()
    {
        // Change flying state as appropriate
        if (controller.isGrounded())
        {
            flying = false;
            startFlyTimer = true;
        }

        // Move the player character
        UpdateMovement();

        // If flying, update stamina
        if (flying && startFlyTimer)
        {
            StartCoroutine(FlyStamina());
        }
    }

    private void UpdateMovement()
    {
        // TODO: Decide the best physics for controlling the player
        // Right now GetAxisRaw and directly set velocity, not at all drifty

        // Check for horizontal & vertical movement input
        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");

        if (!controller.isGrounded() && yMove > 0 && startFlyTimer)
        {
            flying = true;
        }

        // When stunned, you can only move left and right
        if (stunned)
        {
            xMove /= 2;

            // Directly update the player's velocity
            rb.velocity = new Vector2(xMove * moveSpeed, rb.velocity.y);
        }
        else
        {
            if (flying)
            {
                // Make sure moving diagonally doesn't give you extra speed
                Vector2 moveVector = new Vector2(xMove, yMove);

                // Directly update the player's velocity
                rb.velocity = moveVector.normalized * moveSpeed;
            }
            else
            {
                // Directly update the player's velocity
                rb.velocity = new Vector2(xMove * moveSpeed, rb.velocity.y);

                if (Input.GetButtonDown("Jump"))
                    jump = true;
            }
        }

        // Flip the sprite when player moves other way (assumes sprite faces left)
        /*
        if (left && rb.velocity.x < 0)
        {
            left = false;
            sprite.flipX = false;
        }
        else if (!left && rb.velocity.x > 0)
        {
            left = true;
            sprite.flipX = true;
        */
    }

    private void FixedUpdate()
    {
        // Update the character's movement during fixed update
        controller.Move(xMove * Time.fixedDeltaTime, false, jump);
        
        // Reset the jump flag
        if (jump)
            jump = false;
    }

    private IEnumerator FlyStamina()
    {
        // Turn off check for timer to start
        startFlyTimer = false;

        // Turn off gravity for rigidbody
        //rb.gravityScale = 0.0f;

        int i = maxStamina;
        while (i > 0)
        {
            i -= 1;
            yield return new WaitForSeconds(1.0f);
        }

        // Turn on gravity for rigidbody
        //rb.gravityScale = 2.0f;

        flying = false;
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
        //rb.gravityScale = 2.0f;

        // Player stays stunned for a little while
        yield return new WaitForSeconds(3.0f);

        // Goes back to normal
        sprite.color = Color.white;
        //rb.gravityScale = 0.0f;
        stunned = false;
    }

    public bool isStunned()
    {
        return stunned;
    }
}
