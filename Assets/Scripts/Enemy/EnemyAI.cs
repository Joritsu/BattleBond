using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Target & Detection")]
    [Tooltip("Reference to the player's transform.")]
    public Transform player;
    [Tooltip("Distance within which the enemy starts chasing the player.")]
    public float detectionRadius = 10f;

    [Header("Movement Settings")]
    [Tooltip("Horizontal movement speed of the enemy.")]
    public float speed = 3f;
    [Tooltip("Vertical force applied when the enemy jumps.")]
    public float jumpForce = 5f;

    [Header("Ground & Obstacle Detection")]
    [Tooltip("An empty GameObject positioned at the enemy's feet for ground checking.")]
    public Transform groundCheck;
    [Tooltip("Radius for ground detection.")]
    public float groundCheckRadius = 0.2f;
    [Tooltip("LayerMask that defines what is considered ground.")]
    public LayerMask groundLayer;
    [Tooltip("Distance ahead of the enemy to check for obstacles.")]
    public float obstacleDetectionDistance = 1f;

    // Reference to the enemy's Rigidbody2D
    private Rigidbody2D rb;
    // Boolean to store if enemy is currently on the ground.
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Calculate distance to the player.
        float distance = Vector2.Distance(transform.position, player.position);

        // Get the PatrolBehavior component (if attached)
        PatrolBehavior patrol = GetComponent<PatrolBehavior>();

        // If the player is within the detection radius, disable patrol and chase the player.
        if (distance <= detectionRadius)
        {
            if (patrol != null)
                patrol.enabled = false;  // Disable patrol behavior.
            ChasePlayer();
        }
        else
        {
            // If the player is not detected, enable patrol behavior.
            if (patrol != null)
                patrol.enabled = true;
            //Idle();  // Or let the PatrolBehavior script handle movement.
        }
    }
    /// <summary>
    /// When chasing, move horizontally toward the player.
    /// Use a raycast from the groundCheck position in the horizontal direction
    /// to detect obstacles; if one is detected and the enemy is grounded, jump.
    /// </summary>
    void ChasePlayer()
    {
        // Determine horizontal direction toward the player.
        Vector2 direction = (player.position - transform.position).normalized;
        //Optionally flip the enemy sprite if needed.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (direction.x < 0)
                sr.flipX = true;
            else if (direction.x > 0)
                sr.flipX = false;
        }


        // Perform a raycast from groundCheck in the horizontal direction.
        Vector2 rayOrigin = groundCheck.position;
        // Only consider horizontal direction (x component) for obstacle detection.
        Vector2 rayDirection = new Vector2(direction.x, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, obstacleDetectionDistance, groundLayer);

        // If an obstacle is detected ahead and the enemy is on the ground, jump.
        if (hit.collider != null && isGrounded)
        {
            Jump();
        }

        // Set horizontal velocity toward the player.
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
    }

    /// <summary>
    /// Idle state: stop horizontal movement.
    /// </summary>
    void Idle()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    /// <summary>
    /// Applies upward force to make the enemy jump.
    /// </summary>
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void FixedUpdate()
    {
        // Update the grounded status using an OverlapCircle.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // Draw gizmos to visualize the detection radius, ground check area, and obstacle raycast.
    void OnDrawGizmosSelected()
    {
        // Draw the detection radius around the enemy.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw the ground check circle.
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

            // Draw the obstacle detection ray.
            Gizmos.color = Color.blue;
            Vector2 rayStart = groundCheck.position;
            Vector2 rayDir = Vector2.right * obstacleDetectionDistance;
            // If the enemy is flipped, invert the ray.
            if (transform.localScale.x < 0)
                rayDir = Vector2.left * obstacleDetectionDistance;
            Gizmos.DrawLine(rayStart, rayStart + rayDir);
        }
    }
}
