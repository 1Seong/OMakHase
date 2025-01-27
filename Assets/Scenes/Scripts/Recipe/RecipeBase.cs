using UnityEngine;

[CreateAssetMenu(fileName = "RecipeBase", menuName = "Scriptable Objects/RecipeBase")]
public class RecipeBase : ScriptableObject
{
    [SerializeField]
    private CategoryData _categoryData;
    public CategoryData categoryData { get => _categoryData; }

    [SerializeField]
    private Ingredient.MeatFish _meatfish;
    public Ingredient.MeatFish meatfish { get => _meatfish; }

    [SerializeField]
    private Ingredient.Vege _vege;
    public Ingredient.Vege vege { get => _vege; }
}
