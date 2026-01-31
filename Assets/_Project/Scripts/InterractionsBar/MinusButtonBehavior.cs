using UnityEngine;
using UnityEngine.UI;

public class MinusButtonBehavior : MonoBehaviour // TODO : implementation example, delete this in final version
{
    [SerializeField] private Button minusButton;

    [Header("Debug")]
    [SerializeField] protected bool debugMode = false;

    private void Awake()
    {
        this.minusButton.onClick.AddListener(() => {
                InteractBarManager
                    .Instance?
                    .Decrease(); // Use this function to decrease interaction bar
            }
        );
    }
}
