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

    // �������� ��� �����ϴ� �ֹ� �Լ���
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
            Debug.LogError("��������Ʈ Ǯ�� ����ִ�");
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
            Debug.Log("��ٷο� �մ�");
            currentCustomer = GetCustomer(Personality.Picky, false);
        }
        else if (randomIndex == 1)
        {
            Debug.Log("���� �մ�");
            currentCustomer = GetCustomer(Personality.Normal, false);
        }
        else if (randomIndex == 2)
        {
            Debug.Log("������ �մ�");
            currentCustomer = GetCustomer(Personality.Generous, false);
        }
        else
        {
            Debug.Log("������ �մ�");
            currentCustomer = GetCustomer(Personality.Strict, false);
        }

        currentCustomer.InitializeOrder();

        orderText.text = "���� �԰� ���� �丮��\n";

        //currentCustomer.RandomOrder();
        currentCustomer.GetOrder();
        currentPersonality = currentCustomer.personality;

        if (currentCustomer.mainIngredCategory == Ingredient.Main.meat)
        {
            orderText.text += "���� ";

            if(currentCustomer.hateMeatFish == true)
                orderText.text += "�Ⱦ� ";
        }
        else if (currentCustomer.mainIngredCategory == Ingredient.Main.fish)
        {
            orderText.text += "������ ";

            if (currentCustomer.hateMeatFish == true)
                orderText.text += "�Ⱦ� ";
        }
        else if (currentCustomer.mainIngredCategory == Ingredient.Main.vege)
        {
            orderText.text += "��ä�� ";

            if (currentCustomer.hateVege == true)
                orderText.text += "�Ⱦ� ";
        }

        else if (currentCustomer.mainIngredCategory == Ingredient.Main.noCondition)
        {
            if (currentCustomer.meatfish == Ingredient.MeatFish.beef)
            {
                orderText.text += "�Ұ�� ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "�Ⱦ� ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.salmon)
            {
                orderText.text += "���� ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "�Ⱦ� ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.tuna)
            {
                orderText.text += "��ġ ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "�Ⱦ� ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.pork)
            {
                orderText.text += "������� ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "�Ⱦ� ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.chicken)
            {
                orderText.text += "�߰�� ";

                if (currentCustomer.hateMeatFish == true)
                    orderText.text += "�Ⱦ� ";
            }
            else if (currentCustomer.meatfish == Ingredient.MeatFish.none)
            {
                orderText.text += "����, ������ �������� ";
            }

            if (currentCustomer.vege == Ingredient.Vege.potato)
            {
                orderText.text += "���� ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "�Ⱦ� ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.tomato)
            {
                orderText.text += "�丶�� ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "�Ⱦ� ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.carrot)
            {
                orderText.text += "��� ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "�Ⱦ� ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.mushroom)
            {
                orderText.text += "���� ";

                if (currentCustomer.hateVege == true)
                    orderText.text += "�Ⱦ� ";
            }
            else if (currentCustomer.vege == Ingredient.Vege.none)
            {
                orderText.text += "��ä�� �������� ";
            }
        }
        

        if (currentCustomer.baseIngred == Ingredient.Base.rice) {
            orderText.text += "�� ";

            if (currentCustomer.hateBase == true)
                orderText.text += "�Ⱦ� ";
        }
        else if (currentCustomer.baseIngred == Ingredient.Base.bread)
        {
            orderText.text += "�� ";

            if (currentCustomer.hateBase == true)
                orderText.text += "�Ⱦ� ";
        }
        else if (currentCustomer.baseIngred == Ingredient.Base.noodle)
        {
            orderText.text += "�� ";

            if (currentCustomer.hateBase == true)
                orderText.text += "�Ⱦ� ";
        }
        else if (currentCustomer.baseIngred == Ingredient.Base.noCondition)
        {
            orderText.text += "��, ��, �� �ƹ��ų� ";
        }

        if (currentCustomer.cook == Ingredient.Cook.none)
        {
            orderText.text += "\n�� �������� ���� �丮��";
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
