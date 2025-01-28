using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;


    [SerializeField] private Image activeSpriteImage;

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

    [SerializeField] private TextMeshProUGUI orderText;

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
            Debug.LogError("��������Ʈ Ǯ�� ����ִ�");
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
        CustomerData currentCustomer;

        int randomIndex = UnityEngine.Random.Range(0, 3);

        if (randomIndex == 0)
        {
            Debug.Log("��ٷο� �մ�");
            currentCustomer = GetCustomer(Personality.Picky, false);
        }
        else if (randomIndex == 1)
        {
            Debug.Log("���� �մ�");
            currentCustomer = GetCustomer(Personality.Normal, false);
        }
        else
        {
            Debug.Log("������ �մ�");
            currentCustomer = GetCustomer(Personality.Generous, false);
        }

        currentCustomer.InitializeOrder();

        orderText.text = "���� �԰� ���� �丮��\n";

        currentCustomer.RandomOrder();

        if (currentCustomer.mainIngredCategory == Ingredient.Main.meat)
        {
            orderText.text += "���� ";
        }
        else if (currentCustomer.mainIngredCategory == Ingredient.Main.fish)
        {
            orderText.text += "������ ";
        }
        else if (currentCustomer.mainIngredCategory == Ingredient.Main.vege)
        {
            orderText.text += "��ä�� ";
        }

        if (currentCustomer.meatfish == Ingredient.MeatFish.beef)
        {
            orderText.text += "�Ұ�� ";
        }
        else if (currentCustomer.meatfish == Ingredient.MeatFish.salmon)
        {
            orderText.text += "���� ";
        }
        else if (currentCustomer.meatfish == Ingredient.MeatFish.egg)
        {
            orderText.text += "�ް� ";
        }
        else if (currentCustomer.meatfish == Ingredient.MeatFish.pork)
        {
            orderText.text += "������� ";
        }
        else if (currentCustomer.meatfish == Ingredient.MeatFish.chicken)
        {
            orderText.text += "�߰�� ";
        }

        if (currentCustomer.vege == Ingredient.Vege.potato)
        {
            orderText.text += "���� ";
        }
        else if (currentCustomer.vege == Ingredient.Vege.tomato)
        {
            orderText.text += "�丶�� ";
        }
        else if (currentCustomer.vege == Ingredient.Vege.carrot)
        {
            orderText.text += "��� ";
        }
        else if (currentCustomer.vege == Ingredient.Vege.mushroom)
        {
            orderText.text += "���� ";
        }

        if (currentCustomer.baseIngred == Ingredient.Base.rice) {
            orderText.text += "�� ";
        }
        else if (currentCustomer.baseIngred == Ingredient.Base.bread)
        {
            orderText.text += "�� ";
        }
        else if (currentCustomer.baseIngred == Ingredient.Base.noodle)
        {
            orderText.text += "�� ";
        }

        if (currentCustomer.cook == Ingredient.Cook.none || currentCustomer.cook == Ingredient.Cook.noCondition)
        {
            orderText.text += "\n�� �� �丮��";
        }
        else if (currentCustomer.cook == Ingredient.Cook.stirFry)
        {
            orderText.text += "\n�� ���� �丮��";
        }
        else if (currentCustomer.cook == Ingredient.Cook.roast)
        {
            orderText.text += "\n�� ���� �丮��";
        }

    }

}
