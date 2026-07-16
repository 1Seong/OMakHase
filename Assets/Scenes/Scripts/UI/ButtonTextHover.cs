using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject textObject;
    [SerializeField] private TMP_Text textComponent;

    [TextArea]
    [SerializeField] private string hoverText;

    private void Awake()
    {
        textObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textComponent.text = hoverText;
        textObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textObject.SetActive(false);
    }
}
