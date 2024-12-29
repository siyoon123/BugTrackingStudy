using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
        
        SRDebug.Init();
    }
}
