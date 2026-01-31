using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Manages collected visitor cards
/// </summary>
public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance { get; private set; }

    [SerializeField] private List<VisitorCard> collectedCards = new List<VisitorCard>();

    // Event triggered when a new card is collected
    public UnityEvent<VisitorCard> OnCardCollected = new UnityEvent<VisitorCard>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Add a new card to the collection
    /// </summary>
    public void CollectCard(Sprite maskSprite, int score)
    {
        VisitorCard newCard = new VisitorCard(maskSprite, score);
        collectedCards.Add(newCard);
        OnCardCollected?.Invoke(newCard);
    }

    /// <summary>
    /// Get all collected cards
    /// </summary>
    public List<VisitorCard> GetAllCards()
    {
        return new List<VisitorCard>(collectedCards);
    }

    /// <summary>
    /// Get total cards collected
    /// </summary>
    public int GetCardCount()
    {
        return collectedCards.Count;
    }

    /// <summary>
    /// Clear all collected cards (useful for testing)
    /// </summary>
    public void ClearCollection()
    {
        collectedCards.Clear();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            OnCardCollected?.RemoveAllListeners();
        }
    }
}
