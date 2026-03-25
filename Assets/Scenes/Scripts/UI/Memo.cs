using TMPro;
using UnityEngine;

public class Memo : MonoBehaviour
{
    TextMeshProUGUI _textMesh;
    [SerializeField] TextMeshProUGUI dialogueUI;

    UISlide _uiSlide;

    private void Awake()
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        _uiSlide = GetComponentInChildren<UISlide>();
    }

    public void GetOrderText()
    {
        _textMesh.text = dialogueUI.text;
    }

    public void EraseOrderText()
    {
        if(_uiSlide.isActing == false && _uiSlide.getAnchoredPosition().y == _uiSlide.getVisiblePosition().y)
            _textMesh.text = null;
    }
}
