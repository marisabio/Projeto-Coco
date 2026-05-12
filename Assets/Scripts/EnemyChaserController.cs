using UnityEngine;

public class EnemyChaserController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackDamage;
    [SerializeField] private float minDistance;
    [SerializeField] private float attackTimeBuffer;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private float flashDuration;
    [SerializeField] private Material knockbackMaterial;
    [SerializeField] private Transform player;

    private Vector2 playerPosition;
    private Vector2 enemyPosition;
    private float attackTimeCounter;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float direction = 1;
    private Animator animator;
    private bool isFacingRight;
    private Material mainMaterial;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        mainMaterial = spriteRenderer.material;
    }

    void Update()
    {
        playerPosition = (player.transform.position - rb.transform.position).normalized;
        enemyPosition = transform.position;

        // Se lembre de estudar sobre Vector2.Dot depois. Importante!!!
        direction = Vector2.Dot(Vector2.left, enemyPosition - (Vector2)player.transform.position);

        MoveToPlayer();
        FlipSprite();
    }

    private void MoveToPlayer()
    { 
        if (Vector2.Distance(player.transform.position, enemyPosition) > minDistance)
        {
            rb.MovePosition(enemyPosition + playerPosition * (moveSpeed * Time.fixedDeltaTime));
        }
        else if (Vector2.Distance(player.transform.position, enemyPosition) < minDistance)
        {
            rb.MovePosition(enemyPosition - playerPosition * (moveSpeed * Time.fixedDeltaTime));
        }

    }

    public void KnockbackProcess()
    {
        StartFlashDamage();
        animator.SetBool("takingDamage", true);
        rb.linearVelocity = Vector2.zero;
        Vector2 knockbackDirection = (transform.position - player.transform.position).normalized;
        rb.AddForce((knockbackDirection * knockbackForce), ForceMode2D.Impulse);
        Invoke(nameof(EndFlashDamage), flashDuration);
    }
    
    private void StartFlashDamage()
    {
        spriteRenderer.material = knockbackMaterial;
    }
    
    private void EndFlashDamage()
    {
        spriteRenderer.material = mainMaterial;
    }

    // Flipa o sprite
    private void FlipSprite()
    {
        if (direction < 0f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction > 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

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
