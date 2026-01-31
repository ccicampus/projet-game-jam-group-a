using UnityEngine;
using UnityEngine.UI;

public class MinusButtonBehavior : MonoBehaviour
{
    [SerializeField] private Button minusButton;

    [Header("Debug")]
    [SerializeField] protected bool debugMode = false;

    private void Awake()
    {
        this.minusButton.onClick.AddListener(() => {
                InteractBarManager
                    .Instance?
                    .Decrease();
            }
        );
    }
}
