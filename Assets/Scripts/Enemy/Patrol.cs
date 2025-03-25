using UnityEngine;

public class PatrolBehavior : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("Array of waypoints that the enemy will patrol in order.")]
    public Transform[] waypoints;
    [Tooltip("Speed at which the enemy patrols.")]
    public float patrolSpeed = 2f;
    [Tooltip("Horizontal distance to a waypoint to consider it reached.")]
    public float waypointTolerance = 0.2f;

    [Header("Obstacle & Jump Settings")]
    [Tooltip("An empty GameObject positioned at the enemy's feet for ground checking.")]
    public Transform groundCheck;
    [Tooltip("Radius for ground detection.")]
    public float groundCheckRadius = 0.2f;
    [Tooltip("LayerMask that defines what is considered ground and obstacles.")]
    public LayerMask groundLayer;
    [Tooltip("Distance ahead of the enemy to check for obstacles.")]
    public float obstacleDetectionDistance = 1f;
    [Tooltip("Vertical force applied when the enemy jumps.")]
    public float jumpForce = 5f;
    [Tooltip("Maximum height difference (in world units) of a detected obstacle that is jumpable.")]
    public float jumpableObstacleHeightThreshold = 0.5f;

    // Internal index to track the current waypoint.
    private int currentWaypointIndex = 0;
    // Cached components.
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // If no waypoints are set, do nothing.
        if (waypoints == null || waypoints.Length == 0)
            return;

        // Get the current patrol waypoint.
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Calculate the horizontal difference to the waypoint.
        float directionX = targetWaypoint.position.x - transform.position.x;
        // Determine horizontal movement direction.
        Vector2 moveDirection = new Vector2(Mathf.Sign(directionX), 0);

        // Set horizontal velocity while preserving vertical velocity.
        rb.linearVelocity = new Vector2(moveDirection.x * patrolSpeed, rb.linearVelocity.y);

        // If we're close enough horizontally to the waypoint, move to the next.
        if (Mathf.Abs(directionX) < waypointTolerance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        // Flip the sprite based on movement direction.
        if (sr != null)
        {
            sr.flipX = moveDirection.x < 0;
        }

        // --- Obstacle Detection & Jumping ---
        if (groundCheck != null)
        {
            // Check if the enemy is grounded.
            bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            if (isGrounded)
            {
                // Cast a ray from groundCheck in the horizontal movement direction.
                Vector2 rayOrigin = groundCheck.position;
                Vector2 rayDirection = new Vector2(moveDirection.x, 0);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, obstacleDetectionDistance, groundLayer);

                // If an obstacle is hit…
                if (hit.collider != null)
                {
                    // Calculate how high the obstacle is relative to our ground check.
                    // hit.collider.bounds.max.y is the highest Y of the obstacle.
                    float heightDifference = groundCheck.position.y - hit.collider.bounds.max.y;

                    // If the obstacle is below groundCheck and within the jumpable threshold, jump.
                    if (heightDifference > 0 && heightDifference < jumpableObstacleHeightThreshold)
                    {
                        Jump();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Applies upward force to make the enemy jump.
    /// </summary>
    void Jump()
    {
        // Set the vertical component of velocity to jumpForce while preserving horizontal velocity.
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // Optional: Visualize waypoints, ground check, and obstacle ray in the Scene view.
    void OnDrawGizmosSelected()
    {
        // Draw patrol waypoints.
        if (waypoints != null)
        {
            Gizmos.color = Color.cyan;
            foreach (Transform wp in waypoints)
            {
                if (wp != null)
                    Gizmos.DrawSphere(wp.position, 0.2f);
            }
        }

        // Draw ground check circle.
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

            // Draw obstacle detection ray.
            Gizmos.color = Color.blue;
            Vector2 rayOrigin = groundCheck.position;
            // Determine ray direction based on the enemy's horizontal velocity.
            float rayDirX = 1f;
            if (rb != null)
                rayDirX = Mathf.Sign(rb.linearVelocity.x);
            Vector2 rayDir = new Vector2(rayDirX, 0) * obstacleDetectionDistance;
            Gizmos.DrawLine(rayOrigin, rayOrigin + rayDir);
        }
    }
}
