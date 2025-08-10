using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScaleEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float scaleFactor = 1.1f;
    public Image[] TargetImages = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (TargetImages is null || TargetImages.Length == 0)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            foreach(var image in TargetImages)
            {
                image.transform.localScale = Vector3.one;
            }
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (TargetImages is null || TargetImages.Length == 0)
        {
            transform.localScale = Vector3.one * scaleFactor;
        }
        else
        {
            foreach (var image in TargetImages)
            {
                image.transform.localScale = Vector3.one * scaleFactor;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TargetImages is null || TargetImages.Length == 0)
        {
            transform.localScale = Vector3.one * scaleFactor;
        }
        else
        {
            foreach (var image in TargetImages)
            {
                image.transform.localScale = Vector3.one * scaleFactor;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TargetImages is null || TargetImages.Length == 0)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            foreach (var image in TargetImages)
            {
                image.transform.localScale = Vector3.one;
            }
        }
    }

    
}
