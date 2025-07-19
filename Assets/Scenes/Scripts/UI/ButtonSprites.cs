using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSprites : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite selectedSprite;
    public Sprite buttonOnSprite;
    public Sprite deselectedSprite;
    public Image image;

    private bool selected = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = buttonOnSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = selected ? selectedSprite : deselectedSprite;
    }

    public void OnSelected()
    {
        selected = true;
        image.sprite = selectedSprite;
    }

    public void OnDeselected()
    {
        selected = false;
        image.sprite = deselectedSprite;
    }
}
