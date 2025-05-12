using UnityEngine;
using UnityEngine.UI;

public class UI_Test1 : UIPopup
{
    [SerializeField]
    private Button openButton;
    [SerializeField]
    private Button closePreButton;

    protected override void Awake()
    {
        base.Awake();
        SetBtn(openButton, OnClickOpen2);
        SetBtn(closePreButton, OnClickCloseTest);
    }

    private void OnClickOpen2()
    {
        AddressableUIManager.Instance.ShowUI(UIName.UI_Test2.ToString());
    }

    private void OnClickCloseTest()
    {
        AddressableUIManager.Instance.HideUI(UIName.UI_Test1.ToString());
    }
}
