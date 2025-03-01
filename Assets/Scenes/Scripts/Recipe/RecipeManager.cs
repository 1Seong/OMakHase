using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager instance;

    [Serializable]
    private struct CategoryAndRecipes
    {
        public CategoryData categoryData;
        public List<RecipeData> recipeDatas;
    }

    [SerializeField] private List<CategoryAndRecipes> categoryListDatas;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public RecipeData FindNormal(Ingredient.Base baseIng, Ingredient.Cook cook, Ingredient.MeatFish meat, Ingredient.Vege vege)
    {
        CategoryData categoryData = new CategoryData(baseIng, cook);

        foreach(var i in categoryListDatas)
        {
            if (i.categoryData.Equals(categoryData))
            {
                foreach(var j in i.recipeDatas)
                {
                    if (j.meatfish == meat && j.vege == vege) return j;
                }
            }
        }

        Debug.Log("Cannot Find Recipe");
        return null;
    }

    //public RecipeData FindHard() { }
}
