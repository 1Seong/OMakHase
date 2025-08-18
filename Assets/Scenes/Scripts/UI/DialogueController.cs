using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    private bool typeWriterEffectActive;

    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CookManager.instance.DialogueSetEvent += setText;
        DialogueManager.Instance.DialogueSetEvent += setText;
    }

    private void setText(string text)
    {
        if (typeWriterEffectActive)
        {

        }
        else
        {
            setWithNoEffect(text);
        }
    }

    private void setWithNoEffect(string text)
    {
        tmp.text = text;
    }
}
