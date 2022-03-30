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
    // The trigger collider which indicates the gust area
    public BoxCollider2D gustCollider;
    // The particle system which visualizes the gust (may update)
    public ParticleSystem gustParticles;

    // The speed of movement for the player (set in editor)
    public float moveSpeed;
    public float walkingSpeed; // Should be slower than normal movement
    public float glidingSpeed; // Limit for speed when falling

    // Used to flip the player's sprite with direction of motion
    private bool left = true;

    // The current state of movement, reflected in the sprite
    /* 0: OnGround and not moving, 1: walking, 2: flying, 3: flapping */
    private readonly int IDLE = 0, WALK = 1, FLY = 2, FLAP = 3, FALL = 4;

    // Used to track player's movement state during gameplay
    private bool flying = true;
    private bool onGround = false;

    // Used to track when player is affected by wind zone
    private bool inWind = false;
    private Vector2 windEffect = Vector2.zero;
    
    // Used to track the player's stamina throughout the level
    public int defaultStamina; // maxStamina before collecting any flowers
    public int slowdownTime;   // How much time flying after stamina runs out
    public int maxStamina;     // total stamina, upgraded with collectibles
    private float timeLeft;

    // Used to track the player's flapping to create gusts
    private bool flapping = false;

    // Used to track the coroutine which controls the player's flight stamina
    // private Coroutine flyingCoroutine = null;

    // Used to mark the stunned state after hit by water droplet
    private bool stunned = false;

    // All of the player's collected butterflies
    public List<GameObject> followers;

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
        timeLeft = maxStamina + slowdownTime;
    }

    private void StartFlying()
    {
        // Turn off gravity for rigidbody and push up
        // rb.gravityScale = 0.0f;

        // Set the state to flying
        onGround = false;
        flying = true;
        timeLeft = maxStamina + slowdownTime;
        // flyingCoroutine = StartCoroutine(FlyStamina());
    }

    private void StopFlying()
    {
        // Turn gravity on for the rigidbody
        // rb.gravityScale = 2.0f;

        // Disable the flying variables
        flying = false;
        // StopCoroutine(flyingCoroutine);

        // Cancel any flapping (can't gust while out of stamina)
        if (flapping)
            StopFlapping();

        // Signal the gliding animation (in air but not flying)
        animator.SetInteger("state", FALL);
    }

    private void StartFlapping()
    {
        // Set the state to flapping
        flapping = true;

        // Signal the flapping animation
        animator.SetInteger("state", FLAP);

        // Enable the gust (collider, visual effects)
        gustCollider.enabled = true;
        gustParticles.Play();
        //gustObject.SetActive(true);

        // Reset rigidbody velocities?
        rb.velocity = Vector2.zero;
    }

    private void StopFlapping()
    {
        // Set the state to not flapping
        flapping = false;

        // Signal the flapping animation to stop
        animator.SetInteger("state", FLY);

        // Disable the gust (collider, visual effects)
        gustCollider.enabled = false;
        gustParticles.Stop();
        //gustObject.SetActive(false);
    }

    /* Used by Wind Zone to tell butterfly when it's being pushed */
    public void StartWind(Vector2 windVector)
    {
        inWind = true;
        windEffect = windVector;
    }

    public void StopWind()
    {
        inWind = false;
        windEffect = Vector2.zero;
    }

    private void Update()
    {
        if (onGround && !stunned)
        {
            // Stop flying if the butterfly touches the ground
            if (flying)
            {
                StopFlying();
                animator.SetInteger("state", IDLE);
                animator.SetFloat("flapSpeed", 1.0f);
            }
            // Start flying if butterfly is on ground and "jumps"
            if (Input.GetAxisRaw("Vertical") == 1.0f)
            {
                StartFlying();
                animator.SetInteger("state", FLY);
            }
        }

        // Update the state based on whether player is flapping to create gust
        if (flying && Input.GetButtonDown("Flap"))
            StartFlapping();
        else if (flapping && Input.GetButtonUp("Flap"))
            StopFlapping();

        // Is the player doint something that requires stamina?
        bool usingStamina = false;

        // Player can't move (under own power) while flapping
        if (flapping)
        {
            usingStamina = true;

            // Player still gets pushed by the wind
            if (inWind)
            {
                rb.velocity = windEffect;
            }
        }
        else
        {
            // Check for horizontal & vertical movement input
            float xMove = Input.GetAxisRaw("Horizontal");
            float yMove = Input.GetAxisRaw("Vertical");

            // Determine the max downward velocity
            float fallingSpeed = glidingSpeed;

            // Halve the player's movement when stunned (wet), fall faster
            if (stunned)
            {
                xMove /= 2;
                yMove /= 2;
                fallingSpeed += 2.0f;
            }

            // Movement depends on the current state of the butterfly
            if (flying)
            {
                if (xMove != 0.0f || yMove == 1.0f)
                {
                    // Uses stamina when flying horizontally or up
                    usingStamina = true;
                    animator.SetInteger("state", FLY);
                }
                else if (yMove == -1.0f)
                {
                    // If only moving down, don't use stamina, just glide
                    animator.SetInteger("state", FALL);
                }

                // Make sure moving diagonally doesn't give you extra speed
                Vector2 moveVector = new Vector2(xMove, yMove);

                // Directly update the player's velocity
                rb.velocity = moveVector.normalized * moveSpeed;

                // Add any wind effects
                if (inWind)
                    rb.velocity += windEffect;

                // Make sure wind effect doesn't accelerate too much
                if (rb.velocity.y < 0 && Mathf.Abs(rb.velocity.y) > fallingSpeed)
                    rb.velocity = new Vector2(rb.velocity.x, -fallingSpeed);
            }
            else if (!onGround)
            {
                // Allow vertical movement, but only down, and apply gravity
                float yVelocity = Mathf.Min(rb.velocity.y, 0.0f);
                yVelocity += -9.8f * Time.deltaTime;
                float newY = -Mathf.Min(Mathf.Abs(yVelocity), fallingSpeed);

                // Directly update the player's velocity
                rb.velocity = new Vector2(xMove * walkingSpeed, newY);

                // Add wind effects
                if (inWind)
                    rb.velocity += windEffect;

                // Update animation
                animator.SetInteger("state", FALL);
            }
            else
            {
                // Manually apply "gravity" up to terminal velocity cap
                float yVelocity = rb.velocity.y;
                yVelocity += -9.8f * Time.deltaTime;
                float newY = -Mathf.Min(Mathf.Abs(yVelocity), fallingSpeed);

                // Directly update the player's velocity, horizontally
                rb.velocity = new Vector2(xMove * walkingSpeed, newY);

                // Add wind effects at half strength when grounded
                if (inWind)
                    rb.velocity += (windEffect / 2.0f);

                // Update animation based on movement
                if (xMove == 0.0f)
                    animator.SetInteger("state", IDLE);
                else
                    animator.SetInteger("state", WALK);
            }

            // Flip the sprite when player moves other way (assumes sprite faces left)
            if (left && rb.velocity.x < 0)
            {
                left = false;
                transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            }
            else if (!left && rb.velocity.x > 0)
            {
                left = true;
                transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            }
        }

        // Only count down stamina timer when using up stamina
        if (usingStamina)
            UpdateStaminaTimer();
    }

    // Triggered by the checkpoint, to prevent double activation
    public void IncreaseStamina()
    {
        maxStamina += 1;
    }

    private void UpdateStaminaTimer()
    {
        if (timeLeft > slowdownTime)
        {
            // Time with full speed flying
            timeLeft -= Time.deltaTime;
        }
        else if (timeLeft > 0.0f)
        {
            // Slowed down flying, time left until run out of stamina
            timeLeft -= Time.deltaTime;

            // Update the player's animation
            float animSpeed = Mathf.Lerp(0.1f, 0.5f, (float)timeLeft / (float)slowdownTime);
            animator.SetFloat("flapSpeed", animSpeed);
        }
        else
        {
            // When timer runs out, cancel flying ability
            StopFlying();
        }
    }

    /*
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
    */

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    public int GustPower()
    {
        return followers.Count;
    }
}
