using System;
using UnityEngine;
using static Ingredient;

public enum Personality { Picky, Normal, Generous, Strict }

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

    public void InitializeOrder()
    {
        InitializeCategory();
        InitializeIngredient();
        _baseIngred = Ingredient.Base.noCondition;
        _cook = Ingredient.Cook.noCondition;
        _mainIngredCategory = Ingredient.Main.noCondition;
    }

    public void GetOrder() 
    {

        if (CustomerManager.instance.currentPersonality == Personality.Picky)
        {
            PickyOrder();
        }
        else if (CustomerManager.instance.currentPersonality == Personality.Normal)
        {
            NormalOrder();
        }
        else if (CustomerManager.instance.currentPersonality == Personality.Generous)
        {
            GenerousOrder();
        }
        else 
        {
            StrictOrder();
        }

    }

    public void PickyOrder() 
    {
        int randomIndex = UnityEngine.Random.Range(0, 2);

        // 카테고리 하나 특정: 예:“볶음밥을 만들어 주세요.”
        if (randomIndex == 0)
        {
            do
            {
                // Determine from basic category pool
                GetRandomCategory();
                _baseIngred = categoryData.baseIngred;
                _cook = categoryData.cook;
            } while (categoryData == null );

        }

        // 카테고리 범위 지정: “밥을 사용한 요리가 먹고 싶어요.”
        else if (randomIndex == 1)
        {
            do
            {
                _baseIngred = GetRandomEnum<Ingredient.Base>();
                _cook = GetRandomEnum<Ingredient.Cook>();
            } while (_baseIngred == Ingredient.Base.noCondition && _cook == Ingredient.Cook.noCondition);
        }

        randomIndex = UnityEngine.Random.Range(0, 2);

        // 주재료 하나 특정: “오늘은 연어가 땡기네요.”
        if (randomIndex == 0)
        {
            do
            {
                GetRandomIngredient(); // Determine the ingredients(meat, fish, vege) in detail
            } while (meatfish == Ingredient.MeatFish.noCondition && vege == Ingredient.Vege.noCondition);

        }
        // 주재료 범위 지정: “저는 생선이 좋아요.”
        else if (randomIndex == 1)
        {
            do
            {
                _mainIngredCategory = GetRandomEnum<Ingredient.Main>(); // Determine only the ingred category
            } while (_mainIngredCategory == Ingredient.Main.noCondition);
        }
    }


    public void NormalOrder()
    {
        int randomIndex = UnityEngine.Random.Range(0, 2);

        // 카테고리 하나 특정: 예:“볶음밥을 만들어 주세요.”
        if (randomIndex == 0)
        {
            do
            {
                // Determine from basic category pool
                GetRandomCategory();
                _baseIngred = categoryData.baseIngred;
                _cook = categoryData.cook;
            } while (categoryData == null);
        }

        // 주재료 하나 특정: “오늘은 연어가 땡기네요.”
        else if (randomIndex == 1)
        {
            do
            {
                GetRandomIngredient(); // Determine the ingredients(meat, fish, vege) in detail
            } while (meatfish == Ingredient.MeatFish.noCondition && vege == Ingredient.Vege.noCondition);

        }

    }

    public void GenerousOrder()
    {
        int randomIndex = UnityEngine.Random.Range(0, 2);

        // 카테고리 범위 지정: “밥을 사용한 요리가 먹고 싶어요.”
        if (randomIndex == 0)
        {
            _baseIngred = GetRandomEnum<Ingredient.Base>();
            _cook = GetRandomEnum<Ingredient.Cook>();
        }

        // 주재료 범위 지정: “저는 생선이 좋아요.”
        else if (randomIndex == 1)
        {
            _mainIngredCategory = GetRandomEnum<Ingredient.Main>(); // Determine only the ingred category
        }

    }

    public void StrictOrder()
    {
        int randomIndex = UnityEngine.Random.Range(0, 3);

        if (randomIndex == 0)
        {
            PickyOrder();
        }

        else if (randomIndex == 1)
        {
            NormalOrder();
        }

        else { 
            GenerousOrder();
        }

    }

    public void RandomOrder()
    {
        int randomIndex = UnityEngine.Random.Range(0, 2);

        if (randomIndex == 0) {
            do 
            {
                GetRandomIngredient(); // Determine the ingredients(meat, fish, vege) in detail
            } while (meatfish == Ingredient.MeatFish.noCondition || vege == Ingredient.Vege.noCondition);
                
        }
        else if (randomIndex == 1)
        {
            _mainIngredCategory = GetRandomEnum<Ingredient.Main>(); // Determine only the ingred category
            if(_mainIngredCategory == Ingredient.Main.noCondition)
                do
                {
                    GetRandomIngredient(); // Determine the ingredients(meat, fish, vege) in detail
                } while (meatfish == Ingredient.MeatFish.noCondition || vege == Ingredient.Vege.noCondition);
        }

        randomIndex = UnityEngine.Random.Range(0, 2);
        if (randomIndex == 0)
        {
            // Determine from basic category pool
            GetRandomCategory();
            _baseIngred = categoryData.baseIngred;
            _cook = categoryData.cook;
        }
        else if (randomIndex == 1)
        {
            do
            {
                // Determine the category in detail
                _baseIngred = GetRandomEnum<Ingredient.Base>();
                _cook = GetRandomEnum<Ingredient.Cook>();
            } while (_baseIngred == Ingredient.Base.noCondition || _cook == Ingredient.Cook.noCondition);
        }

    }

    // get values from enum
    T GetRandomEnum<T>()
    {

        Array enumValues = Enum.GetValues(typeof(T));


        int randomIndex = UnityEngine.Random.Range(0, enumValues.Length);


        return (T)enumValues.GetValue(randomIndex);
    }
}