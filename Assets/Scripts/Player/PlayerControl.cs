using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    [Header("Movement Settings")]
    public float movSpeed = 5f;
    public float jumpForce = 20f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(1f, 2f);
    public float groundCheckAngle = 0f;

    [Header("Jump Settings")]
    public float jumpCooldown = 0.5f;

    [Header("Ignored Layers")]
    public LayerMask ignoredLayers;

    float speedX;
    bool isGrounded;
    float lastJumpTime;

    // cache these so you’re not reading PlayerPrefs every frame
    KeyCode leftKey, rightKey, jumpKey;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // load your custom bindings once at start
        leftKey  = InputBindings.GetKey("MoveLeft");
        rightKey = InputBindings.GetKey("MoveRight");
        jumpKey  = InputBindings.GetKey("Jump");

        Debug.Log($"Bindings → Left:{leftKey} Right:{rightKey} Jump:{jumpKey}");
    }

    void Update()
    {
            // HORIZONTAL
        KeyCode leftKey  = InputBindings.GetKey("MoveLeft");
        KeyCode rightKey = InputBindings.GetKey("MoveRight");
        KeyCode jumpKey  = InputBindings.GetKey("Jump");

        // movement:
        speedX = 0f;
        if (Input.GetKey(leftKey))  speedX = -movSpeed;
        if (Input.GetKey(rightKey)) speedX =  movSpeed;

        // SPRITE FLIP
        if (speedX < 0) spriteRenderer.flipX = true;
        if (speedX > 0) spriteRenderer.flipX = false;

        // GROUND CHECK
        isGrounded = false;
        foreach (var hit in Physics2D.OverlapBoxAll(groundCheck.position, groundCheckSize, groundCheckAngle))
        {
            if (hit.transform.IsChildOf(transform)) continue;
            if (((1 << hit.gameObject.layer) & ignoredLayers) != 0) continue;
            isGrounded = true;
            break;
        }

        if (Input.GetKeyDown(jumpKey) && isGrounded && Time.time > lastJumpTime + jumpCooldown)
        { 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            lastJumpTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(speedX, rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}
