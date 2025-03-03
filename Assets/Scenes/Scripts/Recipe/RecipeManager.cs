using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager instance;

    [Serializable]
    public struct CategoryAndRecipes
    {
        public CategoryData categoryData;
        public List<RecipeData> recipeDatas;
    }

    [SerializeField] private List<CategoryAndRecipes> categoryListDatas;
    public List<CategoryAndRecipes> getCategoryListDatas () { return categoryListDatas; }

    [SerializeField]
    private Dictionary<(Ingredient.Base, Ingredient.Cook, Ingredient.MeatFish, Ingredient.Vege), RecipeData> recipeDictionary = new Dictionary<(Ingredient.Base, Ingredient.Cook, Ingredient.MeatFish, Ingredient.Vege), RecipeData>();

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        // 리스트를 Dictionary로 변환
        foreach (var combination in categoryListDatas)
        {
            foreach (var recipe in combination.recipeDatas) {
                var key = (combination.categoryData.baseIngred, combination.categoryData.cook , recipe.meatfish, recipe.vege);
                recipeDictionary[key] = recipe;
            }
        }
    }

    public RecipeData GetRecipe(Ingredient.Base baseIngred, Ingredient.Cook cook, Ingredient.MeatFish meatfish, Ingredient.Vege vege)
    {
        var key = (baseIngred, cook,meatfish, vege);
        if (recipeDictionary.TryGetValue(key, out var recipe))
        {
            return recipe;
        }
        else
        {
            Debug.LogWarning("해당 조합의 레시피를 찾을 수 없습니다.");
            return null;
        }
    }

}
