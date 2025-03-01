using NUnit.Framework;
using System;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CookManager : MonoBehaviour
{
    public static CookManager instance;

    public Ingredient.MeatFish meatfish;

    public Ingredient.Vege vege;

    public Ingredient.Base baseIngred;

    public Ingredient.Cook cook;

    [SerializeField, HideInInspector]
    private int _reputationRise;  // 인스펙터에서 수정 불가

    public int ReputationRise => _reputationRise;



    private RectTransform _OrderCanvas;

    private RectTransform _OrderButton;

    private RectTransform _CookCanvas;

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
        // 음식 요리

        RecipeData food = RecipeManager.instance.GetRecipe(baseIngred, cook, meatfish, vege);
        CustomerData customer = CustomerManager.instance.currentCustomer;

        bool requestSatisfied;

        if (food != null)
        {
            // 고객 요구조건 검사
            requestSatisfied = customer.CheckCondition(food);

            Debug.Log(food.recipeName);
            Debug.Log(requestSatisfied);

            _reputationRise = judge(food);

            // 평판 증감
            if (requestSatisfied)
            {
                GameManager.instance.reputation += ReputationRise;
                CustomerManager.instance.orderText.text = "맛있다";
            }
            else
            {

                if (CustomerManager.instance.currentPersonality == Personality.Picky && food.taste >= 7)
                {
                    GameManager.instance.reputation += ReputationRise;
                    CustomerManager.instance.orderText.text = "먹을만 하다";
                }

                else if (CustomerManager.instance.currentPersonality == Personality.Normal && food.taste >= 5)
                {
                    GameManager.instance.reputation += ReputationRise;
                    CustomerManager.instance.orderText.text = "먹을만 하다";
                }

                else if (CustomerManager.instance.currentPersonality == Personality.Generous && food.taste >= 3)
                {
                    GameManager.instance.reputation += ReputationRise;
                    CustomerManager.instance.orderText.text = "먹을만 하다";
                }

                else
                {
                    CustomerManager.instance.orderText.text = "맛없다";
                }
            }

            _OrderCanvas.gameObject.SetActive(true);
            _CookCanvas.gameObject.SetActive(false);
            _OrderButton.gameObject.SetActive(true);

        }
        else 
        {
            Debug.Log("없는 레시피입니다");
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

        _OrderCanvas = GameManager.instance.Order_Canvas.GetComponent<RectTransform>();
        //Debug.Log(_OrderCanvas);
        _OrderButton = GameManager.instance.Order_Canvas.transform.GetChild(2).GetComponent<RectTransform>();
        //Debug.Log(_OrderButton);
        _CookCanvas = GameManager.instance.Cook_Canvas.GetComponent<RectTransform>();
        //Debug.Log(_OrderCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
