using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header ("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpAcceleration;
    [SerializeField] private float jumpMaxAcceleration;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferTime;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    [Header ("Combat Settings")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Material knockbackMaterial;
    [SerializeField] private Material restoreMaterial;
    [SerializeField] private float dyingDuration;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackDuration;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask enemyLayer;

    [Header ("Egg Settings")]
    [SerializeField] private GameObject eggProjectile;
    [SerializeField] private Transform eggPosition;
    [SerializeField] private Transform eggTarget;
    [SerializeField] private float eggForce;
    [SerializeField] private float shootDuration;
    [SerializeField] private float shootRate;
    
    [Header ("Input Settings")] 
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction movementAction;
    [SerializeField] private InputAction attackAction;
    [SerializeField] private InputAction interactAction;
    [SerializeField] private InputAction shootingAction;

    [Header ("Unlock Settings")] 
    [SerializeField] private bool unlockDoubleJump;
    [SerializeField] private bool unlockEggAttack;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Material mainMaterial;
    private Collider2D col;
    private bool enableHorizontalControl;
    private bool enableVerticalControl;
    private bool isFacingRight = false;
    private bool isJumping;
    private bool hasJumped = false;
    private bool canDoubleJump = false;
    private bool isGrounded;
    private bool isAttacking = false;
    private bool isActive = true;
    private bool isInteracting = false;
    private bool canInteract = false;
    private float horizontalInput;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float shootRateCounter;
    private float attackTimeCounter;
    private float shootTimeCounter;
    private Vector3 shootDirection;

    void OnEnable()
    {
        EnableCharacterControl();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        mainMaterial = spriteRenderer.material;

        if (PlayerPrefs.GetFloat("health") <= 0)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = PlayerPrefs.GetFloat("health");
        }
    }

    void Update()
    {
        if (isActive)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        PlayerPrefs.SetFloat("health", currentHealth);

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetFloat("savedScene", currentScene);

        MovementProcess();
        JumpingProcess();
        AttackProcess();
        InteractProcess();
        ShootingProcess();
    }

    private void MovementProcess()
    {
        if (enableHorizontalControl)
        {
            horizontalInput = movementAction.ReadValue<float>();

            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocityY);
        }
        
        if (isFacingRight && horizontalInput > 0)
        {
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!isFacingRight && horizontalInput < 0)
        {
            isFacingRight = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (horizontalInput != 0 && isGrounded)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void JumpingProcess()
    {
        if (enableVerticalControl)
        {
            if (isGrounded)
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (jumpAction.WasPressedThisFrame())
            {
                animator.SetBool("isLanding", false);
                jumpBufferCounter = jumpBufferTime;
                
                if ((coyoteTimeCounter > 0f && jumpBufferCounter > 0f) || canDoubleJump) 
                {
                    isJumping = true;
                    animator.Play("Jumping");
                    coyoteTimeCounter = 0f;
                    jumpBufferCounter = 0f;

                    if (isJumping)
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
                        float velocityRatio = rb.linearVelocityY / jumpSpeed;
                        jumpAcceleration = jumpMaxAcceleration * (1 - velocityRatio);
                        rb.linearVelocityY += jumpAcceleration * Time.deltaTime;
                        animator.SetBool("isFalling", true);
                        hasJumped = true;
                    }
                }

                if (unlockDoubleJump)
                {
                    if (hasJumped)
                    {
                        if (!canDoubleJump)
                        {
                            canDoubleJump = true;
                        }
                        else
                        {
                            canDoubleJump = false;
                            hasJumped = false;
                        }
                    }
                }
            }

            else if (!isGrounded)
            {
                animator.SetBool("isLanding", false);
                jumpBufferCounter = jumpBufferTime;
                
                if (!isJumping && !isAttacking) 
                {
                    animator.Play("Fall Jumping");
                    jumpBufferCounter = 0f;
                }        
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }
            
            if (jumpAction.WasReleasedThisFrame())
            {
                isJumping = false;
                animator.SetBool("isFalling", true);
            }

            if (rb.linearVelocityY < 0)
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                isJumping = false;
            }
            else if (rb.linearVelocityY > 0 && !isJumping)
            {         
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                isJumping = false;
            }

            if (!isJumping && !isAttacking)
            {
                animator.SetBool("isFalling", true);
            }

            if (animator.GetBool("isFalling") == true && isGrounded)
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isLanding", true);
                hasJumped = false;
            }
        }
    }


    private void InteractProcess()
    {
        if (interactAction.WasPressedThisFrame() && canInteract)
        {
            isInteracting = true;
        }

    }

    private void AttackProcess()
        {
            if (attackAction.WasPressedThisFrame())
            {
                StartCoroutine("StartAttack");
            }
            else
            {
                EndAttack();
            }
        }
        
    private IEnumerator StartAttack()
    {
        List<GameObject> enemies = new List<GameObject>();
        animator.SetBool("isAttacking", true);
        isAttacking =  true;
        attackTimeCounter = 0f;

        while (attackTimeCounter <= attackDuration)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, enemyLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemies.Contains(enemy.gameObject))
                {
                    continue;
                }
                enemies.Add(enemy.gameObject);
                enemy.GetComponent<EnemyDamageController>().TakeDamage(attackDamage);
                Debug.Log("Hit!!");
            }
        
            attackTimeCounter += Time.deltaTime;

            yield return null;
        }

        isAttacking =  false;
    }

    private void EndAttack()
    {
        if (animator.GetBool("isAttacking"))
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void ShootingProcess()
    {
        if (unlockEggAttack)
        {
            shootRateCounter += Time.deltaTime;
            
            if (shootingAction.WasPressedThisFrame() && !isJumping && shootRateCounter >= shootRate)
            {
                shootRateCounter = 0f;
                StartCoroutine("StartShooting");
            }
            else
            {
                EndShooting();
            }
        }
    }

    private IEnumerator StartShooting()
    {
        animator.SetBool("isShooting", true);
        isAttacking =  true;
        GameObject egg = Instantiate(eggProjectile, eggPosition.position, Quaternion.identity);

        Vector2 eggSpawnPosition = eggPosition.position;
        Vector2 eggTargetPosition = eggTarget.position;

        shootDirection = (eggTargetPosition - eggSpawnPosition).normalized;

        egg.GetComponent<EggProjectile>().EggLaunch(shootDirection, eggForce);        

        while (shootTimeCounter <= shootDuration)
        {
            DisableCharacterControl();
            shootTimeCounter += Time.deltaTime;

            yield return null;
        }

        isAttacking = false;
        EnableCharacterControl();
    }

    private void EndShooting()
    {
        if (animator.GetBool("isShooting"))
        {
            animator.SetBool("isShooting", false);
        }
       
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DisableCharacterControl();
            StartCoroutine("Die");
        }
    }

    public void RestoreDamage(float damage)
    {
        if (currentHealth < 3)
        {
            StartFlashRestore();
            currentHealth += damage;
            Invoke(nameof(EndFlashRestore), knockbackDuration);
        }
    }

    private void StartFlashDamage()
    {
        spriteRenderer.material = knockbackMaterial;
        animator.Play("Damage");
    }

    private void StartFlashRestore()
    {
        spriteRenderer.material = knockbackMaterial;
    }
    
    private void EndFlashDamage()
    {
        spriteRenderer.material = mainMaterial;
    }

        private void EndFlashRestore()
    {
        spriteRenderer.material = mainMaterial;
    }
    
    private IEnumerator Die()
    {
        isActive = false;
        isGrounded = true;
        animator.Play("Dying");
        animator.SetBool("isDying", true);
        Invoke(nameof(DisableCharacterControl), knockbackDuration + 0.1f);
        yield return new WaitForSeconds(knockbackDuration - 0.1f);
        col.isTrigger = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(dyingDuration);
        gameObject.SetActive(false);

        // int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(0);
    }

    private void DisableCharacterControl()
    {
        attackAction.Disable();
        jumpAction.Disable();
        movementAction.Disable();
        interactAction.Disable();
        shootingAction.Disable();
        enableHorizontalControl = false;
        enableVerticalControl = false;
    }

    private void EnableCharacterControl()
    {
        attackAction.Enable();
        jumpAction.Enable();
        movementAction.Enable();
        interactAction.Enable();
        shootingAction.Enable();
        enableHorizontalControl = true;
        enableVerticalControl = true;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            DisableCharacterControl();
            StartFlashDamage();
            Debug.Log("Damage!");
            rb.linearVelocity = Vector2.zero;
            Vector2 knockbackDirection = (transform.position - other.collider.transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            Invoke(nameof(EndFlashDamage), knockbackDuration);
            Invoke(nameof(EnableCharacterControl), knockbackDuration);
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canInteract = true;

            if (isInteracting)
            {
                other.GetComponent<InteractableController>().Interact();
                isInteracting = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canInteract = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }   

}