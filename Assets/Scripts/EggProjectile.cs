using UnityEngine;

public class EggProjectile : MonoBehaviour
{
    [SerializeField] private float eggDamage;
    private Rigidbody2D rb;

    public void EggLaunch(Vector3 dir, float force)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            GetComponent<EnemyDamageController>().TakeDamage(eggDamage);
        }
    }

}
