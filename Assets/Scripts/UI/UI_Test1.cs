using UnityEngine;
using UnityEngine.UI;

public class UI_Test1 : UIPopup
{
    [SerializeField]
    private Button openButton;

    protected override void Awake()
    {
        base.Awake();
        SetBtn(openButton, OnClickOpen2);
    }

    private void OnClickOpen2()
    {
        AddressableUIManager.Instance.ShowUI(UIName.UI_Test2.ToString());
    }
}
