using UnityEngine;
using UnityEngine.EventSystems;
public class UIScaleCustomEvent : MonoBehaviour, IPointerClickHandler
{   
    [SerializeField] UIScale ui;
    
    public void SetUI(UIScale ui)
    {
        this.ui = ui;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ui?.HideBack();
    }
}
