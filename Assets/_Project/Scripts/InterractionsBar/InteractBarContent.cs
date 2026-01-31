using UnityEngine;

public class InteractBarContent : MonoBehaviour
{
    private const int minNotches = 0;
    private const int maxNotches = 5;
    private int count;

    [Header("Debug")]
    [SerializeField] protected bool debugMode = false;

    private void Start()
    {
        this.count = 0;

        InteractBarManager
            .Instance?
            .OnInteract
            .AddListener((value) =>
                {
                    int checkedValue = this.count switch
                    {
                        <= minNotches => value == -1
                            ? 0
                            : 1,
                        >= maxNotches => value == 1
                            ? 0
                            : -1,
                        _ => value
                    };

                    this.count += checkedValue;
                    this.Render(this.count, maxNotches);
                }
            );
    }

    private void Render(int current, int max)
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (this.debugMode)
            Debug.Log(rectTransform.rect.yMax);

        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            360f * ((float)current / max) // TODO : récupérer 360 avec le code (parent?) pour être plus maintenable
        );
    }
}
