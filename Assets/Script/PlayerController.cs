using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float wallBounceForce = 30f;
    [SerializeField] private float wallBounceCooldown = 0.2f;
    
    [Header("Physics Settings")]
    [SerializeField] private PhysicsMaterial2D bouncyMaterial;
    
    [Header("Ground Check")]
    [SerializeField] private float groundRayLength = 0.2f;
    [SerializeField] private Vector2 groundRayOffset = new Vector2(0.25f, 0);
    [SerializeField] private LayerMask groundLayer;

    [Header("Health Settings")] 
    [SerializeField] private int health = 3;
    [SerializeField] private float invulnerabilityTime = 1f;
    [SerializeField] private float flashInterval = 0.1f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    
    private bool isGrounded;
    private bool isRunning;
    private bool isFacingRight = true;
    private float horizontalInput;
    
    private bool isTouchingWall;
    private bool wasTouchingWall;
    private float lastWallTouchTime;
    
    private readonly string animState = "AnimState";
    private readonly int IDLE = 0;
    private readonly int WALK = 1;
    private readonly int RUN = 2;
    private readonly int JUMP = 3;
    private readonly int FALL = 4;

    private bool disableHorizontalMovement = false;
    private float movementDisableTimer = 0f;
    private Vector2 wallBounceVelocity;
    
    // Damage handling variables
    private bool isInvulnerable = false;
    private float invulnerabilityTimer = 0f;
    private bool isFlashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        if (boxCollider2D == null)
        {
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        }
        
        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            
            if (bouncyMaterial != null)
            {
                boxCollider2D.sharedMaterial = bouncyMaterial;
            }
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealth(health);
        }
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        
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
        
        if (disableHorizontalMovement)
        {
            movementDisableTimer -= Time.deltaTime;
            if (movementDisableTimer <= 0)
            {
                disableHorizontalMovement = false;
            }
        }
        
        // Update invulnerability
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            
            // Handle flashing effect
            if (spriteRenderer != null)
            {
                isFlashing = !isFlashing;
                if (Time.time % flashInterval < flashInterval / 2)
                    spriteRenderer.enabled = isFlashing;
                else
                    spriteRenderer.enabled = !isFlashing;
            }
            
            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
                if (spriteRenderer != null)
                    spriteRenderer.enabled = true;
            }
        }
    }
    
    void FixedUpdate()
    {
        CheckGrounded();
        CheckWallCollision();
        
        if (!disableHorizontalMovement)
        {
            Move();
        }
        else
        {
            rb.linearVelocity = new Vector2(wallBounceVelocity.x, rb.linearVelocity.y);
        }
        
        ApplyJumpPhysics();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Skip if wall touch is on cooldown
        if (Time.time - lastWallTouchTime < wallBounceCooldown)
            return;
        
        // Check if it's an enemy before checking for wall bounce
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // For enemies, we want to handle the bounce differently
            TakeDamage(1);
            
            // Calculate bounce direction (away from enemy)
            Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
            TriggerWallBounce(bounceDirection.x, wallBounceForce * 0.7f);
            return;
        }
            
        // Normal wall bouncing logic
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                float bounceDirection = contact.normal.x;
                float bounceStrength = wallBounceForce;
                
                TriggerWallBounce(bounceDirection, bounceStrength);
                break;
            }
        }
    }
    
    private void TriggerWallBounce(float direction, float strength)
    {
        wallBounceVelocity = new Vector2(direction * strength, rb.linearVelocity.y + 2f);
        
        disableHorizontalMovement = true;
        movementDisableTimer = 0.2f;
        
        rb.linearVelocity = wallBounceVelocity;
        
        lastWallTouchTime = Time.time;
        
        Debug.Log($"WALL BOUNCE! Direction: {direction}, Strength: {strength}, Velocity: {wallBounceVelocity}");
    }
    
    private void Move()
    {
        float speed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
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
        
        Bounds bounds = boxCollider2D.bounds;
        Vector2 leftOrigin = new Vector2(bounds.min.x + groundRayOffset.x, bounds.min.y);
        Vector2 rightOrigin = new Vector2(bounds.max.x - groundRayOffset.x, bounds.min.y);
        Vector2 centerOrigin = new Vector2(bounds.center.x, bounds.min.y);
        
        bool hitLeft = Physics2D.Raycast(leftOrigin, Vector2.down, groundRayLength, groundLayer);
        bool hitCenter = Physics2D.Raycast(centerOrigin, Vector2.down, groundRayLength, groundLayer);
        bool hitRight = Physics2D.Raycast(rightOrigin, Vector2.down, groundRayLength, groundLayer);
        
        isGrounded = hitLeft || hitCenter || hitRight;
    }
    
    private void CheckWallCollision()
    {
        if (boxCollider2D == null) return;
        
        wasTouchingWall = isTouchingWall;
        
        Bounds bounds = boxCollider2D.bounds;
        Vector2 rightOrigin = new Vector2(bounds.max.x, bounds.center.y);
        Vector2 leftOrigin = new Vector2(bounds.min.x, bounds.center.y);
        
        RaycastHit2D hitRight = Physics2D.Raycast(rightOrigin, Vector2.right, 0.2f, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(leftOrigin, Vector2.left, 0.2f, groundLayer);
        
        Debug.DrawRay(rightOrigin, Vector2.right * 0.2f, hitRight ? Color.yellow : Color.cyan);
        Debug.DrawRay(leftOrigin, Vector2.left * 0.2f, hitLeft ? Color.yellow : Color.cyan);
        
        bool hitRightWall = hitRight.collider != null;
        bool hitLeftWall = hitLeft.collider != null;
        isTouchingWall = hitRightWall || hitLeftWall;
        
        if (isTouchingWall && !wasTouchingWall)
        {
            bool movingTowardWall = (hitRightWall && horizontalInput > 0) || (hitLeftWall && horizontalInput < 0);
            
            if (movingTowardWall && Mathf.Abs(rb.linearVelocity.x) > 2f)
            {
                float bounceDirection = hitRightWall ? -1f : 1f;
                TriggerWallBounce(bounceDirection, wallBounceForce);
            }
        }
    }
    
    private void UpdateAnimations()
    {
        if (animator == null) return;
        
        int animationState;
        
        if (!isGrounded)
        {
            animationState = rb.linearVelocity.y > 0 ? JUMP : FALL;
        }
        else if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            animationState = isRunning ? RUN : WALK;
        }
        else
        {
            animationState = IDLE;
        }
        
        animator.SetInteger(animState, animationState);
    }
    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(int dmg)
    {
        // Skip damage if invulnerable
        if (isInvulnerable) return;
        
        health = health - dmg;
        Debug.Log($"Damage taken: {dmg}, health is now {health}");
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealth(health);
        }
        
        // Enable invulnerability with visual feedback
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityTime;
        
        // TODO: Add any damage animations or sounds here
        
        // Check for game over
        if (health <= 0)
        {
            Debug.Log("GAME OVER - Player has died!");
            SceneManager.LoadScene("CityLooseMenu");
        }
    }
    
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
            
            Vector2 rightWallOrigin = new Vector2(bounds.max.x, bounds.center.y);
            Vector2 leftWallOrigin = new Vector2(bounds.min.x, bounds.center.y);
            
            Gizmos.color = isTouchingWall ? Color.yellow : Color.cyan;
            Gizmos.DrawLine(rightWallOrigin, rightWallOrigin + Vector2.right * 0.2f);
            Gizmos.DrawLine(leftWallOrigin, leftWallOrigin + Vector2.left * 0.2f);
        }
    }
}