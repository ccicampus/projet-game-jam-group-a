using UnityEngine;

/// <summary>
/// Serializable data container for visitor configuration
/// Follows same pattern as VisitorCard
/// </summary>
[System.Serializable]
public class VisitorData
{
    [Header("Identity")]
    [SerializeField] private string visitorName;
    [SerializeField] private Sprite maskSprite;
    [SerializeField] private Sprite unmaskSprite;
    [SerializeField] private Sprite bustSprite;
    [SerializeField] private Sprite bustSprite2;
    [SerializeField] private float bustAlternateSpeed = 0.5f;
    // #if UNITY_EDITOR
    [SerializeField] private RuntimeAnimatorController bustAnimation;
    // #endif
    [SerializeField] private VisitorType actualType;

    [Header("Scoring")]
    [SerializeField] private int baseScoreValue = 100;
    [SerializeField] private int difficultyLevel = 1;

    [Header("Dialogue")]
    [SerializeField] private TextAsset dialoguesJson;

    [Header("Clues (For Mini-games)")]
    [SerializeField] private string[] textClues;
    [SerializeField] private bool hasSuspiciousFeature;
    [SerializeField] private Color accentColor = Color.white;

    [Header("Audio")]
    [SerializeField] private AudioClip greetingSound;
    [SerializeField] private AudioClip revealSound;

    [Header("Combat (Optional)")]
    [SerializeField] private Sprite boinkSprite;
    [SerializeField] private AudioClip boinkSound;
    [SerializeField] private float boinkDuration = 0.3f;

    // Properties for controlled access
    public string VisitorName => visitorName;
    public Sprite MaskSprite => maskSprite;
    public Sprite UnmaskSprite => unmaskSprite;
    public Sprite BustSprite => bustSprite;
    public Sprite BustSprite2 => bustSprite2;
    public float BustAlternateSpeed => bustAlternateSpeed;
    // #if UNITY_EDITOR

    public RuntimeAnimatorController BustAnimation => bustAnimation;
    // #endif
    public VisitorType ActualType => actualType;
    public int BaseScoreValue => baseScoreValue;
    public TextAsset DialoguesJson => dialoguesJson;
    public int DifficultyLevel => difficultyLevel;
    public string[] TextClues => textClues;
    public bool HasSuspiciousFeature => hasSuspiciousFeature;
    public Color AccentColor => accentColor;
    public AudioClip GreetingSound => greetingSound;
    public AudioClip RevealSound => revealSound;
    public Sprite BoinkSprite => boinkSprite;
    public AudioClip BoinkSound => boinkSound;
    public float BoinkDuration => boinkDuration;
}
