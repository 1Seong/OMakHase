using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookController : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject combinationPanel;
    [SerializeField] private Slider taste;
    [SerializeField] private RectTransform RightPageUI;

    private Sprite baseSprite;
    private Sprite ingred1Sprite;
    private Sprite ingred2Sprite;
    private Sprite cookSprite;

    [SerializeField] private Ingredient.Base _baseIngred;
    public Ingredient.Base baseIngred
    {
        get => _baseIngred;
        set
        {
            _baseIngred = value;
            UpdateRightPage();
        }
    }

    [SerializeField] private Ingredient.Cook _cook;
    public Ingredient.Cook cook
    {
        get => _cook;
        set
        {
            _cook = value;
            UpdateRightPage();
        }
    }

    [SerializeField] private Ingredient.MeatFish _meatFish;
    public Ingredient.MeatFish meatFish
    {
        get => _meatFish;
        set
        {
            _meatFish = value;
            UpdateRightPage();
        }
    }

    [SerializeField] private Ingredient.Vege _vege;
    public Ingredient.Vege vege
    {
        get => _vege;
        set
        {
            _vege = value;
            UpdateRightPage();
        }
    }

    private void UpdateRightPage()
    {
        var data = RecipeManager.instance.GetRecipe(baseIngred, cook, meatFish, vege);
        var combinationImages = combinationPanel.GetComponentsInChildren<Image>();
        var baseIcon = combinationImages[1];
        var ingredIcon1 = combinationImages[2];
        var ingredIcon2 = combinationImages[3];
        var cookIcon = combinationImages[4];

        if (data != null)
        {
            if (RightPageUI.localScale.x == 0) RightPageUI.localScale = new Vector3(1, 1, 1);

            image.sprite = data.categoryData.sprite;
            title.text = data.recipeName;
            taste.value = data.taste;

            baseIcon.sprite = baseSprite;
            ingredIcon1.sprite = ingred1Sprite;
            ingredIcon2.sprite = ingred2Sprite;
            cookIcon.sprite = cookSprite;
        }
        
    }

    public void OnClick(Ingredient.Base baseIng, Sprite sprite)
    {
        baseIngred = baseIng;
    }

    public void OnClick(Ingredient.Cook cook, Sprite sprite)
    {
        this.cook = cook;
    }

    public void OnClick(Ingredient.MeatFish meat, Sprite sprite)
    {
        meatFish = meat;
    }

    public void OnClick(Ingredient.Vege vege, Sprite sprite)
    {
        this.vege = vege;
    }
}
