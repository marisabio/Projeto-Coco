using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] private float restore;

     private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().RestoreDamage(restore);
            Debug.Log("Health restore!!");
            Destroy(gameObject);
        }
    }
}
