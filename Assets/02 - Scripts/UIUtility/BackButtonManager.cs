using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackButtonManager : MonoBehaviour {

    public static BackButtonManager Instance;

    private Stack<GameObject> backButtonUIStack = new Stack<GameObject>();
    private Dictionary<GameObject, System.Action> backButtonUIDic = new Dictionary<GameObject, System.Action>();

    //private bool isBack = false;

    public bool IsBack {get;set;}
    public bool IsLoading {get;set;}
    
    private bool isTutorial = false;
    public bool IsTutorial
    {
        get
        {
            return isTutorial;
        }
        set
        {
            isTutorial = value;
            // if (isTutorial)
            //     UIManager.Instance.LobbyScene.RemoveStartPopUpCoroutine();
        }
    }
    public bool IsAttendance {get;set;}
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            IsBack = true;
            IsLoading = false;
            IsTutorial = false;
            IsAttendance = false;
            DontDestroyOnLoad(Instance.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public int GetBackCount()
    {
        return backButtonUIStack.Count;
    }

    public void Clear()
    {
        backButtonUIDic.Clear();
        backButtonUIStack.Clear();
    }


    public void AddBackButtonUI(GameObject _backObj,System.Action _hideEvent)
    {
        if (backButtonUIStack.Contains(_backObj) == false)
        {
            backButtonUIStack.Push(_backObj);
            if(backButtonUIDic.ContainsKey(_backObj) == false)
            {
                backButtonUIDic.Add(_backObj, _hideEvent);
            }            
        }        
    }

    // Update is called once per frame
    void Update ()
    {
        if(IsTutorial == true)
            return;

        if(IsBack == false)
            return;
        if(IsLoading)
            return;
        if (IsAttendance)
            return;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            BackButtonProc(true);
        }
		
	}

    /// <summary>
    /// 이벤트 실행은안하고 해당 이벤트 값만 삭제하는 함수
    /// </summary>
    public void PopBackButton(GameObject obj)
    {
        if (obj == null) { return; }

        if (backButtonUIStack.Count <= 0)
        {
            return;            
        }

        GameObject currentObj = backButtonUIStack.Pop();

        if (currentObj == null)
            return;

        if (obj != null && obj == currentObj)
        {
            UIBase currentUI = currentObj.GetComponent<UIBase>();
            if (currentUI != null)
            {
                if (backButtonUIDic.ContainsKey(currentObj))
                {
                    backButtonUIDic.Remove(currentObj);
                }
            }
        }
        else{
                backButtonUIStack.Push(currentObj);
        }
    }

    public void BackButtonProc(bool isBackButton = false)
    {
         while (true)
         {
            if (backButtonUIStack.Count <= 0)
            {
                if (isBackButton == true)
                {
                    BackSceneEvent();
                }
                break;
            }
            GameObject currentObj = backButtonUIStack.Pop();
            Debug.Log(currentObj);
            if(currentObj == null)
                continue;
            UIBase currentUI = currentObj.GetComponent<UIBase>();
            if (currentUI != null)
            {
                if (currentUI.IsPossibleBack() == false)
                {
                    backButtonUIStack.Push(currentObj);
                    break;
                }
             }
             if(backButtonUIDic.ContainsKey(currentObj))
            {
                if(currentObj.activeSelf == false)
                {
                    backButtonUIDic.Remove(currentObj);
                    continue;
                }
                backButtonUIDic[currentObj].Invoke();
                backButtonUIDic.Remove(currentObj);
             }
            break;
        }
    }
    public void AllBackButtonProc()
    {
        while (true)
        {
            if (backButtonUIStack.Count <= 0)
            {
                break;
            }
            GameObject currentObj = backButtonUIStack.Pop();
            if(currentObj == null)
                continue;
            if(backButtonUIDic.ContainsKey(currentObj))
            {
                if(currentObj.activeSelf == false)
                {
                    backButtonUIDic.Remove(currentObj);
                    continue;
                }
                backButtonUIDic[currentObj].Invoke();
                backButtonUIDic.Remove(currentObj);
            }
        }
    }

    public bool IsExitBack()
    {
        return backButtonUIStack.Count <= 0;
    }

    public void BackSceneEvent()
    {
        // switch(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
        // {
        //     case 1:
        //     case 2:
        //         MessageBoxManager.Instance.Show(LocalizationManager.Instance.GetLocalizedValue("game_message_gamequit"),delegate
        //         {
        //             Application.Quit();
        //         },null,true);
        //         MessageBoxManager.Instance.MessageBoxUI.SetYesText("event_finish");
        //         break;
        //     default: 
        //         
        //         break;
        // }
    }
}
