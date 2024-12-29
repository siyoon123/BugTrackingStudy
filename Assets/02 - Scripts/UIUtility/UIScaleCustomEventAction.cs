using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIScaleCustomEventAction : MonoBehaviour, IPointerClickHandler
{
    public Action EventAct;
    
    [SerializeField] UIScale ui;
    public void SetUI(UIScale ui)
    {
        this.ui = ui;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        EventAct?.Invoke();
        ui?.HideBack();
    }
}
