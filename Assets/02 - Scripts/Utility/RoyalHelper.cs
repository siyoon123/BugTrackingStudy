using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
#endif

public class RoyalHelper : MonoBehaviour
{
    private static RoyalHelper instance;
    public static RoyalHelper Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("RoyalHelper", typeof(RoyalHelper)).GetComponent<RoyalHelper>();
            }
            return instance;
        }
    }


    private Dictionary<string, Coroutine> coroutineDic = new Dictionary<string, Coroutine>();

    private Dictionary<string, int> customCountDic = new Dictionary<string, int>();


    public bool IsStarted(GameObject _obj, string _coroutineName)
    {
        string keyName = _obj.GetHashCode() + _coroutineName;
        return coroutineDic.ContainsKey(keyName);
    }
    
    public bool IsStarted(string coroutineName)
    {
        string keyName = coroutineName;
        return coroutineDic.ContainsKey(keyName);
    }

    IEnumerator WaitCoroutine(float _time, System.Action _callEvent, System.Action<float> _updateTimeEvent, float _delay, string _key = "")
    {
        if (_delay > 0)
            yield return new WaitForSeconds(_delay);
        float time = 0;
        if (_time > 0)
        {
            while (time <= 1.0f)
            {
                time += Time.deltaTime / _time;
                if (_updateTimeEvent != null)
                    _updateTimeEvent(time);
                yield return null;
            }
        }

        if (_callEvent != null)
            _callEvent.Invoke();
        if (string.IsNullOrEmpty(_key) == false && coroutineDic.ContainsKey(_key))
        {
            coroutineDic.Remove(_key);
        }

    }
    public Coroutine WaitCallEvent(float _time, System.Action _callEvent, System.Action<float> _updateTimeEvent = null, float _delay = 0)
    {
        Coroutine coroutine = StartCoroutine(WaitCoroutine(_time, _callEvent, _updateTimeEvent, _delay));
        return coroutine;
    }

    public Coroutine WaitCallEvent(string playerId, string _coroutineName, float _time, System.Action _callEvent, System.Action<float> _updateTimeEvent = null, float _delay = 0)
    {
        string keyName = playerId + _coroutineName;
        if (coroutineDic.ContainsKey(keyName))
        {
            if (coroutineDic[keyName] != null)
                StopCoroutine(coroutineDic[keyName]);
            coroutineDic.Remove(keyName);
        }

        Coroutine coroutine = StartCoroutine(WaitCoroutine(_time, _callEvent, _updateTimeEvent, _delay, keyName));
        coroutineDic.Add(keyName, coroutine);

        return coroutine;
    }

    public Coroutine WaitCallEvent(GameObject _obj, string _coroutineName, float _time, System.Action _callEvent, System.Action<float> _updateTimeEvent = null, float _delay = 0)
    {
        string keyName = _obj.GetHashCode() + _coroutineName;
        if (coroutineDic.ContainsKey(keyName))
        {
            if (coroutineDic[keyName] != null)
                StopCoroutine(coroutineDic[keyName]);
            coroutineDic.Remove(keyName);
        }

        Coroutine coroutine = StartCoroutine(WaitCoroutine(_time, _callEvent, _updateTimeEvent, _delay, keyName));
        coroutineDic.Add(keyName, coroutine);

        return coroutine;
    }
    public Coroutine WaitCallEvent(string _id, float _time, System.Action _callEvent, System.Action<float> _updateTimeEvent = null, float _delay = 0)
    {
        string keyName = _id;
        if (coroutineDic.ContainsKey(keyName))
        {
            if (coroutineDic[keyName] != null)
                StopCoroutine(coroutineDic[keyName]);
            coroutineDic.Remove(keyName);
        }

        Coroutine coroutine = StartCoroutine(WaitCoroutine(_time, _callEvent, _updateTimeEvent, _delay, keyName));
        coroutineDic.Add(keyName, coroutine);

        return coroutine;
    }

    IEnumerator WaitUnTimeScaleCoroutine(float _time, System.Action _callEvent, System.Action<float> _updateTimeEvent, float _delay, string _key = "")
    {
        if (_delay > 0)
            yield return new WaitForSecondsRealtime(_delay);
        float time = 0;
        while (time <= 1.0f)
        {
            time += Time.unscaledDeltaTime / _time;
            if (_updateTimeEvent != null)
                _updateTimeEvent(time);
            yield return null;
        }
        if (_callEvent != null)
            _callEvent.Invoke();
        if (string.IsNullOrEmpty(_key) == false && coroutineDic.ContainsKey(_key))
        {
            coroutineDic.Remove(_key);
        }
    }
    public Coroutine WaitUnTimeScaleCallEvent(float _time, System.Action _callEvent, System.Action<float> _updateTimeEvent = null, float _delay = 0)
    {
        Coroutine coroutine = StartCoroutine(WaitUnTimeScaleCoroutine(_time, _callEvent, _updateTimeEvent, _delay));
        return coroutine;
    }
    public Coroutine WaitUnTimeScaleCallEvent(GameObject _obj, string _coroutineName, float _time, System.Action _callEvent, System.Action<float> _updateTimeEvent = null, float _delay = 0)
    {
        string keyName = _obj.GetHashCode() + _coroutineName;
        if (coroutineDic.ContainsKey(keyName))
        {
            StopCoroutine(coroutineDic[keyName]);
            coroutineDic.Remove(keyName);
        }

        Coroutine coroutine = StartCoroutine(WaitUnTimeScaleCoroutine(_time, _callEvent, _updateTimeEvent, _delay, keyName));
        coroutineDic.Add(keyName, coroutine);

        return coroutine;
    }

    IEnumerator SecondTimerCoroutine(int _time, System.Action _completeEvent, System.Action _secondEvent, bool _isTimeScale = true, string _key = "")
    {
        float time = 0;
        while (_time > 0)
        {
            if (_isTimeScale)
                time += Time.deltaTime;
            else
                time += Time.unscaledDeltaTime;
            if (time >= 1.0f)
            {
                time = 0;
                _time--;
                if (_secondEvent != null)
                    _secondEvent.Invoke();
            }
            yield return null;
        }
        if (_completeEvent != null)
            _completeEvent.Invoke();
        if (string.IsNullOrEmpty(_key) == false && coroutineDic.ContainsKey(_key))
        {
            coroutineDic.Remove(_key);
        }
    }
    public Coroutine SecondCallEvent(string _callName, string _coroutineName, int _time, System.Action _callEvent, System.Action _secondEvent = null, bool _isTimeScale = true)
    {
        string keyName = _callName + _coroutineName;
        if (coroutineDic.ContainsKey(keyName))
        {
            if (coroutineDic[keyName] != null)
                StopCoroutine(coroutineDic[keyName]);
            coroutineDic.Remove(keyName);
        }

        Coroutine coroutine = StartCoroutine(SecondTimerCoroutine(_time, _callEvent, _secondEvent, _isTimeScale, keyName));
        coroutineDic.Add(keyName, coroutine);

        return coroutine;
    }

    public Coroutine MyStartCoroutine(GameObject _obj, string _coroutineName, IEnumerator proc)
    {
        string keyName = _obj.GetHashCode() + _coroutineName;
        if (coroutineDic.ContainsKey(keyName))
        {
            if (coroutineDic[keyName] != null)
                StopCoroutine(coroutineDic[keyName]);
            coroutineDic.Remove(keyName);
        }
        Coroutine coroutine = StartCoroutine(proc);
        coroutineDic.Add(keyName, coroutine);
        return coroutine;
    }
    public Coroutine MyStartCoroutine(string playerId, string _coroutineName, IEnumerator proc)
    {
        string keyName = playerId + _coroutineName;
        if (coroutineDic.ContainsKey(keyName))
        {
            if (coroutineDic[keyName] != null)
                StopCoroutine(coroutineDic[keyName]);
            coroutineDic.Remove(keyName);
        }
        Coroutine coroutine = StartCoroutine(proc);
        coroutineDic.Add(keyName, coroutine);
        return coroutine;
    }

    public void StopCoroutine(string playerId, string _coroutineName)
    {
        string keyName = playerId + _coroutineName;
        if (coroutineDic.ContainsKey(keyName))
        {
            if (coroutineDic[keyName] != null)
                StopCoroutine(coroutineDic[keyName]);
            coroutineDic.Remove(keyName);
        }
    }
    public void StopCoroutine(GameObject _obj, string _coroutineName)
    {
        string keyName = _obj.GetHashCode() + _coroutineName;
        if (coroutineDic.ContainsKey(keyName))
        {
            if (coroutineDic[keyName] != null)
                StopCoroutine(coroutineDic[keyName]);
            coroutineDic.Remove(keyName);
        }
    }

    public void StopCoroutine(string _coroutineName)
    {
        string keyName = _coroutineName;
        if (coroutineDic.ContainsKey(keyName))
        {
            if (coroutineDic[keyName] != null)
                StopCoroutine(coroutineDic[keyName]);
            coroutineDic.Remove(keyName);
        }
    }

    public void AllCancel()
    {
        StopAllCoroutines();
    }

    public static string GetTimeString(int _time, int _number = 4)
    {
        string timeString = new System.TimeSpan(0, 0, _time).ToString();
        return timeString = timeString.Remove(0, _number);
    }

    public static int GetRatioIndex(int[] _ratio)
    {
        float maxRatio = 0;
        for (int i = 0; i < _ratio.Length; i++)
        {
            maxRatio += _ratio[i];
        }
        float current = Random.Range(0, maxRatio);
        float currentRatio = 0;
        for (int i = 0; i < _ratio.Length; i++)
        {
            if (currentRatio <= current && current < currentRatio + _ratio[i])
            {
                return i;
            }
            currentRatio += _ratio[i];
        }
        return 0;
    }
    public static int GetRatioIndex(float[] _ratio)
    {
        float maxRatio = 0;
        for (int i = 0; i < _ratio.Length; i++)
        {
            maxRatio += _ratio[i];
        }

        float current = Random.Range(0, maxRatio);
        float currentRatio = 0;
        for (int i = 0; i < _ratio.Length; i++)
        {
            if (currentRatio <= current && current < currentRatio + _ratio[i])
            {
                return i;
            }
            currentRatio += _ratio[i];
        }
        return 0;
    }
    public static int GetRatioIndex(List<float> _ratio)
    {
        float maxRatio = 0;
        for (int i = 0; i < _ratio.Count; i++)
        {
            maxRatio += _ratio[i];
        }

        float current = Random.Range(0, maxRatio);
        float currentRatio = 0;
        for (int i = 0; i < _ratio.Count; i++)
        {
            if (currentRatio <= current && current < currentRatio + _ratio[i])
            {
                return i;
            }
            currentRatio += _ratio[i];
        }
        return 0;
    }



    public bool CheckCustomCount(string key, int maxCount, float timer)
    {
        if (!customCountDic.ContainsKey(key))
            customCountDic[key] = 0;

        customCountDic[key] += 1;

        if (customCountDic[key] >= maxCount)
        {
            customCountDic[key] = 0;
            return true;
        }

        WaitCallEvent(key + "coroutine", timer, () => { customCountDic[key] = 0; });

        return false;
    }

    public string MatchCountClearTimeToString(System.TimeSpan time)
    {
        string timeSpanFormat = "hh':'mm':'ss";
        return time.ToString(timeSpanFormat);
    }

    /* public void ShowChatLockTime(System.DateTime time)
    {
        LocalizationManager localizationManager = LocalizationManager.Instance;
        var currentTime = time - ServerTime.Now;
        string timeString = string.Empty;
        if ((int)currentTime.Days > 0)
        {
            timeString = string.Format(localizationManager.GetLocalizedValue("{0}일"), currentTime.Days);//Local추가
        }
        else if (currentTime.Hours > 0)
        {
            timeString = string.Format(localizationManager.GetLocalizedValue("{0}시간"), currentTime.Hours);
        }
        else if (currentTime.Minutes > 0)
        {
            timeString = string.Format(localizationManager.GetLocalizedValue("{0}분"), currentTime.Minutes);
        }
        else
        {
            timeString = string.Format(localizationManager.GetLocalizedValue("{0}초"), currentTime.Seconds);
        }
        //MessageBoxManager.Instance.Show(string.Format("채팅금지 {0} 남음", timeString));
    } */
    public void WaitForNextFrame(Action onComplete)
    {
        Instance.StartCoroutine(WaitForNextFrameCoroutine(onComplete));
    }

    private IEnumerator WaitForNextFrameCoroutine(Action onComplete)
    {
        yield return null;
        onComplete?.Invoke();
    }
}
