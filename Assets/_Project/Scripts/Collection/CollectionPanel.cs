using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Displays the collection of visited cards
/// </summary>
public class CollectionPanel : MonoBehaviour
{
    [SerializeField] private Transform cardContainer;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI totalCardsText;
    [SerializeField] private GameObject titlePanel;

    private void Start()
    {
        // Setup close button
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }

        // Listen for new cards
        if (CollectionManager.Instance != null)
        {
            CollectionManager.Instance.OnCardCollected.AddListener(OnNewCardCollected);
        }
    }

    private void OnEnable()
    {
        // Refresh display and hide title panel every time the panel is shown
        RefreshCollection();

        if (titlePanel != null)
        {
            titlePanel.SetActive(false);
        }
    }

    /// <summary>
    /// Refresh the displayed cards
    /// </summary>
    public void RefreshCollection()
    {
        if (CollectionManager.Instance == null)
            return;

        // Clear old cards from UI
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        // Create UI card for each collected card
        var cards = CollectionManager.Instance.GetAllCards();
        foreach (var card in cards)
        {
            CreateCardUI(card);
        }

        // Update total count
        if (totalCardsText != null)
        {
            totalCardsText.text = $"Total: {cards.Count}";
        }
    }

    /// <summary>
    /// Create a single card UI element
    /// </summary>
    private void CreateCardUI(VisitorCard card)
    {
        GameObject cardGO = Instantiate(cardPrefab, cardContainer);
        CollectionCardUI cardUI = cardGO.GetComponent<CollectionCardUI>();

        if (cardUI != null)
        {
            cardUI.SetCard(card);
        }
    }

    /// <summary>
    /// Called when a new card is collected
    /// </summary>
    private void OnNewCardCollected(VisitorCard card)
    {
        CreateCardUI(card);

        if (totalCardsText != null)
        {
            totalCardsText.text = $"Total: {CollectionManager.Instance.GetCardCount()}";
        }
    }

    public void ClosePanel()
    {
        // Show title panel again
        if (titlePanel != null)
        {
            titlePanel.SetActive(true);
        }

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(ClosePanel);
        }

        if (CollectionManager.Instance != null)
        {
            CollectionManager.Instance.OnCardCollected.RemoveListener(OnNewCardCollected);
        }
    }
}
