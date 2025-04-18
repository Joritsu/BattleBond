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
        var ec = GetComponent<EnemyController>();
        if (ec != null && ec.isStunned)
            return;
        //Debug.Log("PatrolBehavior Update running");

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

        if (groundCheck != null)
        {
            bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            //Debug.Log("isGrounded: " + isGrounded);

            // Cast the ray
            Vector2 rayOrigin = groundCheck.position;
            Vector2 rayDirection = new Vector2(moveDirection.x, 0);
            //Debug.Log("Raycasting from " + rayOrigin + " in direction " + rayDirection);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, obstacleDetectionDistance, groundLayer);

            if (hit.collider != null)
            {
                float heightDifference = hit.collider.bounds.max.y - groundCheck.position.y;
                //Debug.Log("Detected obstacle with height difference: " + heightDifference);
                if (heightDifference > 0 && heightDifference < jumpableObstacleHeightThreshold)
                {
                    //Debug.Log("Jumping!");
                    Jump();
                }
            }
            else
            {
                //Debug.Log("No obstacle detected");
            }
        }
        else
        {
            //Debug.Log("groundCheck is not assigned");
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
