using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using static Ingredient;


public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;


    [SerializeField] private Image activeSpriteImage;

    public Personality currentPersonality;
    public CustomerData currentCustomer;

    [SerializeField]
    private List<Sprite> spritePool;

    [SerializeField]
    public List<CategoryData> categoryPool;

    [Serializable]
    private struct PersonalityAndCustomers
    {
        public Personality personality;
        public List<CustomerData> normalCustomerDatas;
        public List<CustomerData> specialCustomerDatas;
    }

    [SerializeField] private List<PersonalityAndCustomers> categoryListDatas;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        //spritePool = new List<Sprite>();
    }

    [SerializeField] public TextMeshProUGUI orderText;

    private void Start()
    {
        Sprite randomSprite = GetRandomSprite();
        activeSpriteImage.sprite = randomSprite;

        //test

        //GetRandomOrder();
        
        
        //Debug.Log(currentCustomer);
    }

    public List<CustomerData> GetCustomers(Personality personality, bool isSpecial, int num)
    {
        if (num < 0)
        {
            Debug.Log("num is below zero");
            return null;
        }

        List<CustomerData> result = new List<CustomerData>();
        CustomerData data;
        List<int> nopeIndex = new List<int>();
        int randomIndex;

        Func<int, bool> nopeCheck = x => {
            foreach (var i in nopeIndex)
            {
                if (x == i)
                    return false;
            }
            return true;
        };

        foreach (var i in categoryListDatas)
        {
            if(i.personality == personality)
            {
                if(isSpecial && num > i.specialCustomerDatas.Count || !isSpecial && num > i.normalCustomerDatas.Count)
                {
                    Debug.Log("num out of list range");
                    return null;
                }

                for (int j = 0; j  < num; j++)
                {
                    while (true)
                    {
                        randomIndex = isSpecial ? UnityEngine.Random.Range(0, i.specialCustomerDatas.Count) : UnityEngine.Random.Range(0, i.normalCustomerDatas.Count);

                        if (nopeCheck(randomIndex))
                        {
                            nopeIndex.Add(randomIndex);
                            break;
                        }
                    }

                    if (isSpecial) 
                        result.Add(i.specialCustomerDatas[randomIndex]);
                    else
                    {
                        data = i.normalCustomerDatas[randomIndex];
                        data.sprite = spritePool[UnityEngine.Random.Range(0, spritePool.Count)];
                        result.Add(data);
                    } 
                }
            }
        }

        return result;
    }

    public CustomerData GetCustomer(Personality personality, bool isSpecial)
    {
        CustomerData result = null;
        int randomIndex;

        foreach(var i in categoryListDatas)
        {
            if(i.personality == personality)
            {
                randomIndex = isSpecial ? UnityEngine.Random.Range(0, i.specialCustomerDatas.Count) : UnityEngine.Random.Range(0, i.normalCustomerDatas.Count);

                if (isSpecial)
                    result = i.specialCustomerDatas[randomIndex];
                else
                {
                    result = i.normalCustomerDatas[randomIndex];
                    result.sprite = spritePool[UnityEngine.Random.Range(0, spritePool.Count)];
                }
            }
        }
        return result;
    }

    // 수동으로 모두 지정하는 주문 함수들
    public void GetOrder(Personality personality, Ingredient.MeatFish meatfish, Ingredient.Vege vege, Ingredient.Base baseIngred, Ingredient.Cook cook, bool hateMeatFish, bool hateVege, bool hateBase) {

        currentCustomer = GetCustomer(personality, true);

        currentCustomer.InitializeOrder();

        currentCustomer.GetOrder(meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
        currentPersonality = currentCustomer.personality;
    }

    public void GetOrder(Personality personality, Ingredient.Main main, Ingredient.Base baseIngred, Ingredient.Cook cook, bool hateCategory, bool hateBase)
    {

        currentCustomer = GetCustomer(personality, true);

        currentCustomer.InitializeOrder();

        currentCustomer.GetOrder(main, baseIngred, cook, hateCategory, hateBase);
        currentPersonality = currentCustomer.personality;
    }

    public void GetOrder(Personality personality, Ingredient.MeatFish meatfish, Ingredient.Vege vege, CategoryData category, bool hateMeatFish, bool hateVege, bool hateBase)
    {

        currentCustomer = GetCustomer(personality, true);

        currentCustomer.InitializeOrder();

        currentCustomer.GetOrder(meatfish, vege, category, hateMeatFish, hateVege, hateBase);
        currentPersonality = currentCustomer.personality;
    }

    public void GetOrder(Personality personality, Ingredient.Main main, CategoryData category, bool hateCategory, bool hateBase)
    {

        currentCustomer = GetCustomer(personality, true);

        currentCustomer.InitializeOrder();

        currentCustomer.GetOrder(main, category, hateCategory, hateBase);
        currentPersonality = currentCustomer.personality;
    }













    /// ////////////////////////////////////////////////////////////////////////////////

    // Method to get a random guest sprite
    Sprite GetRandomSprite()
    {
        if (spritePool.Count == 0)
        {
            Debug.LogError("스프라이트 풀이 비어있다");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, spritePool.Count);
        return spritePool[randomIndex];
    }


    // Method to get a random guest order
    public void GetRandomOrder() {

        Sprite randomSprite = GetRandomSprite();
        activeSpriteImage.sprite = randomSprite;

        //test

        int randomIndex = UnityEngine.Random.Range(0, 4);

        if (randomIndex == 0)
        {
            Debug.Log("까다로운 손님");
            currentCustomer = GetCustomer(Personality.Picky, false);
        }
        else if (randomIndex == 1)
        {
            Debug.Log("보통 손님");
            currentCustomer = GetCustomer(Personality.Normal, false);
        }
        else if (randomIndex == 2)
        {
            Debug.Log("관대한 손님");
            currentCustomer = GetCustomer(Personality.Generous, false);
        }
        else
        {
            Debug.Log("엄격한 손님");
            currentCustomer = GetCustomer(Personality.Strict, false);
        }

        currentCustomer.InitializeOrder();

        orderText.text = "내가 먹고 싶은 요리는\n";

        //currentCustomer.RandomOrder();
        currentCustomer.GetOrder();
        currentPersonality = currentCustomer.personality;

        if (currentCustomer.mainIngredCategory == Ingredient.Main.meat)
        {
            orderText.text += "육류 ";

            if(currentCustomer.hateMeatFish == true)
                orderText.text += "싫어 ";
        }
        else if (currentCustomer.mainIngredCategory == Ingredient.Main.fish)
        {
            orderText.text += "생선류 ";

            if (currentCustomer.hateMeatFish == true)
                orderText.text += "싫어 ";
        }
        else if (currentCustomer.mainIngredCategory == Ingredient.Main.vege)
        {
            orderText.text += "과채류 ";

            if (currentCustomer.hateVege == true)
                orderText.text += "싫어 ";
        }

        else if (currentCustomer.mainIngredCategory == Ingredient.Main.noCondition)
        {
            if (currentCustomer.meatfish == Ingredient.MeatFish.beef)
            {
                orderText.text += "소고기 ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "싫어 ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.salmon)
            {
                orderText.text += "연어 ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "싫어 ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.tuna)
            {
                orderText.text += "참치 ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "싫어 ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.pork)
            {
                orderText.text += "돼지고기 ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "싫어 ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.chicken)
            {
                orderText.text += "닭고기 ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "싫어 ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.none)
            {
                orderText.text += "육류, 생선류 넣지말고 ";
            }

            if (currentCustomer.vege == Ingredient.Vege.potato)
            {
                orderText.text += "감자 ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "싫어 ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.tomato)
            {
                orderText.text += "토마토 ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "싫어 ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.carrot)
            {
                orderText.text += "당근 ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "싫어 ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.mushroom)
            {
                orderText.text += "버섯 ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "싫어 ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.none)
            {
                orderText.text += "과채류 넣지말고 ";
            }
        }
        

        if (currentCustomer.baseIngred == Ingredient.Base.rice) {
            orderText.text += "쌀 ";

            if (currentCustomer.hateBase == true)
                orderText.text += "싫어 ";
        }
        else if (currentCustomer.baseIngred == Ingredient.Base.bread)
        {
            orderText.text += "빵 ";

            if (currentCustomer.hateBase == true)
                orderText.text += "싫어 ";
        }
        else if (currentCustomer.baseIngred == Ingredient.Base.noodle)
        {
            orderText.text += "면 ";

            if (currentCustomer.hateBase == true)
                orderText.text += "싫어 ";
        }
        else if (currentCustomer.baseIngred == Ingredient.Base.noCondition)
        {
            orderText.text += "쌀, 빵, 면 아무거나 ";
        }

        if (currentCustomer.cook == Ingredient.Cook.none)
        {
            orderText.text += "\n을 조리하지 않은 요리야";
        }
        else if (currentCustomer.cook == Ingredient.Cook.stirFry)
        {
            orderText.text += "\n을 볶은 요리야";
        }
        else if (currentCustomer.cook == Ingredient.Cook.roast)
        {
            orderText.text += "\n을 구운 요리야";
        }

    }

}
