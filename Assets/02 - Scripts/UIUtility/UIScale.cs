using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Sirenix.OdinInspector;
using static UnityEngine.UI.Selectable;
// using LeTai.Asset.TranslucentImage;

public class UIScale : UIBase {

	[SerializeField] protected Transform panel;
	[SerializeField] private Image backPanel;
	[SerializeField] private CanvasGroup backGroup;
	[SerializeField] protected AnimationCurve animationShowCurve;
	[SerializeField] protected AnimationCurve animationHideCurve;

	[SerializeField] protected float animSpeed = 0.3f;

	[SerializeField] private bool isPause = true;
	
	[HideInInspector][SerializeField]
	protected List<ScrollRect> cacheScrollList = new List<ScrollRect>();
	

	private System.Action completeEvent;
	/* private System.Action showNowEvent;
	private System.Action hideNowEvent; */

	private Vector3 startSize;
	private Vector3 endSize;

	private Color startBackColor;
    
    public bool IsRunning = false;
	public bool ChangeGoodsDepth = true;
	
	private Coroutine animCoroutine;

    public Transform Panel => panel;
    
	public override void Show()
	{
		if(isShow)
			return;
		isShow = true;
		StopAllCoroutines();
		if(panel == null)
			panel = this.transform;
		if(backPanel != null)
		{
			backPanel.gameObject.SetActive(true);
			backPanel.raycastTarget = true;
		}

		if(startSize.magnitude <= 0)
		{
			startSize = panel.transform.localScale;
			if(backPanel != null)
			{
				
				startBackColor = backPanel.color;
			}
		}
		if(backPanel != null)
		{
			backPanel.color = startBackColor;
			if(backGroup != null)
				backGroup.alpha = startBackColor.a;
		}
		foreach(var item in cacheScrollList)
			item.enabled = true;
		base.Show();
		
		if(animCoroutine != null)
			StopCoroutine(animCoroutine);
		animCoroutine = StartCoroutine(AnimCoroutine(true));		
	}
	public void Reset()
	{
		isShow = false;
		/* if(animCoroutine != null)
			StopCoroutine(animCoroutine); */
		//animCoroutine = null;
	}
	/* private void OnDisable() {
		if(animCoroutine != null)
			StopCoroutine(animCoroutine);	
		animCoroutine = null;
	} */
	public override void Hide()
	{
		if(isShow == false)
			return;
		isShow = false;
		if(panel == null)
			panel = this.transform;
		if(backPanel != null)
		{
			//backPanel.raycastTarget = false;
		}
		foreach(var item in cacheScrollList)
			item.enabled = false;
		if(animCoroutine != null)
			StopCoroutine(animCoroutine);
		animCoroutine = StartCoroutine(AnimCoroutine(false));

        if (BackButtonManager.Instance != null) { BackButtonManager.Instance.PopBackButton(this.gameObject); }

		/* if(UIPoolManager.IsValid())
            UIPoolManager.Instance.RemovePoolOrder(this.gameObject); */
	}
	public void BaseHide()
	{
		base.Hide();
	}
	/* public override void RemovePoolOrder()
	{

	} */

	public void Show(System.Action _completeEvent)
	{
		completeEvent = _completeEvent;
		Show();
	}
	public void Hide(System.Action _completeEvent)
	{
		completeEvent = _completeEvent;
		Hide();
	}

	/* public void ShowNowEvent(System.Action showNowEvent)
	{
		this.showNowEvent = showNowEvent;
	}
	public void HideNowEvent(System.Action showNowEvent)
	{
		this.hideNowEvent = hideNowEvent;
	} */

	public void SetTargetScale(float _start,float _end)
	{
		startSize = RoyalValue.Vector3One * _start;
		endSize = RoyalValue.Vector3One * _end;
	}
	IEnumerator AnimCoroutine(bool _isShow)
	{
		float time = 0;

		Vector3 startScale = _isShow ? endSize : startSize;
		Vector3 endScale = _isShow ? startSize : endSize;

		AnimationCurve curve = _isShow ? animationShowCurve : animationHideCurve;
		WaitForFixedUpdate wait = new WaitForFixedUpdate();

        IsRunning = true;

        var backPanelAlphaColor = RoyalValue.ColorWhiteClear;
        if(backGroup == null && backPanel != null)
            backPanelAlphaColor = new Color(backPanel.color.r,backPanel.color.g,backPanel.color.b,0.0f);
        while (time <= 1.0f)
		{
			time += Time.unscaledDeltaTime / animSpeed;
			
			panel.transform.localScale = Vector3.LerpUnclamped(startScale,endScale,curve.Evaluate(time));
			if(backPanel != null)
			{
				if(_isShow == false)
				{
					//backGroup.alpha = Mathf.Lerp(startBackColor.a,0.0f,curve.Evaluate(time));
					
					if(backGroup == null && backPanel != null)
                        backPanel.color = Color.Lerp(startBackColor, backPanelAlphaColor, curve.Evaluate(time));
                }
			}
			if(isPause)
				yield return null;
			else
				yield return wait;
		}

		if(_isShow == false)
		{
			// if (null != UIManager.Instance && null != UIManager.Instance.LobbyUserPanel && ChangeGoodsDepth)
			// 	UIManager.Instance.LobbyUserPanel.ChangeGoodsDepth(false);

			base.Hide();
			if(backPanel != null)
			{
				backPanel.gameObject.SetActive(false);
			}
		}

		if(completeEvent != null)
			completeEvent();

        IsRunning = false;
	}
	public void SetCompleteEvent(System.Action _completeEvent)
	{
		completeEvent = _completeEvent;
	}

	public void SetActive(bool _isShow)
	{
		isShow = _isShow;
	}
	public void SetPanel(Transform _target)
	{
		panel = _target;
	}

	public void SetBackPanel(Image backPanel)
	{
        if (backPanel == null)
            return;

        this.backPanel.gameObject.SetActive(false);
        this.backPanel = backPanel;		
        this.backPanel.gameObject.SetActive(true);
    }
	
	public bool GetActive()
	{
		return isShow;
	}
	
	

	public override bool IsPossibleBack()
	{
		return !IsRunning;
	}

	public virtual void HideBack()
    {

		if(BackButtonManager.Instance.IsBack == true && IsRunning == false && BackButtonManager.Instance.IsExitBack() == false )
			BackButtonManager.Instance.BackButtonProc();
    }
	public void HideNow()
	{
		IsRunning = false;
		SetActive(false);
		base.Hide();
	}
	
#if UNITY_EDITOR
	[Button]
	public void CacheScroll()
	{
		cacheScrollList.Clear();
	
        foreach (var go in Resources.FindObjectsOfTypeAll(typeof(ScrollRect)) as ScrollRect[])
        {
            if (UnityEditor.EditorUtility.IsPersistent(go.transform.root.gameObject))
                continue;
            bool isCheck = false;
            var parent = go.transform.parent;
            while(parent != null)
            {
                if(parent == this.transform)
                {
                    isCheck = true;
                    break;
                }
                parent = parent.parent;
            }

            if(isCheck && go != null)
            {
                cacheScrollList.Add(go);
            }
        }
	}

	[Button]
	public void HideEventToBackPanel()
	{
		if(backPanel == null) return;
		var button = backPanel.GetComponent<Button>();
		if(button != null)
		{
			DestroyImmediate(button);
		}
		if(backPanel.GetComponent<UIScaleCustomEvent>() == null)
		{
			backPanel.gameObject.AddComponent<UIScaleCustomEvent>();
		}
		backPanel.GetComponent<UIScaleCustomEvent>().SetUI(this);
		
	}
	#endif

	[Button]
	public void ShowButton()
	{
		if(Application.isPlaying)
			Show();
	}

	[Button]
	public void SettingBlur()
	{
		// backPanel = this.transform.Find("Blur").GetComponent<TranslucentImage>();
		backGroup = this.transform.Find("Blur").GetComponent<CanvasGroup>();
		backPanel.GetComponent<UIScaleCustomEvent>().SetUI(this);
	}

}
