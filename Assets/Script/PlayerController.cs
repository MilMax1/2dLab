using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    
    [Header("Ground Check")]
    [SerializeField] private float groundRayLength = 0.2f;
    [SerializeField] private Vector2 groundRayOffset = new Vector2(0.25f, 0);
    [SerializeField] private LayerMask groundLayer;

    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    
    // States
    private bool isGrounded;
    private bool isRunning;
    private bool isFacingRight = true;
    
    // Movement
    private float horizontalInput;
    
    // Animation parameter names - updated to match standard animation parameter names
    private readonly string animState = "AnimState";  // Integer parameter to control animation state
    private readonly int IDLE = 0;
    private readonly int WALK = 1;
    private readonly int RUN = 2;
    private readonly int JUMP = 3;
    private readonly int FALL = 4;
    
    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        if (boxCollider2D == null)
        {
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            Debug.LogWarning("BoxCollider2D was not found and has been automatically added.");
        }
    }
    
    void Update()
    {
        // Get input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        
        // Check if player is trying to jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        
        UpdateAnimations();
        
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }
    
    void FixedUpdate()
    {
        CheckGrounded();
        
        Move();
        
        ApplyJumpPhysics();
    }
    
    private void Move()
    {
        float speed = isRunning ? runSpeed : walkSpeed;
        
        Vector2 targetVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
        rb.linearVelocity = targetVelocity;
    }
    
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    
    private void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        
    }
    
    private void CheckGrounded()
    {
        if (boxCollider2D == null) return;
        
        // Calculate raycast origin points at the bottom corners of the collider
        Bounds bounds = boxCollider2D.bounds;
        Vector2 leftOrigin = new Vector2(bounds.min.x + groundRayOffset.x, bounds.min.y);
        Vector2 rightOrigin = new Vector2(bounds.max.x - groundRayOffset.x, bounds.min.y);
        Vector2 centerOrigin = new Vector2(bounds.center.x, bounds.min.y);
        
        // Cast rays from the left, center, and right of the collider
        bool hitLeft = Physics2D.Raycast(leftOrigin, Vector2.down, groundRayLength, groundLayer);
        bool hitCenter = Physics2D.Raycast(centerOrigin, Vector2.down, groundRayLength, groundLayer);
        bool hitRight = Physics2D.Raycast(rightOrigin, Vector2.down, groundRayLength, groundLayer);
        
        // Player is grounded if any ray hits the ground
        isGrounded = hitLeft || hitCenter || hitRight;
    }
    
    private void UpdateAnimations()
    {
        if (animator != null)
        {
            // Determine animation state based on player's current state
            int animationState;
            
            if (!isGrounded)
            {
                if (rb.linearVelocity.y > 0)
                {
                    // Jumping
                    animationState = JUMP;
                }
                else
                {
                    // Falling
                    animationState = FALL;
                }
            }
            else if (Mathf.Abs(horizontalInput) > 0.1f)
            {
                if (isRunning)
                {
                    // Running
                    animationState = RUN;
                }
                else
                {
                    // Walking
                    animationState = WALK;
                }
            }
            else
            {
                // Idle
                animationState = IDLE;
            }
            
            // Set the animation state
            animator.SetInteger(animState, animationState);
            
            Debug.Log($"Animation State: {animationState}, Grounded: {isGrounded}, HInput: {horizontalInput}, Running: {isRunning}");
        }
    }
    
    private void Flip()
    {
        // Flip the sprite by changing the X scale
        isFacingRight = !isFacingRight;
        
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    // Draw gizmos for ground check ray visualization in the editor
    void OnDrawGizmosSelected()
    {
        if (boxCollider2D == null)
            boxCollider2D = GetComponent<BoxCollider2D>();
            
        if (boxCollider2D != null)
        {
            Bounds bounds = boxCollider2D.bounds;
            Vector2 leftOrigin = new Vector2(bounds.min.x + groundRayOffset.x, bounds.min.y);
            Vector2 rightOrigin = new Vector2(bounds.max.x - groundRayOffset.x, bounds.min.y);
            Vector2 centerOrigin = new Vector2(bounds.center.x, bounds.min.y);
            
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(leftOrigin, leftOrigin + Vector2.down * groundRayLength);
            Gizmos.DrawLine(centerOrigin, centerOrigin + Vector2.down * groundRayLength);
            Gizmos.DrawLine(rightOrigin, rightOrigin + Vector2.down * groundRayLength);
        }
    }
}