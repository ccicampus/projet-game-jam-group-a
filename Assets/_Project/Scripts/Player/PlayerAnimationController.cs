using UnityEngine;

/// <summary>
/// Handles player animation state management
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [Header("Animation Settings")]
    [SerializeField] private float minSpeedForRunAnim = 0.1f;
    
    private Animator animator;
    
    // Cached animator parameter IDs
    private int animIsMoving;
    private int animIsGrounded;
    private int animYVelocity;
    private int animAttack;
    private int animHurt;
    private int animDeath;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Cache parameter IDs for better performance
        animIsMoving = Animator.StringToHash("IsMoving");
        animIsGrounded = Animator.StringToHash("IsGrounded");
        animYVelocity = Animator.StringToHash("YVelocity");
        animAttack = Animator.StringToHash("Attack");
        animHurt = Animator.StringToHash("Hurt");
        animDeath = Animator.StringToHash("Death");
    }
    
    private void Update()
    {
        if (animator == null || rb == null) return;
        
        UpdateMovementAnimation();
        UpdateJumpAnimation();
    }
    
    private void UpdateMovementAnimation()
    {
        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > minSpeedForRunAnim;
        animator.SetBool(animIsMoving, isMoving);
    }
    
    private void UpdateJumpAnimation()
    {
        animator.SetFloat(animYVelocity, rb.linearVelocity.y);
    }
    
    public void SetGrounded(bool grounded)
    {
        animator.SetBool(animIsGrounded, grounded);
    }
    
    public void TriggerAttack()
    {
        animator.SetTrigger(animAttack);
    }
    
    public void TriggerHurt()
    {
        animator.SetTrigger(animHurt);
    }
    
    public void TriggerDeath()
    {
        animator.SetTrigger(animDeath);
    }
    
    public void FlipSprite(bool flipX)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = flipX;
        }
    }
}
