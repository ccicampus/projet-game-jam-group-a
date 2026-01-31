using UnityEngine;
using UnityEngine.UI;

public class InteractBarContent : MonoBehaviour
{
    private const int minNotches = 0;
    private const int maxNotches = 5;
    private int count;

    [SerializeField] private Image interactionBar;

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
        // TODO : currently no render, fix issue here
        this.interactionBar.fillAmount = (float)current / max;
    }
}
