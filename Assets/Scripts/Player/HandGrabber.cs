using UnityEngine;

public class HandGrabber : MonoBehaviour
{
    [Header("Grab Settings")]
    public float grabRadius = 0.5f;        // How close an object must be to grab.
    public LayerMask grabbableLayer;       // Layer for grabbable objects.
    public Vector3 grabOffset = Vector3.zero;  // Offset to position the grabbed object relative to the hand.
    
    [Header("Player Reference")]
    public Collider2D playerCollider;      // Assign the player's Collider2D in the Inspector.
    
    [Header("Additional Colliders to Ignore")]
    public Collider2D[] additionalIgnoreColliders;  // E.g., your torso colliders

    private GameObject grabbedObject;      // The currently grabbed object.
    private Collider2D grabbedCollider;    // The grabbed object's collider.
    private Collider2D handCollider;       // The hand's collider.

    void Start()
    {
        handCollider = GetComponent<Collider2D>();
        // Ignore collisions between the hand and the player.
        if (playerCollider != null && handCollider != null)
        {
            Physics2D.IgnoreCollision(handCollider, playerCollider, true);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            AttemptGrab();
        if (Input.GetMouseButtonUp(0))
            ReleaseGrab();
    }

    void AttemptGrab()
    {
        // Look for objects within the grab radius.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, grabRadius, grabbableLayer);
        
        if (colliders.Length > 0)
        {
            grabbedObject = colliders[0].gameObject;
            grabbedCollider = grabbedObject.GetComponent<Collider2D>();

            // Disable collisions between the player's collider and the grabbed object.
            if (playerCollider != null && grabbedCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, grabbedCollider, true);
            }
            // Also ignore collisions between the hand and the grabbed object.
            if (handCollider != null && grabbedCollider != null)
            {
                Physics2D.IgnoreCollision(handCollider, grabbedCollider, true);
            }
            // Disable collisions with additional colliders (like torso colliders)
            if (additionalIgnoreColliders != null && grabbedCollider != null)
            {
                foreach (Collider2D col in additionalIgnoreColliders)
                {
                    Physics2D.IgnoreCollision(col, grabbedCollider, true);
                }
            }
            
            // Optional: disable physics on the grabbed object so it doesn't fight the IK movement.
            Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;

            }
            
            // Parent the object to the hand while preserving its world transform.
            grabbedObject.transform.SetParent(transform, true);
            
            // Optionally, if you want to apply a positional offset:
            Vector3 desiredWorldPosition = transform.TransformPoint(grabOffset);
            grabbedObject.transform.position = desiredWorldPosition;
        }
    }

    void ReleaseGrab()
    {
        if (grabbedObject != null)
        {
            // Unparent the grabbed object.
            grabbedObject.transform.SetParent(null);
            
            // Re-enable collisions between the player and the grabbed object.
            if (playerCollider != null && grabbedCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, grabbedCollider, false);
            }
            // Re-enable collisions between the hand and the grabbed object.
            if (handCollider != null && grabbedCollider != null)
            {
                Physics2D.IgnoreCollision(handCollider, grabbedCollider, false);
            }
            // Re-enable collisions with additional colliders.
            if (additionalIgnoreColliders != null && grabbedCollider != null)
            {
                foreach (Collider2D col in additionalIgnoreColliders)
                {
                    Physics2D.IgnoreCollision(col, grabbedCollider, false);
                }
            }
            
            // Restore its physics.
            Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;

            }
            
            grabbedObject = null;
            grabbedCollider = null;
        }
    }

    // Visualize the grab radius in the editor.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
}
