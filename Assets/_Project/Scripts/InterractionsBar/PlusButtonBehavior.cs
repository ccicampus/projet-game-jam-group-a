using UnityEngine;
using UnityEngine.UI;

public class PlusButtonBehavior : MonoBehaviour // TODO : implementation example, delete this in final version
{
    [SerializeField] private Button plusButton;

    [Header("Debug")]
    [SerializeField] protected bool debugMode = false;

    private void Awake()
    {
        this.plusButton.onClick.AddListener(() => {
                InteractBarManager
                    .Instance?
                    .Increase(); // Use this function to increase interaction bar
            }
        );
    }
}
