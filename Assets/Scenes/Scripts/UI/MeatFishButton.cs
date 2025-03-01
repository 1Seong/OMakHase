using UnityEngine;
using UnityEngine.UI;

public class MeatFishButton : MonoBehaviour, ButtonBehavior
{
    [SerializeField] private BookController controller;
    private Sprite sprite;
    [SerializeField] private Ingredient.MeatFish meat;

    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
    }

    public void OnClick()
    {
        controller.OnClick(meat, sprite);
    }
}
