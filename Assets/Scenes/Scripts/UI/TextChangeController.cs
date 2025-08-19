using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextChangeController : MonoBehaviour
{

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueManager.Instance.SpriteSetEvent += setText;
    }

    private void setText()
    {
        if (DialogueManager.Instance.IsRandom)
        {
            text.color = Color.white;

        }
        else
        {
            text.color = Color.black;
        }
    }
}
