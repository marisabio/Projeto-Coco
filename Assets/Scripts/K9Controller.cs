using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K9Controller : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackDamage;
    [SerializeField] private float minDistance;
    [SerializeField] private float activeDistance;
    [SerializeField] private float vulnerableTime;
    [SerializeField] private float attackTimeBuffer;
    [SerializeField] private float attackDuration;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private float flashDuration;
    [SerializeField] private Material knockbackMaterial;
    [SerializeField] private float attackRadius;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;

    private Vector2 playerPosition;
    private Vector2 dogPosition;
    private float attackBufferTimeCounter;
    private float vulnerableTimeCounter;
    private float attackTimeCounter;
    private bool hasAttacked = false;
    //private bool isAttacking = false;
    private bool isActive = false;
    private bool isTakingDamage = false;
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
        dogPosition = transform.position;

        direction = Vector2.Dot(Vector2.left, dogPosition - (Vector2)player.transform.position);
    
        if (isAlive)
        {
            if (Vector2.Distance(player.transform.position, dogPosition) < activeDistance)
            {
                isActive = true;
            }
            if (isActive)
            {
                if (hasAttacked)
                {
                    attackBufferTimeCounter = attackTimeBuffer;
                    vulnerableTimeCounter = vulnerableTime;
                    hasAttacked = false;
                }
                else if (attackBufferTimeCounter <= 0 && !isTakingDamage)
                {
                    StartCoroutine("StartAttack");
                }
                else
                {
                    attackBufferTimeCounter -= Time.deltaTime;
                    vulnerableTimeCounter -= Time.deltaTime;
                }
            }
        }

        
        FlipSprite();
    }

    void FixedUpdate()
    {
        MoveToPlayer();
    } 

    private void MoveToPlayer()
    { 
        if (isActive && isAlive)
        {
            if (Vector2.Distance(player.transform.position, dogPosition) < minDistance && !isTakingDamage && vulnerableTimeCounter <= 0)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                rb.MovePosition(dogPosition + playerPosition * (moveSpeed * Time.fixedDeltaTime));
                animator.SetBool("isWalking", true);
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                animator.SetBool("isWalking", false);
            }
        }
    }

    public void KnockbackProcess()
    {     
        StartFlashDamage();
        //animator.SetBool("takingDamage", true);
        Vector2 knockbackDirection = (transform.position - player.transform.position).normalized;
        rb.AddForce((knockbackDirection * knockbackForce), ForceMode2D.Impulse);
        Invoke(nameof(EndFlashDamage), flashDuration);
    }
    
    private void StartFlashDamage()
    {
        isTakingDamage = true;
        spriteRenderer.material = knockbackMaterial;
    }
    
    private void EndFlashDamage()
    {
        isTakingDamage = false;
        rb.linearVelocity = Vector2.zero;
        //animator.SetBool("takingDamage", false);
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

    private IEnumerator StartAttack()
    {
        List<GameObject> hitList = new List<GameObject>();
        animator.Play("K9 Attacking");
        attackTimeCounter = 0f;
        //isAttacking = true;

        while (attackTimeCounter <= attackDuration)
        {
            attackTimeCounter += Time.deltaTime;

            yield return null;
        }

        hasAttacked = true;
        //isAttacking = false;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && isAlive && !isTakingDamage)
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage);
            hasAttacked = true;
            Debug.Log("Enemy hit!!");
        }
    }
}
