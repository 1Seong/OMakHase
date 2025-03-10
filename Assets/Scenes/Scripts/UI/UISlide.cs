using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UISlide : MonoBehaviour
{
    protected RectTransform mainImage; // main image
    [SerializeField] private Image background; // background image
    [SerializeField] protected float speed = 5f;

    protected Vector2 hiddenPosition;
    protected Vector2 visiblePosition;

    public bool isActing;

    protected virtual void Awake()
    {
        mainImage = GetComponent<RectTransform>();
    }

    protected virtual void Start()
    {
        hiddenPosition = new Vector2(0f, -Screen.height);
        visiblePosition = Vector2.zero;

        // 패널을 숨김
        mainImage.anchoredPosition = hiddenPosition;
        background.color = new Color(0, 0, 0, 0); // 투명하게 설정
        background.gameObject.SetActive(false); // 배경 비활성화
    }

    public void ShowPanel()
    {
        if (isActing) return;
        StartCoroutine(SlideIn());
    }

    public void HidePanel()
    {
        if (isActing) return;
        StartCoroutine(SlideOut());
    }

    protected virtual IEnumerator SlideIn()
    {
        var t = 0f;
        isActing = true;
        background.gameObject.SetActive(true);

        while(mainImage.anchoredPosition.y < visiblePosition.y - 1f)
        {
            mainImage.anchoredPosition += (visiblePosition - mainImage.anchoredPosition) * speed * Time.deltaTime;
            background.color = new Color(0, 0, 0, Mathf.Lerp(0, 0.5f, t)); 
            t += Time.deltaTime;

            yield return null;
        }

        mainImage.anchoredPosition = visiblePosition;
        background.color = new Color(0, 0, 0, 0.5f);
        isActing = false;
    }

    protected virtual IEnumerator SlideOut()
    {
        var t = 0f;
        isActing = true;

        while(mainImage.anchoredPosition.y > hiddenPosition.y + 1f)
        {
            mainImage.anchoredPosition += (hiddenPosition - mainImage.anchoredPosition) * speed * Time.deltaTime;
            background.color = new Color(0, 0, 0, Mathf.Lerp(0.5f, 0, t));
            t += Time.deltaTime;

            yield return null;
        }

        mainImage.anchoredPosition = hiddenPosition;
        background.color = new Color(0, 0, 0, 0);
        background.gameObject.SetActive(false);
        isActing = false;
    }
}
