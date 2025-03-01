using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour, ButtonBehavior
{
    [SerializeField] private BookController controller;
    private Sprite sprite;
    [SerializeField] private Ingredient.Base baseIng;

    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
    }

    public void OnClick()
    {
        controller.OnClick(baseIng, sprite);
    }
}
