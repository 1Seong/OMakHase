using NUnit.Framework;
using System;
using TreeEditor;
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
        // ���� �丮

        RecipeData food = RecipeManager.instance.GetRecipe(baseIngred, cook, meatfish, vege);
        CustomerData customer = CustomerManager.instance.currentCustomer;

        bool requestSatisfied;

        // �� �䱸���� �˻�
        requestSatisfied = customer.CheckCondition(food);

        Debug.Log(food.recipeName);
        Debug.Log(requestSatisfied);


        // ���� ����
        if (requestSatisfied)
        {
            GameManager.instance.reputation += judge(food);
        }
        else {

            if (CustomerManager.instance.currentPersonality == Personality.Picky && food.taste >= 7)
            {
                GameManager.instance.reputation += judge(food);
            }

            else if (CustomerManager.instance.currentPersonality == Personality.Normal && food.taste >= 5)
            {
                GameManager.instance.reputation += judge(food);
            }

            else if (CustomerManager.instance.currentPersonality == Personality.Generous && food.taste >= 3)
            {
                GameManager.instance.reputation += judge(food);
            }
        }
        

    }

    public int judge(RecipeData food ) 
    {
        int person = 0;
        if (CustomerManager.instance.currentPersonality == Personality.Picky)
        {
            person = 5;
        }

        else if (CustomerManager.instance.currentPersonality == Personality.Normal)
        {
            person = 5;
        }

        else if (CustomerManager.instance.currentPersonality == Personality.Generous)
        {
            person = 3;
        }

        else
        {
            person = 7;
        }

        if (food.isNew) {
            return 6 + Math.Max((food.taste - person), 0);
        }

        else
        {
            return 3 + Math.Max((food.taste - person), 0);
        }

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
