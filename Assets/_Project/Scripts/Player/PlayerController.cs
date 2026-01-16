using UnityEngine;

/// <summary>
/// Main player controller handling movement, jumping, and input
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private bool isGrounded;
    private bool wasGrounded;
    private float horizontalInput;
    private float currentSpeed;
    private float jumpBufferCounter;
    private float coyoteTimeCounter;
    
    private int animSpeed;
    private int animGrounded;
    private int animJump;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (animator != null)
        {
            animSpeed = Animator.StringToHash("Speed");
            animGrounded = Animator.StringToHash("IsGrounded");
            animJump = Animator.StringToHash("Jump");
        }
        
        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.SetParent(transform);
            groundCheckObj.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = groundCheckObj.transform;
        }
    }
    
    private void Update()
    {
        HandleInput();
        CheckGround();
        HandleJumpBuffer();
        UpdateAnimations();
    }
    
    private void FixedUpdate()
    {
        Move();
    }
    
    private void HandleInput()
    {
        // Use modern Input System via InputManager
        horizontalInput = InputManager.Instance.MoveInput.x;

        if (InputManager.Instance.JumpPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
    
    private void Move()
    {
        float targetSpeed = horizontalInput * moveSpeed;
        float speedDifference = targetSpeed - currentSpeed;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        
        currentSpeed += speedDifference * accelRate * Time.fixedDeltaTime;
        
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
        
        if (horizontalInput != 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = horizontalInput < 0;
        }
    }
    
    private void HandleJumpBuffer()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            Jump();
            jumpBufferCounter = 0f;
        }
    }
    
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        
        if (animator != null)
        {
            animator.SetTrigger(animJump);
        }
    }
    
    private void CheckGround()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        if (!wasGrounded && isGrounded)
        {
            OnLand();
        }
    }
    
    private void OnLand()
    {
        Debug.Log("Player landed");
    }
    
    private void UpdateAnimations()
    {
        if (animator == null) return;
        
        animator.SetFloat(animSpeed, Mathf.Abs(currentSpeed));
        animator.SetBool(animGrounded, isGrounded);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
