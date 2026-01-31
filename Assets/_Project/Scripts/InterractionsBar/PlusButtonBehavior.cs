using UnityEngine;
using UnityEngine.UI;

public class PlusButtonBehavior : MonoBehaviour
{
    [SerializeField] private Button plusButton;

    [Header("Debug")]
    [SerializeField] protected bool debugMode = false;

    private void Awake()
    {
        this.plusButton.onClick.AddListener(() => {
                InteractBarManager
                    .Instance?
                    .Increase();
            }
        );
    }
}
