using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UISlide : MonoBehaviour
{
    private RectTransform mainImage; // main image
    [SerializeField] private Image background; // background image
    [SerializeField] private float speed = 5f;

    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;

    public bool isActing;

    private void Awake()
    {
        mainImage = GetComponent<RectTransform>();
    }

    private void Start()
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

    private IEnumerator SlideIn()
    {
        var t = 0f;
        isActing = true;
        background.gameObject.SetActive(true);

        while(mainImage.anchoredPosition.y < visiblePosition.y - 0.2f)
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

    private IEnumerator SlideOut()
    {
        var t = 0f;
        isActing = true;

        while(mainImage.anchoredPosition.y > hiddenPosition.y + 0.2f)
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
