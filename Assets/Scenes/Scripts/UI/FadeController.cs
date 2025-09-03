using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image targetImage;  // 페이드할 UI 이미지
    [SerializeField] private RectTransform[] UIList;  // 페이드 여부에 따라 활성화시킬 ui들
    [SerializeField] private float fadeDuration = 1.0f; // 페이드 걸리는 시간 (초 단위)

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>(); // 만약 지정 안 했다면 자기 자신 Image 가져오기
        GameManager.instance.DayPassEvent += StartFadeIn;
    }

    private void Start()
    {
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color c = targetImage.color;
        c.a = 0f; // 시작은 투명
        targetImage.color = c;

        while (elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            targetImage.color = c;
            yield return null;
        }
        if(targetImage.color.a == 1)
            enableUIs();
    }
    private IEnumerator FadeOut()
    {
        if (targetImage.color.a == 1)
            disableUIs();

        float elapsed = 0f;
        Color c = targetImage.color;
        c.a = 1f; // 시작은 불투명
        targetImage.color = c;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(1f - (elapsed / fadeDuration));
            targetImage.color = c;
            yield return null;
        }

        gameObject.SetActive(false);
    }
    private void enableUIs() {
        for (int i = 0; i < UIList.Length; i++) {
            UIList[i].gameObject.SetActive(true);
        }
    }

    private void disableUIs()
    {
        for (int i = 0; i < UIList.Length; i++)
        {
            UIList[i].gameObject.SetActive(false);
        }
    }
}
