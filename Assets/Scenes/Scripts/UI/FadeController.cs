using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image targetImage;  // 페이드할 UI 이미지
    [SerializeField] private RectTransform[] UIList;  // 페이드 여부에 따라 활성화시킬 ui들
    [SerializeField] private RectTransform[] CreditUIList;  // 페이드 여부에 따라 활성화시킬 크레딧 전용 ui들
    [SerializeField] private float _fadeDuration = 1.0f; // 페이드 걸리는 시간 (초 단위)

    [SerializeField] private bool _fadingInProgress = false; // 페이드 진행 여부
    public bool fadingInProgress { get => _fadingInProgress; }
    public void fadeProgressStatusChange() { _fadingInProgress = !_fadingInProgress; }

    [SerializeField] private bool _fadingDone = false; // 페이드 완료 여부
    public bool fadingDone { get => _fadingDone; }
    public void fadeDoneStatusChange() { _fadingDone = !_fadingDone; }

    public float fadeDuration { get => _fadeDuration; }


    private void Awake()
    {

        if (targetImage == null)
            targetImage = GetComponent<Image>(); // 만약 지정 안 했다면 자기 자신 Image 가져오기
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
        c.a = 0f; // 시작은 투명
        targetImage.color = c;

        while (elapsed < _fadeDuration) {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / _fadeDuration);
            targetImage.color = c;
            yield return null;
        }

        if (targetImage.color.a == 1 && fadeMode == 1)
        {
            enableUIs(0);
        }

        // 엔딩 크레딧 전용
        if (targetImage.color.a == 1 && fadeMode == 2)
        {
            enableUIs(1);
        }

        fadeProgressStatusChange();

        DialogueManager.Instance.dialogueReset();
        if (DialogueManager.Instance.pastID.Contains("GTR"))
            DialogueManager.Instance.SpriteSetEvent();

    }
    private IEnumerator FadeOut(int fadeMode)
    {
        fadeProgressStatusChange();

        if (targetImage.color.a == 1 && fadeMode == 1)
            disableUIs(0);

        // 엔딩 크레딧 전용
        if (targetImage.color.a == 1 && fadeMode == 2)
        {
            disableUIs(1);
        }

        float elapsed = 0f;
        Color c = targetImage.color;
        c.a = 1f; // 시작은 불투명
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

    private void enableUIs(int uiMode) {
        if (uiMode == 0) {
            for (int i = 0; i < UIList.Length; i++)
            {
                UIList[i].gameObject.SetActive(true);
            }
        }
        if (uiMode == 1) {
            for (int i = 0; i < CreditUIList.Length; i++)
            {
                CreditUIList[i].gameObject.SetActive(true);
            }
        }
    }

    private void disableUIs(int uiMode)
    {
        if (uiMode == 0)
        {
            for (int i = 0; i < UIList.Length; i++)
            {
                UIList[i].gameObject.SetActive(false);
            }
        }
        if (uiMode == 1)
        {
            for (int i = 0; i < CreditUIList.Length; i++)
            {
                CreditUIList[i].gameObject.SetActive(true);
            }
        }

    }
}
