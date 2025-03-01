using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


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

        GetOrder();
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


    // Method to get a random guest sprite
    Sprite GetRandomSprite()
    {
        if (spritePool.Count == 0)
        {
            Debug.LogError("½ºÇÁ¶óÀÌÆ® Ç®ÀÌ ºñ¾îÀÖ´Ù");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, spritePool.Count);
        return spritePool[randomIndex];
    }


    // Method to get a random guest order
    public void GetOrder() {

        Sprite randomSprite = GetRandomSprite();
        activeSpriteImage.sprite = randomSprite;

        //test

        int randomIndex = UnityEngine.Random.Range(0, 4);

        if (randomIndex == 0)
        {
            Debug.Log("±î´Ù·Î¿î ¼Õ´Ô");
            currentCustomer = GetCustomer(Personality.Picky, false);
        }
        else if (randomIndex == 1)
        {
            Debug.Log("º¸Åë ¼Õ´Ô");
            currentCustomer = GetCustomer(Personality.Normal, false);
        }
        else if (randomIndex == 2)
        {
            Debug.Log("°ü´ëÇÑ ¼Õ´Ô");
            currentCustomer = GetCustomer(Personality.Generous, false);
        }
        else
        {
            Debug.Log("¾ö°ÝÇÑ ¼Õ´Ô");
            currentCustomer = GetCustomer(Personality.Strict, false);
        }

        currentCustomer.InitializeOrder();

        orderText.text = "³»°¡ ¸Ô°í ½ÍÀº ¿ä¸®´Â\n";

        //currentCustomer.RandomOrder();
        currentCustomer.GetOrder();
        currentPersonality = currentCustomer.personality;

        if (currentCustomer.mainIngredCategory == Ingredient.Main.meat)
        {
            orderText.text += "À°·ù ";

            if(currentCustomer.hateMeatFish == true)
                orderText.text += "½È¾î ";
        }
        else if (currentCustomer.mainIngredCategory == Ingredient.Main.fish)
        {
            orderText.text += "»ý¼±·ù ";

            if (currentCustomer.hateMeatFish == true)
                orderText.text += "½È¾î ";
        }
        else if (currentCustomer.mainIngredCategory == Ingredient.Main.vege)
        {
            orderText.text += "°úÃ¤·ù ";

            if (currentCustomer.hateVege == true)
                orderText.text += "½È¾î ";
        }

        else if (currentCustomer.mainIngredCategory == Ingredient.Main.noCondition)
        {
            if (currentCustomer.meatfish == Ingredient.MeatFish.beef)
            {
                orderText.text += "¼Ò°í±â ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "½È¾î ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.salmon)
            {
                orderText.text += "¿¬¾î ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "½È¾î ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.tuna)
            {
                orderText.text += "ÂüÄ¡ ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "½È¾î ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.pork)
            {
                orderText.text += "µÅÁö°í±â ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "½È¾î ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.chicken)
            {
                orderText.text += "´ß°í±â ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "½È¾î ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.none)
            {
                orderText.text += "À°·ù, »ý¼±·ù ³ÖÁö¸»°í ";
            }

            if (currentCustomer.vege == Ingredient.Vege.potato)
            {
                orderText.text += "°¨ÀÚ ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "½È¾î ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.tomato)
            {
                orderText.text += "Åä¸¶Åä ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "½È¾î ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.carrot)
            {
                orderText.text += "´ç±Ù ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "½È¾î ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.mushroom)
            {
                orderText.text += "¹ö¼¸ ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "½È¾î ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.none)
            {
                orderText.text += "°úÃ¤·ù ³ÖÁö¸»°í ";
            }
        }
        

        if (currentCustomer.baseIngred == Ingredient.Base.rice) {
            orderText.text += "½Ò ";

            if (currentCustomer.hateBase == true)
                orderText.text += "½È¾î ";
        }
        else if (currentCustomer.baseIngred == Ingredient.Base.bread)
        {
            orderText.text += "»§ ";

            if (currentCustomer.hateBase == true)
                orderText.text += "½È¾î ";
        }
        else if (currentCustomer.baseIngred == Ingredient.Base.noodle)
        {
            orderText.text += "¸é ";

            if (currentCustomer.hateBase == true)
                orderText.text += "½È¾î ";
        }
        else if (currentCustomer.baseIngred == Ingredient.Base.noCondition)
        {
            orderText.text += "½Ò, »§, ¸é ¾Æ¹«°Å³ª ";
        }

        if (currentCustomer.cook == Ingredient.Cook.none)
        {
            orderText.text += "\nÀ» Á¶¸®ÇÏÁö ¾ÊÀº ¿ä¸®¾ß";
        }
        else if (currentCustomer.cook == Ingredient.Cook.stirFry)
        {
            orderText.text += "\nÀ» ººÀº ¿ä¸®¾ß";
        }
        else if (currentCustomer.cook == Ingredient.Cook.roast)
        {
            orderText.text += "\nÀ» ±¸¿î ¿ä¸®¾ß";
        }

    }

}
