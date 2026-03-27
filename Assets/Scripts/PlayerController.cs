using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] public float jumpSpeed;
    [SerializeField] public float fallMultiplier;
    [SerializeField] public float lowJumpMultiplier;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isFacingRight = false;
    private bool isJumping;
    private bool isGrounded;
    private float horizontalInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        AnimationProcess();
    }

    void FixedUpdate()
    {
        MovementProcess();
        JumpingProcess();
    }

    void MovementProcess()
    {
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

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

    public void OnMove(InputValue value)
    {
        horizontalInput = value.Get<float>();
        Debug.Log(horizontalInput);
    }

    public void OnJump()
    {
        if (isGrounded) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX * Time.deltaTime, jumpSpeed);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }   

}