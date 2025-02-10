using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UISlide : MonoBehaviour
{
    private RectTransform mainImage; // main image
    [SerializeField] private Image background; // background image
    [SerializeField] private float slideDuration = 0.4f; // slide duration

    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;

    private void Awake()
    {
        mainImage = GetComponent<RectTransform>();
    }

    private void Start()
    {
        hiddenPosition = new Vector2(0f, -Screen.height);
        visiblePosition = Vector2.zero;

        // �г��� ����
        mainImage.anchoredPosition = hiddenPosition;
        background.color = new Color(0, 0, 0, 0); // �����ϰ� ����
        background.gameObject.SetActive(false); // ��� ��Ȱ��ȭ
    }

    public void ShowPanel()
    {
        StartCoroutine(SlideIn());
    }

    public void HidePanel()
    {
        StartCoroutine(SlideOut());
    }

    private IEnumerator SlideIn()
    {
        background.gameObject.SetActive(true);

        for(float i = 0; i < slideDuration; i += Time.deltaTime)
        {
            mainImage.anchoredPosition = Vector2.Lerp(hiddenPosition, visiblePosition, i / slideDuration);
            background.color = new Color(0, 0, 0, Mathf.Lerp(0, 0.5f, i/slideDuration)); 

            yield return null;
        }

        mainImage.anchoredPosition = visiblePosition;
        background.color = new Color(0, 0, 0, 0.5f);
    }

    private IEnumerator SlideOut()
    {
        for (float i = 0; i < slideDuration; i += Time.deltaTime)
        {
            mainImage.anchoredPosition = Vector2.Lerp(visiblePosition, hiddenPosition, i / slideDuration);
            background.color = new Color(0, 0, 0, Mathf.Lerp(0.5f, 0, i / slideDuration));

            yield return null;
        }

        mainImage.anchoredPosition = hiddenPosition;
        background.color = new Color(0, 0, 0, 0);
        background.gameObject.SetActive(false);
    }
}
