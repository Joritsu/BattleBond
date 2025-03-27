using UnityEngine;
using Pathfinding;  // Requires the A* Pathfinding Project

public class EnemyPathfinder : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    [Tooltip("The target the enemy will follow (typically the player's transform).")]
    public Transform target;
    [Tooltip("The speed at which the enemy moves along the path.")]
    public float speed = 200f;
    [Tooltip("Distance at which the enemy considers that it has reached a waypoint.")]
    public float nextWaypointDistance = 3f;

    // Pathfinding components.
    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D rb;

    // Update the path every 0.5 seconds.
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        // Start a new path to the target position, and repeat it every 0.5 seconds.
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        else
        {
            Debug.LogWarning("Target is null or Seeker is busy.");
        }
}


    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        else
        {
            Debug.LogError("Path error: " + p.errorLog);
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        // If we've reached the end of the path, do nothing.
        if (currentWaypoint >= path.vectorPath.Count)
            return;

        // Calculate the direction to the current waypoint.
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        // Check if we are close enough to the waypoint.
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    // Optional: Visualize the calculated path in the Scene view.
    void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = currentWaypoint; i < path.vectorPath.Count; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(path.vectorPath[i], 0.1f);

                if (i == currentWaypoint)
                {
                    Gizmos.DrawLine(rb.position, path.vectorPath[i]);
                }
                else
                {
                    Gizmos.DrawLine(path.vectorPath[i - 1], path.vectorPath[i]);
                }
            }
        }
    }
}
