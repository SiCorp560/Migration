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
    // The animator component which animates the player's sprite
    public Animator animator;
    // The transform position which indicates the ground
    public Vector2 groundOffset;

    // The speed of movement for the player (set in editor)
    public float moveSpeed;

    // Used to flip the player's sprite with direction of motion
    private bool left = true;

    // Used to track player's movement state during gameplay
    private bool flying = false;
    private bool onGround = true;
    
    // Used to track the player's stamina throughout the level
    public int defaultStamina;
    public int slowdownTime;
    private int maxStamina;

    // Used to track the coroutine which controls the player's flight stamina
    private Coroutine flyingCoroutine = null;

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

    private void StartFlying()
    {
        // Turn off gravity for rigidbody and push up
        rb.gravityScale = 0.0f;

        // Set the state to flying
        onGround = false;
        flying = true;
        flyingCoroutine = StartCoroutine(FlyStamina());
        
        // TODO: Switch to flying animation?
    }

    private void StopFlying()
    {
        // Turn gravity on for the rigidbody
        rb.gravityScale = 2.0f;

        // Set the state to on the ground
        flying = false;
        StopCoroutine(flyingCoroutine);

        // TODO: Switch to walking animation?
        animator.SetFloat("flapSpeed", 1.0f);
    }

    private void Update()
    {
        // Update the player's current movement state
        if (onGround && !stunned)
        {
            // Stop flying if the butterfly touches the ground
            if (flying)
                StopFlying();
            // Start flying if butterfly is on ground and jumps
            if (Input.GetAxisRaw("Vertical") == 1.0f)
                StartFlying();
        }

        // Check for horizontal & vertical movement input
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");

        // Movement depends on the current state of the butterfly
        if (stunned)
        {
            // Halve the player's movement speed
            xMove /= 2;

            // Directly update the player's velocity
            rb.velocity = new Vector2(xMove * moveSpeed, rb.velocity.y);
        }
        else if (flying)
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

    // Triggered by the checkpoint, to prevent double activation
    public void IncreaseStamina()
    {
        maxStamina += 1;
    }

    private IEnumerator FlyStamina()
    {
        int timeLeft = maxStamina;
        while (timeLeft > 0)
        {
            // Wait for a second
            yield return new WaitForSeconds(1.0f);

            // Update the timer
            timeLeft -= 1;
        }

        // Transition from normal flying to slowed down flying
        float animSpeed = 0.5f;
        animator.SetFloat("flapSpeed", animSpeed);

        timeLeft = slowdownTime;
        while (timeLeft > 0)
        {
            // Wait for a second
            yield return new WaitForSeconds(1.0f);

            // Update the timer
            timeLeft -= 1;

            // Update the player's animation
            animSpeed = Mathf.Lerp(0.1f, 0.5f, (float)timeLeft / (float)slowdownTime);
            animator.SetFloat("flapSpeed", animSpeed);
        }

        // When timer runs out, cancel flying ability
        StopFlying();
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

        // If the player is flying, stop that
        if (flying)
            StopFlying();

        // Play sound effect
        if (AudioManager.S != null)
            AudioManager.S.Play("Chime");

        // Turn the sprite blue (to indicate wet?)
        sprite.color = new Color(0.8f, 1.0f, 1.0f);

        // Player stays stunned for a little while
        yield return new WaitForSeconds(3.0f);

        // Goes back to normal
        sprite.color = Color.white;
        stunned = false;
    }

    public bool IsStunned()
    {
        return stunned;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            onGround = true;
        }
    }
}
