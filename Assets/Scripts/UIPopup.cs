using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : UIBase
{
    [Header("===닫기 버튼===")]
    public Button[] btnClose;
    public System.Action m_callBack;

    [Header("항상 최상단으로 셋팅할지 여부 체크")]
    public bool m_isSortLast = true;

    protected bool m_isClose = false;

    //하위 요소 삭제
    protected void DelectAllChild(Transform tr)
    {
        var Allchild = tr.GetComponentsInChildren<Transform>(true);
        for (int i = 1; i < Allchild.Length; i++)
        {
            Destroy(Allchild[i].gameObject);
        }
    }

    //닫기 버튼 이벤트 할당
    protected virtual void Awake()
    {
        for (int i = 0; i < btnClose.Length; ++i)
        {
            SetBtn(btnClose[i], OnClick_Close);
        }
    }

    //오픈 시 위치조정
    public override void Open()
    {
        base.Open();

        transform.localScale = Vector3.one;
        m_isClose = false;

        if (m_isSortLast)
            SetSortLast();

        SetStretch();
    }

    //m_isSortLast 값으로 위에올지 설정
    public void SetSortLast()
    {
        transform.SetAsLastSibling();
    }

    //크기조정
    public void SetStretch()
    {
        RectTransform _rect = gameObject.GetComponent<RectTransform>();
        _rect.sizeDelta = Vector2.zero;
    }

    //버튼 클릭 시 UI 닫기
    public virtual void OnClick_Close()
    {
        Close();
    }

    //닫기
    public override void Close()
    {
        base.Close();
        if (m_isClose == false)
        {
            m_callBack?.Invoke();
            m_callBack = null;
        }
        m_isClose = true;
    }
}
