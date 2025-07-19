using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScaleEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float scaleFactor = 1.1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * scaleFactor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * scaleFactor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

    
}
