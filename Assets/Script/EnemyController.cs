using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator anim;

    public float moveSpeed = 2f;
    public float moveDistance = 3f;
    public LayerMask playerLayer;
    public float damageRadius = 0.5f;

    private Vector2 startingPosition;
    private bool movingRight = true;
    private float damageTimer = 0f;
    private float damageCooldown = 0.5f;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        Patrol();
        CheckPlayerDamage();
    }

    private void CheckPlayerDamage()
    {
        // Decrease damage timer
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }

        // If cooldown complete, check for player to damage
        if (damageTimer <= 0)
        {
            // Check for overlapping player using a circle cast
            Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, damageRadius, playerLayer);
            
            // If player found, damage them
            if (playerCollider != null)
            {
                PlayerController player = playerCollider.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(1);
                    damageTimer = damageCooldown; // Set cooldown to prevent damage spam
                }
            }
        }
    }

    // We'll still keep this method for compatibility, but it may not work reliably
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && damageTimer <= 0)
            {
                player.TakeDamage(1);
                damageTimer = damageCooldown;
            }
        }
    }

    private void Patrol()
    {
        float displacement = transform.position.x - startingPosition.x;

        // Change direction 
        if (movingRight && displacement >= moveDistance)
            movingRight = false;
        else if (!movingRight && displacement <= -moveDistance)
            movingRight = true;

        // Move 
        float direction = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // Flip 
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * (movingRight ? -1 : 1);
        transform.localScale = localScale;
    }
    
    // Visualize the damage radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}