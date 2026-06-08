using System.Collections;
using UnityEngine;

public class EggProjectile : MonoBehaviour
{
    [SerializeField] private float eggDamage;
    [SerializeField] private float eggDuration;
    [SerializeField] private float flashDuration;
    [SerializeField] private Material knockbackMaterial;
    [SerializeField] private float knockbackForce;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Material mainMaterial;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainMaterial = spriteRenderer.material;
    }

    void Update()
    {
        StartCoroutine("EggTime");
    }

    public void EggLaunch(Vector3 dir, float force)
    {   
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyDamageController>().TakeDamage(eggDamage);
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            other.gameObject.GetComponent<BossDamageController>().TakeDamage(eggDamage);
        }

        StartCoroutine(KnockbackProcess(other));
    }

    private IEnumerator EggTime()
    {
        yield return new WaitForSeconds(eggDuration);
        Destroy(gameObject);
    }

    private IEnumerator KnockbackProcess(Collision2D other)
    {
        StartFlashDamage();
        rb.linearVelocity = Vector2.zero;
        Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
        rb.AddForce((knockbackDirection * knockbackForce), ForceMode2D.Impulse);
        Invoke(nameof(EndFlashDamage), flashDuration);

        yield return new WaitForSeconds(flashDuration);
        Destroy(gameObject);

    }
    private void StartFlashDamage()
    {
        spriteRenderer.material = knockbackMaterial;
    }
    
    private void EndFlashDamage()
    {
        spriteRenderer.material = mainMaterial;
    }
}
