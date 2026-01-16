using UnityEngine;

/// <summary>
/// Power-up items that provide temporary or permanent benefits
/// </summary>
public class PowerUp : Item
{
    [Header("Power-Up Settings")]
    [SerializeField] private PowerUpType type = PowerUpType.Health;
    [SerializeField] private int amount = 20;
    [SerializeField] private float duration = 0f; // 0 = permanent
    
    public enum PowerUpType
    {
        Health,
        Speed,
        Jump,
        Invincibility,
        DoubleJump
    }
    
    protected override void OnCollected(GameObject collector)
    {
        switch (type)
        {
            case PowerUpType.Health:
                ApplyHealthPowerUp(collector);
                break;
            case PowerUpType.Speed:
                ApplySpeedPowerUp(collector);
                break;
            case PowerUpType.Jump:
                ApplyJumpPowerUp(collector);
                break;
            case PowerUpType.Invincibility:
                ApplyInvincibilityPowerUp(collector);
                break;
        }
    }
    
    private void ApplyHealthPowerUp(GameObject collector)
    {
        PlayerHealth health = collector.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.Heal(amount);
            Debug.Log($"Healed {amount} health");
        }
    }
    
    private void ApplySpeedPowerUp(GameObject collector)
    {
        // Implement speed boost
        Debug.Log("Speed boost applied");
    }
    
    private void ApplyJumpPowerUp(GameObject collector)
    {
        // Implement jump boost
        Debug.Log("Jump boost applied");
    }
    
    private void ApplyInvincibilityPowerUp(GameObject collector)
    {
        // Implement invincibility
        Debug.Log("Invincibility applied");
    }
}
