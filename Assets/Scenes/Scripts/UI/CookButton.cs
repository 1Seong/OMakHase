using UnityEngine;
using UnityEngine.UI;

public class CookButton : MonoBehaviour, ButtonBehavior
{
    [SerializeField] private BookController controller;
    private Sprite sprite;
    [SerializeField] private Ingredient.Cook cook;

    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
    }

    public void OnClick()
    {
        controller.OnClick(cook, sprite);
    }
}
