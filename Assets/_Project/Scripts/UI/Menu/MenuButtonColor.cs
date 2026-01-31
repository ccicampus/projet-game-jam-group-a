using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MenuButtonColor : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Color normalColor = Color.white;

    private Color highlightColor;

    private void Awake()
    {
        if (buttonText == null)
            buttonText = GetComponentInChildren<TextMeshProUGUI>();

        ColorUtility.TryParseHtmlString("#B84515", out highlightColor);

        ResetColor();
    }

    public void OnPointerEnter(PointerEventData eventData) => SetHighlight();
    public void OnSelect(BaseEventData eventData) => SetHighlight();
    public void OnPointerExit(PointerEventData eventData) => ResetColor();
    public void OnDeselect(BaseEventData eventData) => ResetColor();

    private void SetHighlight()
    {
        if (buttonText != null)
            buttonText.color = highlightColor;
    }

    public void ResetColor()
    {
        if (buttonText != null)
            buttonText.color = normalColor;
    }
}