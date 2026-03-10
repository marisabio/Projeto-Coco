using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] public float jumpSpeed;
    [SerializeField] public float fallMultiplier;
    [SerializeField] public float lowJumpMultiplier;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isFacingRight = true;
    private bool isJumping;
    private bool isGrounded;
    private float horizontalInput;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

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
        
    }

    void FixedUpdate()
    {
        MovementProcess();
        JumpingProcess();
    }

    void MovementProcess()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocityY);

        if (isFacingRight && horizontalInput < 0)
        {
            isFacingRight = false;
            spriteRenderer.flipX = isFacingRight;
        }
        else if (!isFacingRight && horizontalInput > 0)
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

    public void OnMove(InputValue value)
    {
        horizontalInput = value.Get<float>();
        Debug.Log(horizontalInput);
    }

    public void OnJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpSpeed);
    }   

}