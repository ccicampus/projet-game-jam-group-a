using UnityEngine;

/// <summary>
/// Alternative movement script with different physics approach
/// Use this OR PlayerController, not both
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float airControl = 0.5f;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int maxJumps = 2;
    
    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpsRemaining;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, groundLayer);

        if (isGrounded)
        {
            jumpsRemaining = maxJumps;
        }

        // Use modern Input System via InputManager
        if (InputManager.Instance.JumpPressed && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsRemaining--;
        }
    }

    private void FixedUpdate()
    {
        // Use modern Input System via InputManager
        float moveInput = InputManager.Instance.MoveInput.x;
        float controlFactor = isGrounded ? 1f : airControl;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed * controlFactor, rb.linearVelocity.y);
    }
}
