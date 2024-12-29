using System.Dynamic;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using LeTai.Asset.TranslucentImage;

public class UIPoolManager : ManagerSingleton<UIPoolManager>
{
    [Header("PopUp GameObject")]
    [SerializeField] private RectTransform[] canvasUI;        

    [SerializeField] private Dictionary<string, GameObject> spwanDic = new Dictionary<string, GameObject>();
    [SerializeField] private Dictionary<int, int> spawnCanvasIndexDic = new Dictionary<int, int>();

    protected override void Awake()
    {
        base.Awake();

        spwanDic = new Dictionary<string, GameObject>();
    }

    public T GetPopUpPath<T>(string path, int index = 0, bool isFront = false, int isChangeCanvasIndex = -1)
    {
        string typeName = path + "/" + typeof(T).Name;

        if (!spwanDic.ContainsKey(typeName) || IsDestroyed(typeName))
        {
            //생성
            spwanDic[typeName] = PrefabGo(typeName, index, isFront);
            spawnCanvasIndexDic[spwanDic[typeName].GetHashCode()] = index;
        }
        if (isChangeCanvasIndex != -1 && isChangeCanvasIndex < canvasUI.Length)
        {

            ChangePopLayer(spwanDic[typeName], isChangeCanvasIndex, isFront);
        }
        /* else if(index != 1)
        {
            SetPopLayer(spwanDic[typeName], isFront);
        } */

        return spwanDic[typeName].GetComponent<T>();
    }
    public T GetPopUp<T>(int index = 0, bool isFront = false, int isChangeCanvasIndex = -1)
    {
        string typeName = typeof(T).Name;

        if (!spwanDic.ContainsKey(typeName) || IsDestroyed(typeName))
        {
            //생성
            spwanDic[typeName] = PrefabGo(typeName, index, isFront);
            spawnCanvasIndexDic[spwanDic[typeName].GetHashCode()] = index;
        }
        if (isChangeCanvasIndex != -1 && isChangeCanvasIndex < canvasUI.Length)
        {

            ChangePopLayer(spwanDic[typeName], isChangeCanvasIndex, isFront);
        }
        /* else if(index != 1)
        {
            SetPopLayer(spwanDic[typeName], isFront);
        } */
        

        return spwanDic[typeName].GetComponent<T>();
    }

    public T GetPopUp<T>(string name, int index = 0, bool isFront = false, int isChangeCanvasIndex = -1)
    {
        if (!spwanDic.ContainsKey(name) || IsDestroyed(name))
        {
            //생성
            spwanDic[name] = PrefabGo(name, index, isFront);
            spawnCanvasIndexDic[spwanDic[name].GetHashCode()] = index;
        }

        if (isChangeCanvasIndex != -1 && isChangeCanvasIndex < canvasUI.Length)
        {
            
            ChangePopLayer(spwanDic[name],isChangeCanvasIndex,isFront);
        }
        /* else if(index != 1)
        {
            SetPopLayer(spwanDic[name], isFront);
        } */

        return spwanDic[name].GetComponent<T>();
    }
    public bool IsDestroyed(string name)
    {
        return spwanDic.ContainsKey(name) && spwanDic[name] == null;
    }

    public bool IsSpwanPopUp(string name)
    {
        return spwanDic.ContainsKey(name);
    }

    //public GameObject PrefabGo(System.Type type)
    //{
    //    GameObject res = Resources.Load("UI/Lobby/" + type.Name) as GameObject;
    //    if (res == null)
    //    {
    //        Debug.LogError(type.Name + " Prefab is Null");
    //        return null;
    //    }
            
    //    GameObject go = Instantiate(res, canvasUI);
    //    return go;
    //}

    public GameObject PrefabGo(string name, int index, bool isFront)
    {
        Debug.Log("UI/Lobby/" + name);
        GameObject res = Resources.Load("UI/Lobby/" + name) as GameObject;
        if (res == null)
        {
            Debug.LogError(name + " Prefab is Null");
            return null;
        }

        GameObject go = Instantiate(res, canvasUI[index]);

        if (isFront == false)
        {
            go.transform.SetAsFirstSibling();
        }
        else
        {
            go.transform.SetAsLastSibling();
        }

        return go;
    }

    //public GameObject PrefabBundle(string name, int index, bool isFront)
    //{
    //    GameObject res = AssetBundleLoader.Instance.LoadGameObject($"UI/Lobby/{name}");
    //    if (res == null)
    //    {
    //        Debug.LogError(name + " Prefab is Null");
    //        return null;
    //    }

    //    GameObject go = Instantiate(res, canvasUI[index]);

    //    if (isFront == false)
    //    {
    //        go.transform.SetAsFirstSibling();
    //    }
    //    else
    //    {
    //        go.transform.SetAsLastSibling();
    //    }

    //    return go;
    //}

    public bool CheckOpenPopUp()
    {
        foreach(var popupObject in spwanDic.Values)
        {
            if(popupObject == null)
                continue;

            if (popupObject.activeSelf)
                return true;
        }

        return false;
    }

    public void AllClosePopupUI()
    {
        /* foreach(var popupObject in spwanDic.Values)
        {
            if(popupObject.activeSelf)
                popupObject.SetActive(false);
        } */
        foreach(var popupObject in spwanDic.Values)
        {
            if(popupObject == null)
                continue;
            UIBase baseUI = popupObject.GetComponent<UIBase>();
            if(popupObject.activeSelf)
            {
                if(baseUI != null && baseUI.IsShow)
                {
                    baseUI.IsShow = false;
                    baseUI.gameObject.SetActive(false);
                    RemovePoolOrder(popupObject);
                }
            }
        }
        BackButtonManager.Instance.AllBackButtonProc();
        BackButtonManager.Instance.Clear();
    }

    private void ChangePopLayer(GameObject obj, int index, bool isFront)
    {
        obj.transform.parent = canvasUI[index];
        if (isFront == false)
        {
            obj.transform.SetAsFirstSibling();
        }
        else
        {
            obj.transform.SetAsLastSibling();
        }
    }

    

    private List<GameObject> uiPoolQueueList = new List<GameObject>();
    [SerializeField] private Transform blurCanvas;

    public bool IsValidOrder(GameObject obj)
    {
        int hashCode = obj.GetHashCode();
        return spawnCanvasIndexDic.ContainsKey(hashCode) && (spawnCanvasIndexDic[hashCode] != 1 && spawnCanvasIndexDic[hashCode] != 3);
    }

    public void AddPoolOrder(GameObject obj)
    {
        if(IsValidOrder(obj) == false)
            return;
        // var blur = GetBlur(obj);
        // if(blur != null)
        // {
        //     for(int i=0; i<uiPoolQueueList.Count; i++)
        //     {
        //         SetOrderLayer(uiPoolQueueList[i], false);
        //     }
        //     SetOrderLayer(obj, true);
        // }
        // else
        // {
        //     SetOrderLayer(obj, true, false);
        // }
        //
        // //같은 오브젝트가 두번 add되지 않도록 처리 : Gacha
        // if (!uiPoolQueueList.Contains(obj)) uiPoolQueueList.Add(obj);
    }


    //이전것 블러 있으면 되돌림
    public void RemovePoolOrder(GameObject obj)
    {
        if(IsValidOrder(obj) == false)
            return;

        // var blur = GetBlur(obj);
        // if(blur != null)
        // {
        //     for(int i=0; i<uiPoolQueueList.Count; i++)
        //     {
        //         SetOrderLayer(uiPoolQueueList[i], false);
        //     }
        //     uiPoolQueueList.Remove(obj);
        //     if(uiPoolQueueList.Count > 0)
        //     {
        //         GameObject lastBlur = uiPoolQueueList[uiPoolQueueList.Count - 1];
        //         SetOrderLayer(lastBlur, true);
        //     }
        // }
        // else
        // {
        //     SetOrderLayer(obj, false, false);
        //     uiPoolQueueList.Remove(obj);
        // }

        
        
    }

    public void SetOrderLayer(GameObject obj, bool isActive, bool isBlur = true)
    {
        if(isActive)
        {
            obj.transform.SetParent(blurCanvas);
            //obj.transform.SetAsFirstSibling();
        }
        else
        {
            int canvasIndex = spawnCanvasIndexDic[obj.GetHashCode()];
            obj.transform.SetParent(canvasUI[canvasIndex]);
        }
        // if(isBlur)
        //     SetBlur(obj, isActive);
        
    }
    // public TranslucentImage GetBlur(GameObject obj)
    // {
    //     var blur = obj.GetComponentInChildren<TranslucentImage>();
    //     return blur;
    // }
    // public void SetBlur(GameObject obj, bool isActive)
    // {
    //     var blur = obj.GetComponentInChildren<TranslucentImage>();
    //     if(blur != null)
    //     {
    //         blur.enabled = isActive;
    //         /* if(isActive)
    //         {
    //             if(blur.source != null)
    //                 blur.source.maxUpdateRate = float.MaxValue;
    //             RoyalHelper.Instance.MyStartCoroutine(obj, "WaitBlur", WaitBlurSource(blur) );
    //         }
    //         else
    //         {
    //             if(blur.source != null)
    //                 blur.source.maxUpdateRate = 0;
    //         } */
    //     }
    // }

    // IEnumerator WaitBlurSource(TranslucentImage blurImage)
    // {
    //     yield return new WaitUntil( () => blurImage.source );
    //     blurImage.source.maxUpdateRate = float.MaxValue;
    //     yield return new WaitForSecondsRealtime(0.3f);
    //     blurImage.source.maxUpdateRate = 0;
    //
    // }

    [SerializeField] private Transform goodsCanvas;
    public Transform GetPopupGoodsCanvas()
    {
        return goodsCanvas;
    }

	public bool IsPopupAllClosed()
    {
        foreach (GameObject popupObject in spwanDic.Values)
        {
            if (null != popupObject)
            {
                UIScale uiScale = popupObject.GetComponent<UIScale>();

                if (null != uiScale)
                {
                    if (uiScale.IsShow)
                    {
                        return false;
                    }
                }
                else
                {
                    if (popupObject.activeSelf)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
    
    /* void OnDestroy()
    {
        foreach(var item in spwanDic.Keys.ToArray())
        {
            string key = $"UI/Lobby/{item}";
            //Resources.UnloadAsset(spwanDic[item]);
            AssetBundleLoader.Instance.Release(spwanDic[item]);
        }

    } */
}
