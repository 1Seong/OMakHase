using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image targetImage;  // ���̵��� UI �̹���
    [SerializeField] private RectTransform[] UIList;  // ���̵� ���ο� ���� Ȱ��ȭ��ų ui��
    [SerializeField] private float _fadeDuration = 1.0f; // ���̵� �ɸ��� �ð� (�� ����)

    [SerializeField] private bool _fadingInProgress = false; // ���̵� ���� ����
    public bool fadingInProgress { get => _fadingInProgress; }
    public void fadeProgressStatusChange() { _fadingInProgress = !_fadingInProgress; }

    [SerializeField] private bool _fadingDone = false; // ���̵� �Ϸ� ����
    public bool fadingDone { get => _fadingDone; }
    public void fadeDoneStatusChange() { _fadingDone = !_fadingDone; }

    public float fadeDuration { get => _fadeDuration; }


    private void Awake()
    {

        if (targetImage == null)
            targetImage = GetComponent<Image>(); // ���� ���� �� �ߴٸ� �ڱ� �ڽ� Image ��������
        DialogueManager.Instance.DayPassEvent += StartFadeIn;
        DialogueManager.Instance.FadeEvent += StartFade;

    }

    private void Start()
    {
    }

    public void StartFadeIn(int fadeMode)
    {
        StartCoroutine(FadeIn(fadeMode));
    }

    public void StartFadeOut(int fadeMode)
    {
        StartCoroutine(FadeOut(fadeMode));
    }

    public void StartFade(int fadeMode) {
        StartCoroutine(Fade(fadeMode));
    }

    private IEnumerator FadeIn(int fadeMode)
    {
        fadeProgressStatusChange();

        float elapsed = 0f;
        Color c = targetImage.color;
        c.a = 0f; // ������ ����
        targetImage.color = c;

        while (elapsed < _fadeDuration) {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / _fadeDuration);
            targetImage.color = c;
            yield return null;
        }

        if (targetImage.color.a == 1 && fadeMode == 1)
            enableUIs();

        fadeProgressStatusChange();

        DialogueManager.Instance.dialogueReset();
        if (DialogueManager.Instance.pastID.Contains("GTR"))
            DialogueManager.Instance.SpriteSetEvent();

    }
    private IEnumerator FadeOut(int fadeMode)
    {
        fadeProgressStatusChange();

        if (targetImage.color.a == 1 && fadeMode == 1)
            disableUIs();

        float elapsed = 0f;
        Color c = targetImage.color;
        c.a = 1f; // ������ ������
        targetImage.color = c;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(1f - (elapsed / _fadeDuration));
            targetImage.color = c;
            yield return null;
        }

        fadeProgressStatusChange();

        fadeDoneStatusChange();

        DialogueManager.Instance.GetNextDialogue();

        gameObject.SetActive(false);
    }

    private IEnumerator Fade(int fadeMode)
    {
        StartCoroutine(FadeIn(fadeMode));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(FadeOut(fadeMode));
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
