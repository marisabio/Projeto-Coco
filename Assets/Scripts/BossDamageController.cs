using UnityEngine;
using UnityEngine.Events;

public class BossDamageController : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float deathTime;
    [SerializeField] private AudioClip damageSound;
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;
    private float currentHealth;
    private AudioSource audioSource;
    
    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        audioSource.PlayOneShot(damageSound);
        
        onTakeDamage.Invoke();
        
        if (currentHealth <= 0)
        {
            Invoke(nameof(Die), deathTime);
        }
    }
    private void Die()
    {
        onDeath.Invoke();
    }
    
}
