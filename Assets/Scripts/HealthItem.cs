using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] private float restore;
    [SerializeField] private AudioClip restoreSound;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D col;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerController>().currentHealth < 3)
            {
                audioSource.PlayOneShot(restoreSound);
                other.gameObject.GetComponent<PlayerController>().RestoreDamage(restore);
                Debug.Log("Health restore!!");
                
                spriteRenderer.enabled = false;
                col.enabled = false;
            }
            
        }
    }
}
