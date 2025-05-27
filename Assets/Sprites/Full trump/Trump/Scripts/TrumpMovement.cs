using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;

    [Header("Double Tap Settings")]
    public float doubleTapThreshold = 0.3f;
    private float lastLeftTapTime = -1f;
    private float lastRightTapTime = -1f;
    private bool isRunning = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Components")]
    private Rigidbody2D rb;
    public Animator animator; // Assign your Animator component in the Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // It's good practice to ensure InputBindings are initialized.
        // Though GetKey() in InputBindings already calls Initialize(),
        // you might consider a central initialization point in your game.
        // For example, in a GameManager script's Awake(): InputBindings.Initialize();
    }

    void Update()
    {
        // Ground check using OverlapCircle.
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);

        // Jumping
        if (Input.GetKeyDown(InputBindings.GetKey("Jump")) && isGrounded) // MODIFIED
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // Optionally trigger a jump animation:
            animator.SetTrigger("Jump");
        }

        // Horizontal input.
        float moveInput = 0f;
        if (Input.GetKey(InputBindings.GetKey("MoveRight"))) // MODIFIED
            moveInput = -1f;
        else if (Input.GetKey(InputBindings.GetKey("MoveLeft"))) // MODIFIED
            moveInput = 1f;

        // Detect double-tap for running.
        if (Input.GetKeyDown(InputBindings.GetKey("MoveLeft"))) // MODIFIED
        {
            float timeSinceLastLeft = Time.time - lastLeftTapTime;
            if (timeSinceLastLeft < doubleTapThreshold)
                isRunning = true;
            lastLeftTapTime = Time.time;
        }
        if (Input.GetKeyDown(InputBindings.GetKey("MoveRight"))) // MODIFIED
        {
            float timeSinceLastRight = Time.time - lastRightTapTime;
            if (timeSinceLastRight < doubleTapThreshold)
                isRunning = true;
            lastRightTapTime = Time.time;
        }
        if (Mathf.Approximately(moveInput, 0f))
        {
            isRunning = false;
        }

        // Choose speed based on running.
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        // Update animator parameters.
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsRunning", isRunning);

        // Optional: Flip character sprite based on direction.
        if (moveInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = moveInput > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    // Optional: Visualize ground check.
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}