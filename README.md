# Addressable UI Manager
Addressables로 UI관리하기
## 개요
`Addressable UI Manager`는 Unity의 Addressables 시스템을 활용하여 효율적으로 UI 리소스를 관리하도록 돕는 관리 도구입니다. Addressables는 Unity의 강력한 자산 관리 시스템으로, 리소스를 동적으로 로드하거나 메모리 관리를 최적화하는 데 사용됩니다. 이를 UI와 결합함으로써 프로젝트의 성능을 개선하고 유지보수를 단순화합니다.

---

## 주요 기능

1. **동적 UI 로드**:
   - Addressables를 사용하여 필요한 UI 리소스를 동적으로 로드합니다.
   - 불필요한 메모리 사용을 줄이고, UI 리소스를 필요할 때만 호출합니다.

2. **효율적인 메모리 관리**:
   - 사용하지 않는 UI 리소스를 해제하거나 정리하여 메모리 사용량을 최적화합니다.

3. **유지보수 향상**:
   - UI 리소스를 Addressables 시스템에 등록하여 프로젝트 구조와 관리가 간단해집니다.
   - UI 변경 및 업데이트가 쉬워집니다.

4. **유연한 확장성**:
   - Addressables의 태그와 그룹 기능을 활용해 다양한 UI 시나리오에 맞게 확장 가능합니다.

---

## 사용 예시
enum 타입인 UIName에 사용할 UI명을 등록한다.
이후 Addresable에 사용할 UI를 Group,Label을 맞게 등록한다.
`AddressableUIManager.Instance.ShowUI(UIName.UI_Test1.ToString());` 해당 방식으로 원하는 UI를 호출한다.

<details>
<summary>AddressableUIManager</summary>
  
`Addressable`을 이용하여 UI를 관리하는 매니저

`LoadPreloadUIKeysByLabel("UI");`를 작성하여 매개변수에 해당하는 Label에 관한 Addressable요소를 로드해서 가지고 있는다.

이후 사용자가 특정 UI를 호출 할 경우 사전에 로드한 데이터에 들어있을경우 해당 UI를 생성한다.
  
</details>



<details>
<summary>UIBase</summary>

`UIBase`는 `UIPopup`의 부모 클래스입니다.

1. **팝업의 열고 닫히는 기본 기능 제공**:
   - 팝업 UI의 활성화와 비활성화를 관리합니다.

2. **UI 구성 요소 기능 간략화**:
   - 버튼, 토글, 텍스트, 슬라이더, 이미지 등 주요 UI 구성 요소의 기능을 단순화하여 사용성을 높입니다.

</details>

<details>
<summary>UIPopup</summary>

`UIPopup`은 UI스크립트의 부모 클래스입니다.

1. **팝업의 크기,위치**:
   - 팝업 UI의 크기, 인덱스가 가장 위에 올 것인가 관리합니다.

</details>

<details>
<summary>UI_Test1</summary>

`UI_Test1`은 `UIPopup`을 상속받아 구현된 UI 클래스입니다.

### 주요 기능
1. **버튼 이벤트 설정**:
   - `Awake` 메서드에서 버튼을 초기화하고 클릭 이벤트를 설정합니다.

2. **UI 동적 호출**:
   - `OnClickOpen2` 메서드를 통해 `UI_Test2` UI를 Addressables 시스템을 사용하여 동적으로 호출합니다.

### 코드 예시
```c#
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
```
</details>

---

`Addressable UI Manager`는 Unity 프로젝트에서 UI 관리의 복잡성을 줄이고 성능을 높이는 데 큰 도움을 줍니다.

수정해야할 사항 : 현재는 주석쳐진 SetIcon에서 SpritePool에서 이미지를 뽑아오는 기능을 Addresalbe로 변환해야한다.

다음 사항 : 커스텀 UI 기능툴 만들어보기,  
참고 : https://dev-junwoo.tistory.com/147#comment20184515  
참고 : https://blog.treeplla.com/%ED%95%9C-%EB%8B%AC-%EC%95%88%EC%97%90-%EA%B2%8C%EC%9E%84-%EB%A7%8C%EB%93%A4%EA%B8%B0%EA%B0%80-%EA%B0%80%EB%8A%A5%ED%96%88%EB%8D%98-%EC%9D%B4%EC%9C%A0-672a20524fed  

다음 사항 :  젠킨스 자동빌드 만들어보기  
참고 : https://dev-junwoo.tistory.com/155  
