using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator anim;

    public float moveSpeed = 2f;
    public float moveDistance = 3f;

    private Vector2 startingPosition;
    private bool movingRight = true;

    private void Start()
    {
        startingPosition = transform.position;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
        }
    }

    private void Update()
    {
        Patrol();
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
}
