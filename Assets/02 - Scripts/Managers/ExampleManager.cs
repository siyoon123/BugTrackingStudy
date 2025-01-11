using UnityEngine;
using UnityEngine.UI;

public class ExampleManager : MonoBehaviour
{
    [SerializeField] private UIScale example_Popup_1;
    [SerializeField] private UIScale example_Popup_2;
    [SerializeField] private UIScale example_Popup_3;
    [SerializeField] private Image example_Popup_3_Reddot;
    
    [SerializeField] private UIScale example_Popup_4;

    public void OnClickShowExamplePopup1()
    {
        example_Popup_1.Show();
    }
    
    public void OnClickShowExamplePopup2()
    {
        example_Popup_2.Show();
    }
    
    public void OnClickShowExamplePopup3()
    {
        example_Popup_3.Show();
        example_Popup_3_Reddot.gameObject.SetActive(false);
    }
    
    public void OnClickShowExamplePopup4()
    {
        example_Popup_4.Show();
    }
}
