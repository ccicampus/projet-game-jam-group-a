using UnityEngine;

/// <summary>
/// Represents a collected visitor card with sprite and score data
/// </summary>
[System.Serializable]
public class VisitorCard
{
    [SerializeField] private Sprite maskSprite;
    [SerializeField] private int scoreObtained;

    public Sprite MaskSprite => maskSprite;
    public int ScoreObtained => scoreObtained;

    public VisitorCard(Sprite sprite, int score)
    {
        maskSprite = sprite;
        scoreObtained = score;
    }
}