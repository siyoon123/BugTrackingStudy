using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
public class UIBase : SerializedMonoBehaviour {

    [SerializeField] private bool isActiveBackbutton = true;
    [SerializeField] private bool isUI = true;
    private System.Action showEvent;
    private System.Action hideEvent;

    protected bool isPossibleBack = false;

    protected bool isShow = false;
	public bool IsShow {get { return isShow;} set{isShow = value;}}

    public virtual void Show()
    {
        if(isUI == false)
            return;

        if (isActiveBackbutton == true)
            BackButtonManager.Instance.AddBackButtonUI(this.gameObject,Hide);

        this.gameObject.SetActive(true);

        if (showEvent != null)
            showEvent.Invoke();

        isPossibleBack = true;

        if(UIPoolManager.IsValid())
            UIPoolManager.Instance.AddPoolOrder(this.gameObject);
        
    }
    public virtual void Hide()
    {
        if(isUI == false)
            return;
            
        if (isActiveBackbutton == true)
            BackButtonManager.Instance.PopBackButton(this.gameObject);

        this.gameObject.SetActive(false);


        if (hideEvent != null)
            hideEvent.Invoke();
        
        isPossibleBack = true;

        RemovePoolOrder();
    }

    public virtual void RemovePoolOrder()
    {
        if(UIPoolManager.IsValid())
            UIPoolManager.Instance.RemovePoolOrder(this.gameObject);
    }

    public void ShowDepth(UIBase nextUI,System.Action hideEvent = null)
    {
        var oldEvent = GetHideEvent();
        SetHideEvent(null);
        Hide();
        nextUI.SetHideEvent(delegate
        {
            if(hideEvent != null)
                hideEvent.Invoke();
                
            SetHideEvent(oldEvent);
            Show();
        });
        nextUI.Show();
    }

    public void SetShowEvent(System.Action _showEvent)
    {
        showEvent = _showEvent;
    }
    public void SetHideEvent(System.Action _hideEvent)
    {
        hideEvent = _hideEvent;
    }
    public void CallShowEvent()
	{
		if(showEvent != null)
			showEvent.Invoke();
	}
    public void CallHideEvent()
	{
		if(hideEvent != null)
			hideEvent.Invoke();
	}

    public virtual bool IsPossibleBack()
    {
        return isPossibleBack;
    }

    public System.Action GetShowEvent()
	{
		return showEvent;
	}
    public System.Action GetHideEvent()
	{
		return hideEvent;
	}

    public void SetActive(bool _isShow)
	{
		isShow = _isShow;
	}

	public bool GetActive()
	{
		return isShow;
	}
    
    public virtual void SetAutoShow(System.Action completeEvent)
    {
        completeEvent.Invoke();
    }


    
}
