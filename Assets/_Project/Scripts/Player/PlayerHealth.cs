using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages player health, damage, and death
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    
    [Header("Invincibility")]
    [SerializeField] private float invincibilityDuration = 1f;
    private float invincibilityTimer;
    private bool isInvincible;
    
    [Header("Events")]
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent OnDeath;
    public UnityEvent OnDamageTaken;
    
    [Header("Debug")]
    [SerializeField] private bool debugMode = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHealth <= 0)
            return;
        
        currentHealth = Mathf.Max(0, currentHealth - damage);
        
        if (debugMode)
            Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        OnHealthChanged?.Invoke(currentHealth);
        OnDamageTaken?.Invoke();
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartInvincibility();
        }
    }
    
    public void Heal(int amount)
    {
        if (currentHealth >= maxHealth)
            return;
        
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        
        if (debugMode)
            Debug.Log($"Player healed {amount}. Health: {currentHealth}/{maxHealth}");
        
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    private void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }
    
    private void Die()
    {
        if (debugMode)
            Debug.Log("Player died");
        
        OnDeath?.Invoke();
    }
    
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public float GetHealthPercentage() => (float)currentHealth / maxHealth;
    public bool IsInvincible() => isInvincible;
}
