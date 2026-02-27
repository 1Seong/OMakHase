using System.Collections;
using UnityEngine;

public class IntroAnim : MonoBehaviour
{
    [SerializeField] private RectTransform targetImage;
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject CreditButton;
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private float duration = 3.0f;
    [SerializeField] private float targetY = 1520f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        var startY = targetImage.anchoredPosition.y;
        var elapsed = 0f;
        var skippedWaiting = false;
        var skipped = false;

        while(!skippedWaiting && elapsed < startDelay)
        {
            // 클릭 감지 시 스킵
            if (Input.GetMouseButtonDown(0))
            {
                skippedWaiting = true;
            }

            elapsed += Time.deltaTime;

            yield return null;
        }

        elapsed = 0f;

        while (!skipped && elapsed < duration)
        {
            // 클릭 감지 시 스킵
            if (Input.GetMouseButtonDown(0))
            {
                skipped = true;
            }

            elapsed += Time.deltaTime;
            var t = Mathf.Clamp01(elapsed / duration);

            // SmoothStep: 시작과 끝 모두 부드럽게 (ease in-out)
            var smoothT = t * t * (3f - 2f * t);

            var pos = targetImage.anchoredPosition;
            pos.y = Mathf.Lerp(startY, targetY, smoothT);
            targetImage.anchoredPosition = pos;

            yield return null;
        }

        // 목표 지점으로 즉시 이동 (완료 or 스킵)
        var finalPos = targetImage.anchoredPosition;
        finalPos.y = targetY;
        targetImage.anchoredPosition = finalPos;

        StartButton.SetActive(true);
        CreditButton.SetActive(true);
    }
}
