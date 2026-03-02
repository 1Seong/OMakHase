using TMPro;
using UnityEngine;

public class Memo : MonoBehaviour
{
    TextMeshProUGUI _textMesh;
    [SerializeField] TextMeshProUGUI dialogueUI;

    private void Awake()
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void GetOrderText()
    {
        _textMesh.text = dialogueUI.text;
    }

    public void EraseOrderText()
    {
        _textMesh.text = null;
    }
}
