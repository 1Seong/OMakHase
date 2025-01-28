using System;
using UnityEngine;
using static Ingredient;

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

    public void GetRandomIngredient()
    {
        _meatfish = GetRandomEnum<Ingredient.MeatFish>();
        _vege = GetRandomEnum<Ingredient.Vege>();
    }

    public void GetRandomCategory()
    {
        _categoryData = CustomerManager.instance.categoryPool[UnityEngine.Random.Range(0, CustomerManager.instance.categoryPool.Count)];
    }

    public void InitializeIngredient()
    {
        _meatfish = Ingredient.MeatFish.noCondition;
        _vege = Ingredient.Vege.noCondition;
    }

    public void InitializeCategory()
    {
        _categoryData = null;
    }

    // get values from enum
    T GetRandomEnum<T>()
    {

        Array enumValues = Enum.GetValues(typeof(T));


        int randomIndex = UnityEngine.Random.Range(0, enumValues.Length);


        return (T)enumValues.GetValue(randomIndex);
    }
}
