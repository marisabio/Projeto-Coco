using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Debug.Log("Enemy hit!!");
        }
        else if(other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyDamageController>().TakeDamage(damage);
            Debug.Log("Enemy in obstacle!!");
        }
    }
}
