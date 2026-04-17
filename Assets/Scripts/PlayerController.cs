using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Essas são todas as variáveis que cuidam do movimento da Dorotéia. O pulo dela tem muita variáveis! É para parecer um pouco mais natural e responsivo
    // Tudo pode ser mudado no inspector dentro da Unity em si
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

    // Variáveis de combate, ainda em desenvolvimento
    [Header ("Combat Settings")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Material knockbackMaterial;
    [SerializeField] private float dyingDuration;

    // Variáveis de input usando o novo sistema de inputs da Unity. Qualquer coisa, elas também podem ser mudadas no inspector.
    [Header ("Input Settings")] 
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction movementAction;

    // Variáveis aleatórias 
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Material mainMaterial;
    private bool enableHorizontalControl;
    private bool enableVerticalControl;
    private bool isFacingRight = false;
    private bool isJumping;
    private bool isGrounded;
    private float currentHealth;
    private float horizontalInput;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    // O novo sistema de input da Unity exige que os inputs sejam ativados no código antes de serem usados. 
    // Isso é útil pq permite que a gente desative eles facilmente durante diálogos e custscenes, se necessário.
    void OnEnable()
    {
        EnableCharacterControl();
    }

    // No start a gente instanceia os componentes do Player para serem usados no código.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        mainMaterial = spriteRenderer.material;
    }

    // No update vamos deixar os "processos" relacionados a diferentes elementos de gameplay da Dorotéia,
    // assim como outros elementos de física que precisam ser checados pro frame
    void Update()
    {
        // Isso aqui checa se tem um chão embaixo da Dorotéia. Bem importante!
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        PlayerPrefs.SetFloat("health", currentHealth);

        MovementProcess();
        JumpingProcess();
    }

    // Esse trecho de código cuida do processo de movimento da Dorotéia. No caso, o movimento de direita pra esquerda!
    void MovementProcess()
    {
        if (enableHorizontalControl)
        {
            // O input da direção será lido aqui e colocado numa variável.
            horizontalInput = movementAction.ReadValue<float>();

            // A velocidade da Dorotéia vai aumentar dependendo da direção do input.
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocityY);
        }
        
        // Código pra flipar o sprite.
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

        // Aqui controla a animação de andar da Dorotéia. O input tem que ser maior que zero e ela precisar estar no chão.
        if (horizontalInput != 0 && isGrounded)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    // Esse trecho aqui cuida do processo de pulo. É meio complexo e tomou um tico mais do meu tempo do que eu gostaria,
    // mas acho que o resultado final ficou um pouco melhor do que só seguir o tutorial mais básico na web de pulo de platformer.
    void JumpingProcess()
    {
        if (enableVerticalControl)
        {
            //Esse trechinho aqui cuida do "coyote jump" da Dorotéia.
            if (isGrounded)
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            // Esse trecho cuida do processo do pulo no momento em que o jogador aperta o botão de pulo.
            // Também cuida do buffer do pulo, um tempinho a mais de reação pro pulo pro jogo ficar um pouco mais responsivo.
            if (jumpAction.WasPressedThisFrame())
            {
                animator.SetBool("isLanding", false);
                jumpBufferCounter = jumpBufferTime;
                
                if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f) 
                {
                    isJumping = true;
                    animator.Play("Jumping");
                    coyoteTimeCounter = 0f;
                    jumpBufferCounter = 0f;

                    // Isso cuida da física do pulo em si. A aceleração do pulo da Dorotéia e tudo mais.
                    if (isJumping)
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
                        float velocityRatio = rb.linearVelocityY / jumpSpeed;
                        jumpAcceleration = jumpMaxAcceleration * (1 - velocityRatio);
                        rb.linearVelocityY += jumpAcceleration * Time.deltaTime;
                        animator.SetBool("isFalling", true);
                    }
                }        
            }
            // Esse trechinho faz com que ela entre no ciclo de animação e física do pulo mesmo caso ela não pule,
            // mas esteja caindo de uma plataforma ou coisa assim. 
            else if (!isGrounded)
            {
                animator.SetBool("isLanding", false);
                jumpBufferCounter = jumpBufferTime;
                
                if (!isJumping) 
                {
                    animator.Play("Fall Jumping");
                    jumpBufferCounter = 0f;
                }        
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }
            
            // A aceleração do pulo é interrompida caso o jogador solte o botão antes do ápice do pulo.
            if (jumpAction.WasReleasedThisFrame())
            {
                isJumping = false;
                animator.SetBool("isFalling", true);
            }

            // Esses dois trechinhos do código aceleram a queda do jogador dependendo do ápice do pulo.
            // Pode parecer meio esquisito mas muito platformer usa isso pra deixar a queda um pouco mas responsiva.
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

            // Esse restinho do código determina o resto da animação de pulo, constando a queda e a aterrissagem.
            if (!isJumping)
            {
                animator.SetBool("isFalling", true);
            }

            if (animator.GetBool("isFalling") == true && isGrounded)
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isLanding", true);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine("Die");
        }
    }

    private void StartFlashDamage()
    {
        spriteRenderer.material = knockbackMaterial;
        animator.Play("Damage");
    }
    
    private void EndFlashDamage()
    {
        spriteRenderer.material = mainMaterial;
    }
    
    private IEnumerator Die()
    {
        Invoke(nameof(DisableCharacterControl), knockbackDuration + 0.1f);
        animator.Play("Dying");
        yield return new WaitForSeconds(dyingDuration);     
        gameObject.SetActive(false);
    }

    private void DisableCharacterControl()
    {
        jumpAction.Disable();
        movementAction.Disable();
        enableHorizontalControl = false;
        enableVerticalControl = false;
    }

    private void EnableCharacterControl()
    {
        jumpAction.Enable();
        movementAction.Enable();
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

    // Isso aqui é só pra ser possível ver o círculo do detector de chão no editor da Unity.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }   

}