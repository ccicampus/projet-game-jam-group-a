using UnityEngine;

/// <summary>
/// Base class for all collectible items
/// </summary>
public abstract class Item : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] protected int scoreValue = 10;
    [SerializeField] protected bool destroyOnCollect = true;
    
    [Header("Audio")]
    [SerializeField] protected AudioClip collectSound;
    
    [Header("VFX")]
    [SerializeField] protected GameObject collectEffect;
    
    protected bool isCollected = false;
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }
    
    protected virtual void Collect(GameObject collector)
    {
        isCollected = true;
        
        // Add score
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        
        // Play sound
        if (collectSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(collectSound);
        }
        
        // Spawn VFX
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }
        
        // Apply item effect
        OnCollected(collector);
        
        // Destroy or disable
        if (destroyOnCollect)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    protected abstract void OnCollected(GameObject collector);
}
