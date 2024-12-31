using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleManager : MonoBehaviour
{
    [SerializeField] private UIScale example_Popup_1;

    public void OnClickShowExamplePopup1()
    {
        example_Popup_1.Show();
    }
}
