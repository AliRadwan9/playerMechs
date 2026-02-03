// this is a simple 2D player movement script with dash, double jump, and sword slash mechanics
// attach this script to the player GameObject which has a Rigidbody2D and Animator component
// make sure to set up the Animator with appropriate parameters and animations
// also, create a sword hitbox GameObject as a child of the player and assign it to the swordHitbox variable
// same thing for the groundCheck GameObject to detect ground collisions
// adjust the movement, dash, jump, and slash settings in the inspector as needed
// ensure the ground GameObjects are tagged as "Ground" for proper ground detection
// this script also includes a simple health system where the player can take damage and die
// customize the TakeDamage and Die methods to fit your game's requirements


using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [Header("Dash Settings")]
    public float dashForce = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isGrounded;
    private bool isDashing;
    private bool doubleJumpUsed;
    private float lastDashTime;
    private float lastSlashTime;
    private bool isSlashing;
    public float slashCooldown = 0.5f;
    public float slashDuration = 0.3f;
    public GameObject swordHitbox;
    public int playerHealth = 3;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");

        // Move player if not dashing
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);


            // Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }

            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time >= lastDashTime + dashCooldown)
            {
                StartCoroutine(Dash());
            }
            // Double Jump
            if (!doubleJumpUsed && Input.GetButtonDown("Jump") && !isGrounded)
            {
                DoubleJump();
            }
            // Reset double jump when grounded
            if (isGrounded)
            {
                doubleJumpUsed = false;
            }

            // Flip sprite based on movement direction
            if (x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (x < 0)
                transform.localScale = new Vector3(-1, 1, 1);

            // Animation updates
            anim.SetBool("isRunning", x != 0 && isGrounded);
            anim.SetBool("isGrounded", isGrounded);
            anim.SetBool("isJumping", !isGrounded && rb.linearVelocity.y > 0);

            if (Input.GetButtonDown("Fire1") && !isSlashing && Time.time >= lastSlashTime + slashCooldown)
            {
                StartCoroutine(SwordSlash());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        anim.SetBool("isDashing", true);

        // Dash in facing direction
        float dashDirection = transform.localScale.x;
        rb.linearVelocity = new Vector2(dashDirection * dashForce, rb.linearVelocity.y * 0.5f);


        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        anim.SetBool("isDashing", false);

    }
    private IEnumerator SwordSlash()
    {
        isSlashing = true;
        lastSlashTime = Time.time;

        // Enable sword hitbox
        if (swordHitbox != null)
        {
            swordHitbox.SetActive(true);
        }
        yield return new WaitForSeconds(0.2f); // Duration of slash animation
        // Disable sword hitbox
        if (swordHitbox != null)
        {
            swordHitbox.SetActive(false);
        }


        isSlashing = false;
    }
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log("Player took damage, remaining health: " + playerHealth);
        if (playerHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        // Handle player death (e.g., reload scene, show game over screen)
        Debug.Log("Player has died!");
        
    }
    private void OnDrawGizmos()
    {
        // Visualize dash direction in editor
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * dashForce * 0.1f);
    }
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;

    }
    private void DoubleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset vertical velocity
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        doubleJumpUsed = true;
    }
}

