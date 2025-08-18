using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBackgroundController : MonoBehaviour
{

    public Sprite[] BackgroundImages = null;

    private Image background;

    private void Awake()
    {
        background = GetComponent<Image>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueManager.Instance.DialogueBackgroundSetEvent += setBackground;
    }

    private void setBackground()
    {
        if (DialogueManager.Instance.IsRandom)
        {
            //background.sprite = BackgroundImages[1];
            background.color = Color.red;
        }
        else
        {
            //background.sprite = BackgroundImages[0];
            background.color = Color.white;
        }
    }


}
