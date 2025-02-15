using System;
using UnityEngine;

public class CookManager : MonoBehaviour
{
    public static CookManager instance;

    public Ingredient.MeatFish meatfish;

    public Ingredient.Vege vege;

    public Ingredient.Base baseIngred;

    public Ingredient.Cook cook;

    public void getMeatFish(int index) 
    {
        meatfish = GetDataFromEnum<Ingredient.MeatFish>(index);
    }

    public void getVege(int index)
    {
        vege = GetDataFromEnum<Ingredient.Vege>(index);
    }

    public void getBase(int index)
    {
        baseIngred = GetDataFromEnum<Ingredient.Base>(index);
    }

    public void getCook(int index)
    {
        cook = GetDataFromEnum<Ingredient.Cook>(index);
    }

    public void initCook() { 
        meatfish = Ingredient.MeatFish.noCondition;
        vege = Ingredient.Vege.noCondition;
        baseIngred = Ingredient.Base.noCondition;
        cook = Ingredient.Cook.noCondition;
    }


    T GetDataFromEnum<T>(int index)
    {

        Array enumValues = Enum.GetValues(typeof(T));


        return (T)enumValues.GetValue(index);
    }

    public void cookFood()
    {

        RecipeData food = RecipeManager.instance.GetRecipe(baseIngred, cook, meatfish, vege);
        CustomerData customer = CustomerManager.instance.currentCustomer;

        bool requestSatisfied = false;

        // 요구조건 검사


        // 주재료와 요리방법 검사
        if (customer.baseIngred == food.categoryData.baseIngred && customer.cook == food.categoryData.cook) 
        {

            // 메인 카테고리 검사
            if (customer.mainIngredCategory != Ingredient.Main.noCondition)
            {
                if (customer.mainIngredCategory == Ingredient.Main.meat && (food.meatfish == Ingredient.MeatFish.beef || food.meatfish == Ingredient.MeatFish.chicken || food.meatfish == Ingredient.MeatFish.pork))
                {
                    requestSatisfied = true;
                }
                else if (customer.mainIngredCategory == Ingredient.Main.fish && (food.meatfish == Ingredient.MeatFish.tuna || food.meatfish == Ingredient.MeatFish.salmon))
                {
                    requestSatisfied = true;
                }
                else if (customer.mainIngredCategory == Ingredient.Main.vege && (food.vege == Ingredient.Vege.potato || food.vege == Ingredient.Vege.tomato || food.vege == Ingredient.Vege.carrot || food.vege == Ingredient.Vege.mushroom))
                {
                    requestSatisfied = true;
                }
            }
            // 메인 카테고리로 지정을 한 것이 아닐 때
            else
            {
                if (customer.meatfish == food.meatfish && customer.vege == food.vege)
                {
                    requestSatisfied = true;
                }
            }

        }



        Debug.Log(food.recipeName);
        Debug.Log(requestSatisfied);

    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
