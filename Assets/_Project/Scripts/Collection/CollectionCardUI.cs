using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Displays a single visitor card in the collection UI
/// </summary>
public class CollectionCardUI : MonoBehaviour
{
    [SerializeField] private Image maskImage;
    [SerializeField] private TextMeshProUGUI scoreText;

    /// <summary>
    /// Set the card data to display
    /// </summary>
    public void SetCard(VisitorCard card)
    {
        if (maskImage != null)
        {
            if (card.MaskSprite != null)
            {
                maskImage.sprite = card.MaskSprite;
            }
            else
            {
                // If no sprite, set a random color for testing
                maskImage.color = GetRandomColor();
            }
        }

        if (scoreText != null)
        {
            scoreText.text = $"Score: {card.ScoreObtained}";
        }
    }

    /// <summary>
    /// Get a random color for testing (remove this when sprites are available)
    /// </summary>
    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}