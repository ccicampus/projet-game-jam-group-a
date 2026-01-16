using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base enemy class with health and damage system
/// Inherit from this for specific enemy types
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int maxHealth = 50;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int damageAmount = 10;
    [SerializeField] protected int scoreValue = 100;
    
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 3f;
    
    [Header("Events")]
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent OnDeath;
    
    [Header("Debug")]
    [SerializeField] protected bool debugMode = false;
    
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected bool isDead = false;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }
    
    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;
        
        currentHealth = Mathf.Max(0, currentHealth - damage);
        
        if (debugMode)
            Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        OnHealthChanged?.Invoke(currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnDamaged();
        }
    }
    
    protected virtual void OnDamaged()
    {
        // Override for hit reactions (flash, knockback, etc.)
    }
    
    protected virtual void Die()
    {
        if (isDead) return;
        
        isDead = true;
        
        if (debugMode)
            Debug.Log($"{gameObject.name} died");
        
        // Add score
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        
        OnDeath?.Invoke();
        
        // Disable physics
        if (rb != null)
        {
            rb.simulated = false;
        }
        
        // Play death animation or destroy
        Destroy(gameObject, 0.5f);
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
    
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsDead() => isDead;
}
