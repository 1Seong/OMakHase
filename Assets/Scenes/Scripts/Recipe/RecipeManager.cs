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

  
}
