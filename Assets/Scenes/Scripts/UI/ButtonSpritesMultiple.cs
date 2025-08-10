using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSpritesMultiple : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Serializable]
    private struct SpriteChange { public Image target; public Sprite Selected; public Sprite ButtonOn; public Sprite Deselected; }

    [SerializeField]
    private SpriteChange[] TargetImages = null;
 
    private bool selected = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach(var image in TargetImages)
        {
            image.target.sprite = image.ButtonOn;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var image in TargetImages)
        {
            image.target.sprite = selected ? image.Selected : image.Deselected;
        }
    }

    public void OnSelected()
    {
        selected = true;
        foreach (var image in TargetImages)
        {
            image.target.sprite = image.Selected;
        }
    }

    public void OnDeselected()
    {
        selected = false;
        foreach (var image in TargetImages)
        {
            image.target.sprite = image.Deselected;
        }
    }
}
