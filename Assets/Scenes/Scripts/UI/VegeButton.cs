using UnityEngine;
using UnityEngine.UI;

public class VegeButton : MonoBehaviour, ButtonBehavior
{
    [SerializeField] private BookController controller;
    private Sprite sprite;
    [SerializeField] private Ingredient.Vege vege;

    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
    }

    public void OnClick()
    {
        controller.OnClick(vege, sprite);
    }
}
