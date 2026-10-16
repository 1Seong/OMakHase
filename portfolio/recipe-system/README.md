# 레시피 데이터와 조합 조회

**담당자:** 최성원 — 레시피 데이터 구조와 조합 조회 구현

## 코드

**CategoryData / RecipeBase / RecipeData:** 조리 분류와 레시피 정보를 에셋으로 분리합니다. **Ingredient:** 재료·조리법의 식별값을 정의합니다. **RecipeManager:** 네 가지 선택값으로 레시피를 조회합니다.

```csharp
// 데이터 구조 발췌: Inspector에서 조합과 결과를 편집하는 에셋
public class CategoryData : ScriptableObject
{
    public Ingredient.Base baseIngred;
    public Ingredient.Cook cook;
}

public class RecipeBase : ScriptableObject
{
    public CategoryData categoryData;
    public Ingredient.MeatFish meatfish;
    public Ingredient.Vege vege;
}

// 조합 조건은 공통 기반에 두고, 완성 요리의 이름·맛·이미지만 확장한다.
public class RecipeData : RecipeBase
{
    public string recipeName;
    public string desc;
    public int taste;
    public Sprite BookSprite;
}
```

```csharp
// RecipeManager: 에디터에서 입력한 분류별 목록을 실행용 인덱스로 변환
[Serializable]
public struct CategoryAndRecipes
{
    public CategoryData categoryData;
    public List<RecipeData> recipeDatas;
}

[SerializeField] private List<CategoryAndRecipes> categoryListDatas;
private readonly Dictionary<
    (Ingredient.Base, Ingredient.Cook, Ingredient.MeatFish, Ingredient.Vege),
    RecipeData> recipeDictionary = new();

// 시작 시 한 번 구축하므로 요리·레시피북에서 목록을 다시 순회하지 않음
private void Start()
{
    foreach (var group in categoryListDatas)
    {
        foreach (var recipe in group.recipeDatas)
        {
            // 기본 재료·조리법·육류/생선·채소 네 값이 모두 같아야 같은 조합이다.
            var key = (group.categoryData.baseIngred, group.categoryData.cook,
                recipe.meatfish, recipe.vege);
            recipeDictionary[key] = recipe;
        }
    }
}

// 등록되지 않은 조합은 null을 반환
public RecipeData GetRecipe(Ingredient.Base baseIngred, Ingredient.Cook cook,
    Ingredient.MeatFish meatfish, Ingredient.Vege vege)
{
    // 인덱스 생성 때와 같은 순서로 키를 구성해 요리 화면과 레시피북이 공유한다.
    return recipeDictionary.TryGetValue((baseIngred, cook, meatfish, vege),
        out var recipe) ? recipe : null;
}
```

## 생략 및 재구성한 내용

- 원본: `Assets/Scenes/Scripts/Recipe/`의 CategoryData·Ingredient·RecipeBase·RecipeData·RecipeManager. 기준 커밋: `87c7ce2f89e529301e5f32d91fe2c6c5155ddaac`.
- 데이터 클래스는 구조를 보여주기 위해 private SerializeField와 읽기 전용 프로퍼티를 public 필드로 축약했습니다. CreateAssetMenu, 무작위 주문 생성, 재료 분류 매핑은 생략했습니다.
- 싱글턴·using·Unity 에셋은 원본 의존성으로, 블록들은 서로 다른 클래스의 발췌이며 단독 실행되지 않습니다.
