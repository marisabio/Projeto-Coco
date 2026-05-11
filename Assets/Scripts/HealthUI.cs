using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    float playerHealth;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        playerHealth = PlayerPrefs.GetFloat("health");
        int health = Mathf.FloorToInt(playerHealth);
        
        switch(health)
        {
            case >= 3:
            animator.Play("Full Health");
            break;
            case 2:
            animator.Play("Damage 01");
            break;
            case 1:
            animator.Play("Damage 02");
            break;
            case <= 0:
            animator.Play("Damage 03");
            break;
        }
    }
}