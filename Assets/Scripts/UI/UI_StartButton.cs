using UnityEngine;
using UnityEngine.UI;

public class UI_StartButton : UIPopup
{
    [SerializeField]
    private Button startButton;

    protected override void Awake()
    {
        base.Awake();
        SetBtn(startButton, OnClick_Start);
    }

    private void OnClick_Start()
    {
        AddressableUIManager.Instance.ShowUI(UIName.UI_Test1.ToString());
    }
}
