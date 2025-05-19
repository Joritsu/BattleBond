using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private Vector3 originalScale; // Assign this in Inspector
    private Animator animator;
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
    // Flag to prevent multiple jump coroutines.
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale; // On the child!
    }

    void Update()
    {
        float moveX = rb.linearVelocity.x;
        if (Mathf.Abs(moveX) > 0.01f)
        {
            Vector3 scale = originalScale;
            scale.x *= moveX < 0 ? -1f : 1f;
            transform.localScale = scale; // On the child!
        }
        var ec = GetComponent<EnemyController>();
        if (ec != null && ec.isStunned)
            return;
        // Calculate distance to the player.
        float distance = Vector2.Distance(transform.position, player.position);

        // Get the PatrolBehavior and EnemyPathfinder components (if attached).
        PatrolBehavior patrol = GetComponent<PatrolBehavior>();
        EnemyPathfinder pathfinder = GetComponent<EnemyPathfinder>();

        // If the player is within the detection radius, disable patrol and enable pathfinding.
        if (distance <= detectionRadius)
        {
            if (patrol != null)
                patrol.enabled = false;
            if (pathfinder != null)
                pathfinder.enabled = true; // Enable pathfinding chase behavior.
            
            ChasePlayer();
        }
        else
        {
            if (patrol != null)
                patrol.enabled = true;
            if (pathfinder != null)
                pathfinder.enabled = false;
        }
        if (animator != null)
        {
            // Use Mathf.Abs so speed is always positive
            float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
            animator.SetFloat("Speed", horizontalSpeed);

        }

    }

    /// <summary>
    /// When chasing, move horizontally toward the player.
    /// Uses a raycast from groundCheck to detect obstacles; if one is detected and the enemy is grounded,
    /// starts a jump coroutine.
    /// </summary>
    void ChasePlayer()
    {

        // Determine horizontal direction toward the player.
        Vector2 direction = (player.position - transform.position).normalized;

        

        // Perform a raycast from groundCheck in the horizontal direction.
        Vector2 rayOrigin = groundCheck.position;
        Vector2 rayDirection = new Vector2(Mathf.Sign(direction.x), 0);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, obstacleDetectionDistance, groundLayer);

        // If an obstacle is detected and enemy is on the ground and not already jumping, start jump coroutine.
        if (hit.collider != null && isGrounded && !isJumping)
        {
            StartCoroutine(JumpAndResumePathfinding());
        }

        // Set horizontal velocity toward the player.
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
    }

    /// <summary>
    /// Coroutine that makes the enemy jump and re-enables pathfinding after landing.
    /// </summary>
    IEnumerator JumpAndResumePathfinding()
    {
        isJumping = true;
        // Temporarily disable pathfinding.
        EnemyPathfinder pathfinder = GetComponent<EnemyPathfinder>();
        if (pathfinder != null)
            pathfinder.enabled = false;

        // Perform the jump.
        Jump();

        // Wait until the enemy is no longer grounded (i.e., is airborne).
        yield return new WaitWhile(() => isGrounded);
        // Then wait until the enemy lands (is grounded again).
        yield return new WaitUntil(() => isGrounded);

        // Re-enable pathfinding after landing.
        if (pathfinder != null)
            pathfinder.enabled = true;
        isJumping = false;
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

    // Draw gizmos to visualize the detection radius, ground check, and obstacle ray.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

            Gizmos.color = Color.blue;
            Vector2 rayStart = groundCheck.position;
            Vector2 rayDir = Vector2.right * obstacleDetectionDistance;
            // Use sprite flipping to determine ray direction.
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null && sr.flipX)
                rayDir = Vector2.left * obstacleDetectionDistance;
            Gizmos.DrawLine(rayStart, rayStart + rayDir);
        }
    }
}
