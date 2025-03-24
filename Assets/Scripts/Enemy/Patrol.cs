using UnityEngine;

public class PatrolBehavior : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("Array of waypoints that the enemy will patrol in order.")]
    public Transform[] waypoints;
    [Tooltip("Speed at which the enemy patrols.")]
    public float patrolSpeed = 2f;
    [Tooltip("Distance to a waypoint to consider it reached.")]
    public float waypointTolerance = 0.2f;

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
        // If there are no waypoints, do nothing.
        if (waypoints.Length == 0) return;

        // Get the current target waypoint.
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        // Calculate the horizontal direction to the waypoint.
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;

        // Set horizontal velocity, preserving vertical velocity.
        rb.linearVelocity = new Vector2(direction.x * patrolSpeed, rb.linearVelocity.y);

        // Check if we've reached the waypoint.
        if (Mathf.Abs(transform.position.x - targetWaypoint.position.x) < waypointTolerance)
        {
            // Move to the next waypoint in the array, looping back if necessary.
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        // Flip the sprite using the SpriteRenderer's flipX property rather than modifying localScale.
        if (sr != null)
        {
            sr.flipX = direction.x < 0;
        }
    }

    // Optional: Visualize the patrol waypoints in the Scene view.
    void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Gizmos.color = Color.cyan;
        foreach (Transform wp in waypoints)
        {
            if (wp != null)
                Gizmos.DrawSphere(wp.position, 0.2f);
        }
    }
}
