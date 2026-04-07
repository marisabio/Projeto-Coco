using UnityEngine;
using UnityEngine.InputSystem;

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

    // Variáveis de input usando o novo sistema de inputs da Unity. Qualquer coisa, elas também podem ser mudadas no inspector.
    [Header ("Input Settings")] 
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction movementAction;

    // Variáveis aleatórias 
    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = false;
    private bool isJumping;
    private bool isGrounded;
    private float horizontalInput;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    // O novo sistema de input da Unity exige que os inputs sejam ativados no código antes de serem usados. 
    // Isso é útil pq permite que a gente desative eles facilmente durante diálogos e custscenes, se necessário.
    void OnEnable()
    {
        jumpAction.Enable();
        movementAction.Enable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        MovementProcess();
        JumpingProcess();
    }

    void FixedUpdate()
    {
        
    }

    void MovementProcess()
    {
        horizontalInput = movementAction.ReadValue<float>();

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocityY);

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

    void JumpingProcess()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpAction.WasPressedThisDynamicUpdate())
        {
            jumpBufferCounter = jumpBufferTime;
            
            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f) 
            {
                isJumping = true;
                animator.Play("Jumping");
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;

                Debug.Log("Jump!");

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

        if (!isJumping)
        {
            animator.SetBool("isFalling", true);
        }

        if (animator.GetBool("isFalling")  == true && isGrounded)
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isLanding", true);
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }   

}