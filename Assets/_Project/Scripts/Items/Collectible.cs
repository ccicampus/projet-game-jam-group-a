using UnityEngine;

/// <summary>
/// Simple collectible item (coins, gems, etc.)
/// </summary>
public class Collectible : Item
{
    [Header("Collectible Type")]
    [SerializeField] private CollectibleType type = CollectibleType.Coin;
    
    public enum CollectibleType
    {
        Coin,
        Gem,
        Star,
        Key
    }
    
    protected override void OnCollected(GameObject collector)
    {
        // Additional logic based on type
        switch (type)
        {
            case CollectibleType.Coin:
                Debug.Log("Coin collected!");
                break;
            case CollectibleType.Gem:
                Debug.Log("Gem collected!");
                break;
            case CollectibleType.Star:
                Debug.Log("Star collected!");
                break;
            case CollectibleType.Key:
                Debug.Log("Key collected!");
                break;
        }
    }
}
