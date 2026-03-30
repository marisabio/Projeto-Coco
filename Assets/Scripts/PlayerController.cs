using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header ("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] public float jumpForce;
    [SerializeField] public float jumpSpeed;
    [SerializeField] public float jumpAcceleration;
    [SerializeField] public float jumpMaxAcceleration;
    [SerializeField] public float fallMultiplier;
    [SerializeField] public float lowJumpMultiplier;
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
            spriteRenderer.flipX = isFacingRight;
        }
        else if (!isFacingRight && horizontalInput < 0)
        {
            isFacingRight = true;
            spriteRenderer.flipX = isFacingRight;
        }
    }

    void JumpingProcess()
    {
        if (isGrounded && jumpAction.IsPressed()) 
        {
            isJumping = true;
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