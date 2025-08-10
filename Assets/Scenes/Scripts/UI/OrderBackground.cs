using UnityEngine;
using UnityEngine.UI;

public class OrderBackground : MonoBehaviour
{
    public Sprite defaultBackground;
    public Sprite day5Background;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void changeToDefault()
    {
        image.sprite = defaultBackground; 
    }

    public void changeBackgroundDay5()
    {
        image.sprite = day5Background;
    }
}
