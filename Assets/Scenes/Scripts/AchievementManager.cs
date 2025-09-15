using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    public AchievementData[] achievementList;
    Dictionary<AchievementData, bool> achieved;

    [SerializeField] RectTransform achievementUI; // 機瞳 で機 UI
    [SerializeField] TextMeshProUGUI achievementUItext; // 機瞳 で機 UI 臢蝶お
    [SerializeField] protected float speed = 5f;

    protected Vector2 hiddenPosition;
    protected Vector2 visiblePosition;

    public bool isActing;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        hiddenPosition = new Vector2(0f, achievementUI.GetComponent<RectTransform>().rect.height);
        visiblePosition = Vector2.zero;

        achieved = new Dictionary<AchievementData, bool>();

        foreach (AchievementData achievement in achievementList) {
            achieved[achievement] = false;
        }

    }


    public void AchievedUnlock(AchievementData achievement)
    {
        if (achieved[achievement] == false)
        {
            achieved[achievement] = true;
            achievementUItext.text = achievement.achievementTitle;
        }
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

        while (achievementUI.anchoredPosition.y > visiblePosition.y + 1f)
        {
            achievementUI.anchoredPosition += (visiblePosition - achievementUI.anchoredPosition) * speed * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }

        achievementUI.anchoredPosition = visiblePosition;
        isActing = false;
    }

    protected virtual IEnumerator SlideOut()
    {
        var t = 0f;
        isActing = true;

        while (achievementUI.anchoredPosition.y < hiddenPosition.y - 1f)
        {
            achievementUI.anchoredPosition += (hiddenPosition - achievementUI.anchoredPosition) * speed * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }

        achievementUI.anchoredPosition = hiddenPosition;
        isActing = false;
    }
}
