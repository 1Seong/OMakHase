using System.Collections.Generic;
using UnityEngine;
using static foodData;

public class CookingManager : MonoBehaviour
{
    public static CookingManager instance;

    public foodData selectedFood;  // ���� ���õ� ������ (base��� �̸��� fooddata�� ��ϵȴ�)
    public List<foodData> Recipes; // �����ǵ�

    public enum Category { meat, fish, vegetable };
    private Category mainCategory;

    void Awake()
    {
        CookingManager.instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIngredient(int n)
    {
        selectedFood.SetIngredient((int)n);
        Debug.Log($"��� ���õ�: {selectedFood.FoodIngrdient}");
    }

    public void SetCook(int n)
    {
        selectedFood.SetCook((int)n);
        Debug.Log($"������ ���õ�: {selectedFood.FoodCook}");
    }

    public List<string> getFromCategory()
    {
        switch (mainCategory)
        {
            case Category.meat:
                return new List<string>() { "pork", "egg", "chicken", "beef" };
            case Category.fish:
                return new List<string>() { "salmon" };
            case Category.vegetable:
                return new List<string>() { "potato", "tomato", "mushroom", "carrot" };
            default:
                return null;
        }
    }

    public string getFromCategory(int i)
    {
        switch (mainCategory)
        {
            case Category.meat:
                return new List<string>() { "pork", "egg", "chicken", "beef" }[i];
            case Category.fish:
                return new List<string>() { "salmon" }[i];
            case Category.vegetable:
                return new List<string>() { "potato", "tomato", "mushroom", "carrot" }[i];
            default:
                return null;
        }
    }
}
