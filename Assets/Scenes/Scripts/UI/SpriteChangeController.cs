using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChangeController : MonoBehaviour
{
    public Sprite[] Sprites = null;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueManager.Instance.SpriteSetEvent += setImage;
    }

    private void setImage()
    {
        if (DialogueManager.Instance.IsRandom)
        {
            image.sprite = Sprites[1];

        }
        else
        {
            image.sprite = Sprites[0];
        }
    }
}
