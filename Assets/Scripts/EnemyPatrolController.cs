using UnityEngine;

public class EnemyPatrolController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackDamage;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float direction = 1;
    private bool isFacingRight;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isFacingRight)
        {
            rb.linearVelocity = new Vector2(-direction * moveSpeed, rb.linearVelocityY);
        }
        else if (!isFacingRight)
        {
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocityY);
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Patrol Zone"))
        {
            Debug.Log("Teste");

            if (isFacingRight)
            {
                isFacingRight = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (!isFacingRight)
            {
                isFacingRight = true;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    // Causa dano pro jogador
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage);
            Debug.Log("Enemy hit!!");
        }
    }
}
