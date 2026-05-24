using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrimRatController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackDamage;
    [SerializeField] private float minDistance;
    [SerializeField] private float activeDistance;
    [SerializeField] private float attackTimeBuffer;
    [SerializeField] private float attackDuration;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private float flashDuration;
    [SerializeField] private Material knockbackMaterial;
    [SerializeField] private Transform player;

    private Vector2 playerPosition;
    private Vector2 enemyPosition;
    private float attackBufferTimeCounter;
    private float attackTimeCounter;
    private bool hasAttacked = false;
    private bool isActive = false;
    private bool isAlive = true;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float direction = 1;
    private Animator animator;
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

        direction = Vector2.Dot(Vector2.left, enemyPosition - (Vector2)player.transform.position);
    
        if (isAlive)
        {
            if (Vector2.Distance(player.transform.position, enemyPosition) < activeDistance)
            {
                isActive = true;
            }

            if (hasAttacked)
            {
                attackBufferTimeCounter = attackTimeBuffer;
                hasAttacked = false;
            }
            else
            {
                attackBufferTimeCounter -= Time.deltaTime;
            }
        }

        MoveToPlayer();
        FlipSprite();
    }

    private void MoveToPlayer()
    { 
        if (isActive && isAlive)
        {
            if (Vector2.Distance(player.transform.position, enemyPosition) < minDistance && attackBufferTimeCounter <= 0f)
            {
                rb.MovePosition(enemyPosition + playerPosition * (moveSpeed * Time.fixedDeltaTime));
                animator.SetBool("isWalking", true);
            }
            else if (Vector2.Distance(player.transform.position, enemyPosition) < minDistance && attackBufferTimeCounter >= 0f)
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    public void KnockbackProcess()
    {     
        StartFlashDamage();
        isAlive = false;
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

    private void FlipSprite()
    {
        if (direction > 0f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction < 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // private IEnumerator StartAttack()
    //{
    //    while (attackTimeCounter <= attackDuration)
    //    {
            
    //    }
    //    yield return null;

    // }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && isAlive)
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage);
            hasAttacked = true;
            Debug.Log("Enemy hit!!");
        }
    }
}
