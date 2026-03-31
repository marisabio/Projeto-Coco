using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header ("Input Settings")] 
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction movementAction;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isFacingRight = false;
    private bool isJumping;
    private bool isGrounded;
    private float horizontalInput;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    void OnEnable()
    {
        jumpAction.Enable();
        movementAction.Enable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        AnimationProcess();
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

        if (jumpAction.IsPressed())
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f) 
        {
            isJumping = true;
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;

            Debug.Log("Jump!");

            if (isJumping)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
                float velocityRatio = rb.linearVelocityY / jumpSpeed;
                jumpAcceleration = jumpMaxAcceleration * (1 - velocityRatio);
                rb.linearVelocityY += jumpAcceleration * Time.deltaTime;
            }
        }

        if (jumpAction.WasReleasedThisFrame())
        {
            isJumping = false;        
        }

        if (rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (rb.linearVelocityY > 0 && !isJumping)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void AnimationProcess()
    {
        if (horizontalInput != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }   

}