using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    
    [Header("Movement Settings")]
    public float movSpeed = 5f;    // Speed for left/right movement
    public float jumpForce = 20f;  // Force applied for jumping

    [Header("Ground Detection")]
    public Transform groundCheck;         // Empty GameObject to detect ground
    public Vector2 groundCheckSize = new Vector2(1f, 2f);  // Size of the box for ground detection
    public float groundCheckAngle = 0f;   // Angle (in degrees) for the OverlapBox (set to 0 if not needed)

    [Header("Jump Settings")]
    public float jumpCooldown = 0.5f;  // Time between jumps

    [Header("Ignored Layers")]
    [Tooltip("Layers to ignore for ground detection (e.g., weapon layer)")]
    public LayerMask ignoredLayers;  // Set this in the Inspector (e.g., to ignore the weapon layer)

    float speedX;
    bool isGrounded;
    float lastJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get horizontal input.
        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;

        // Ground check using OverlapBoxAll.
        Collider2D[] hits = Physics2D.OverlapBoxAll(groundCheck.position, groundCheckSize, groundCheckAngle);
        isGrounded = false;
        foreach (Collider2D hit in hits)
        {
            // Ignore colliders that are part of the player.
            if (hit.transform.IsChildOf(transform))
                continue;
            // Ignore colliders that are on one of the ignored layers.
            if (((1 << hit.gameObject.layer) & ignoredLayers) != 0)
                continue;
            // If any collider remains, we are grounded.
            isGrounded = true;
            break;
        }

        // Jump input with cooldown.
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && Time.time > lastJumpTime + jumpCooldown)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            lastJumpTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        // Preserve vertical velocity while setting horizontal velocity.
        rb.linearVelocity = new Vector2(speedX, rb.linearVelocity.y);
    }

    // Optional: Visualize the ground check area in the Scene view.
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}
