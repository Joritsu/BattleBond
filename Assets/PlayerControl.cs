using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    public float movSpeed = 5f;  // Speed for left/right movement
    public float jumpForce = 20f; // Force applied for jumping
    public Transform groundCheck; // Empty game object to detect ground
    public LayerMask groundLayer; // Layer for ground detection

    public float jumpCooldown = 0.5f; // Time between jumps
    public Vector2 groundCheckSize = new Vector2(1f, 2f); // Size of the box for ground detection

    float speedX;
    bool isGrounded;
    float lastJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get player input for horizontal movement
        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;

        // Ground check using OverlapBox
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0.2f, groundLayer);

        // Jump input with cooldown check
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && Time.time > lastJumpTime + jumpCooldown)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Apply jumpForce to Y
            lastJumpTime = Time.time; 
        }
    }

    void FixedUpdate()
    {
        // Preserve Y velocity while moving horizontally and applying linear velocity
        rb.linearVelocity = new Vector2(speedX, rb.linearVelocity.y); // Set X velocity and preserve Y velocity
    }
}
