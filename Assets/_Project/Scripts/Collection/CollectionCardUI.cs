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
        if (maskImage != null && card.MaskSprite != null)
        {
            maskImage.sprite = card.MaskSprite;
        }

        if (scoreText != null)
        {
            scoreText.text = $"Score: {card.ScoreObtained}";
        }
    }
}