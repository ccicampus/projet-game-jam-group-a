using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Permet la gestion de la focus sur les boutons du menu entre la souris et le clavier. 
/// </summary>
public class MenuButtonFocus : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}
