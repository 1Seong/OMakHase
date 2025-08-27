using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerReactionChangeController : MonoBehaviour
{
    private Sprite newSprite;
    public Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void setReactionImage(string p_SpriteName)
    {
        DialogueManager.Instance.getReactionUI.gameObject.SetActive(true);
        newSprite = Resources.Load<Sprite>("UI/" + p_SpriteName);
        if (newSprite != null)
        {

            image.sprite = newSprite;
        }
        else
        {
            Debug.Log(p_SpriteName+" 해당 이름의 파일이 존재하지 않습니다.");
        }
    }

}
