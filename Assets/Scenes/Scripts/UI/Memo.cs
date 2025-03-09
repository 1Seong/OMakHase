using TMPro;
using UnityEngine;

public class Memo : MonoBehaviour
{
    TextMeshProUGUI _textMesh;

    private void Awake()
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _textMesh.text = DialogueManager.Instance.currentDialogue;
    }

    private void OnDisable()
    {
        _textMesh.text = null;
    }
}
