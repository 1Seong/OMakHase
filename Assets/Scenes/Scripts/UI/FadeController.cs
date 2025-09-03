using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image targetImage;  // ���̵��� UI �̹���
    [SerializeField] private RectTransform[] UIList;  // ���̵� ���ο� ���� Ȱ��ȭ��ų ui��
    [SerializeField] private float fadeDuration = 1.0f; // ���̵� �ɸ��� �ð� (�� ����)

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>(); // ���� ���� �� �ߴٸ� �ڱ� �ڽ� Image ��������
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
        c.a = 0f; // ������ ����
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
        c.a = 1f; // ������ ������
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
