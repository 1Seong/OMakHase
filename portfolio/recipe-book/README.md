# 선택 상태에 따라 갱신되는 레시피북

**담당자:** 최성원 — 레시피북 선택 상태, 결과 표시와 재료 선택 UI 구현

## 코드

**BookController:** 선택한 조합을 조회하고 미등록·미발견·발견 상태를 구분해 표시합니다. **CookButton / MeatFishButton / VegeButton:** 선택값을 레시피북에 전달합니다. 중복되는 버튼 중 CookButton을 대표로 제시합니다.

```csharp
// BookController: 조리법이 바뀌면 결과 페이지를 갱신
private Ingredient.Cook _cook;
public Ingredient.Cook cook
{
    get => _cook;
    set
    {
        _cook = value;
        UpdateRightPage();
    }
}

// 버튼에서 받은 선택값을 상태에 반영
public void OnClick(Ingredient.Cook cook, Sprite sprite)
{
    this.cook = cook;
}

// 조합 존재 여부와 발견 여부를 구분해 레시피 상세 표시
public void UpdateRightPage()
{
    var data = RecipeManager.instance.GetRecipe(baseIngred, cook, meatFish, vege);
    // 등록되지 않은 조합은 페이지를 숨기고, 존재하지만 미발견인 요리와 구분한다.
    if (data == null)
    {
        RightPageUI.localScale = Vector3.zero;
        return;
    }

    RightPageUI.localScale = Vector3.one;
    rightPageTutorial.SetActive(false);
    // 아직 만들지 않은 메뉴는 정답 이미지 대신 잠금 이미지를 보여준다.
    bool unlocked = UnlockManager.instance.IsRecipeUnlocked(data.recipeName);
    image.sprite = unlocked ? data.BookSprite : lockSprite;
    image.SetNativeSize();
    title.text = unlocked ? data.recipeName : "아직 만들어 본 적 없는 메뉴입니다.";

    // 맛 수치 2점당 별 하나, 1점은 반쪽 별로 표시
    for (int i = 0; i < stars.Length; i++)
    {
        // 잠긴 요리는 별점도 숨겨 미발견 메뉴의 맛 정보가 드러나지 않게 한다.
        stars[i].gameObject.SetActive(unlocked);
        if (!unlocked) continue;

        int remainingTaste = data.taste - 2 * i;
        stars[i].sprite = remainingTaste switch
        {
            < 1 => emptyStar,
            1 => halfStar,
            _ => fullStar
        };
    }
}
```

```csharp
// CookButton: 조리법 버튼이 결과 조회·표시 로직을 직접 소유하지 않음
public class CookButton : MonoBehaviour, ButtonBehavior
{
    [SerializeField] private BookController controller;
    [SerializeField] private Ingredient.Cook cook;
    private Sprite sprite;

    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
    }

    // 다른 재료 버튼도 자신의 enum 값을 같은 방식으로 전달
    public void OnClick()
    {
        controller.OnClick(cook, sprite);
    }
}
```

## 생략 및 재구성한 내용

- 원본: `Assets/Scenes/Scripts/UI/`의 BookController·CookButton·MeatFishButton·VegeButton. 기준 커밋: `87c7ce2f89e529301e5f32d91fe2c6c5155ddaac`.
- 동일한 구조의 baseIngred·meatFish·vege 프로퍼티와 OnClick 오버로드는 생략하고 cook을 대표로 남겼습니다. 버튼은 각자 상태를 변경하는 대신 BookController에 선택을 전달합니다.
- UpdateRightPage의 잠금·발견 분기에 중복된 페이지 활성화와 별 숨김 기능을 합쳤습니다. 
- 첫 블록은 BookController 내부 발췌이며 Image·TMP·별 이미지·enum·ButtonBehavior·싱글턴 의존성 없이 단독 실행되지 않습니다.
