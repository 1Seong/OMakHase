using UnityEngine;

public enum Personality { Picky, Normal, Generous }

[CreateAssetMenu(fileName = "CustomerData", menuName = "Scriptable Objects/CustomerData")]
public class CustomerData : RecipeBase
{
    [SerializeField]
    private Ingredient.Base _baseIngred;
    public Ingredient.Base baseIngred { get => _baseIngred; }

    [SerializeField]
    private Ingredient.Cook _cook;
    public Ingredient.Cook cook { get => _cook; }

    [SerializeField]
    private Ingredient.Main _mainIngredCategory;
    public Ingredient.Main mainIngredCategory { get => _mainIngredCategory; }

    [Header("person data")]

    [SerializeField]
    private Personality _personality;
    public Personality personality { get => _personality; }

    [SerializeField]
    private CustomerLineData _customerLineData;
    public CustomerLineData customerLineData { get => _customerLineData; }

    [SerializeField]
    private bool _isSpecial;
    public bool isSpecial { get => _isSpecial; }

    public Sprite sprite;

    public bool CheckCondition(RecipeData recipe)
    {
        if (categoryData != null)
            if (!categoryData.Equals(recipe.categoryData)) return false;

        if(meatfish != Ingredient.MeatFish.noCondition)
            if (meatfish != recipe.meatfish) return false;

        if(vege != Ingredient.Vege.noCondition)
            if(vege != recipe.vege) return false;

        if(baseIngred != Ingredient.Base.noCondition)
            if(baseIngred != recipe.categoryData.baseIngred) return false;

        if (cook != Ingredient.Cook.noCondition)
            if (cook != recipe.categoryData.cook) return false;

        if (mainIngredCategory != Ingredient.Main.noCondition)
            if (!Ingredient.IsSubCategory(recipe.meatfish, mainIngredCategory) && !Ingredient.IsSubCategory(recipe.vege, mainIngredCategory))
                return false;

        return true;
    }
}