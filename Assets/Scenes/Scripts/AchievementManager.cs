using System;
using TMPro;
using UnityEngine;

public enum Achievements { 
    BadEnding, NormalEnding, GoodEnding,
    Chicken, Mushroom, Noodle, Salmon, Carrot, Beef,
    StoryKang, StoryChoi, StoryJang, StoryLeeAccept, StoryLeeDecline,
    Pos10, Neutral10, Neg10,
    Reput10
}

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    public AchievementData[] achievementList;

    [SerializeField] Popup achievementUI; // 업적 팝업 UI
    [SerializeField] TextMeshProUGUI achievementUItext; // 업적 팝업 UI 텍스트
    //[SerializeField] protected float speed = 5f;

    //protected Vector2 hiddenPosition;
    //protected Vector2 visiblePosition;

    //public bool isActing;

    private const string positiveCountKey = "AchivmPosCnt";
    private const string neutralCountKey = "AchivmNetCnt";
    private const string negativeCountKey = "AchivmNegCnt";
    private const string reputationKey = "AchivmRep";

    #region parameterizedAchievement
    // ===================================== 반응 업적과 명성 업적은 아래 함수들을 호출

    // 긍정 반응 카운트 수 증가 및 업적 해금
    public void IncreasePositiveResponse()
    {
        var n = PlayerPrefs.GetInt(positiveCountKey, 0);
        ++n;
        if (n == 10 && PlayerPrefs.GetInt(Achievements.Pos10.ToString(), 0) == 0)
        {
            AchievementUnlock(Achievements.Pos10);
        }
        PlayerPrefs.SetInt(positiveCountKey, n);
        PlayerPrefs.Save();
    }

    // 중립 반응 카운트 수 증가 및 업적 해금
    public void IncreaseNeturalResponse()
    {
        var n = PlayerPrefs.GetInt(neutralCountKey, 0);
        ++n;
        if (n == 10 && PlayerPrefs.GetInt(Achievements.Neutral10.ToString(), 0) == 0)
        {
            AchievementUnlock(Achievements.Neutral10);
        }
        PlayerPrefs.SetInt(neutralCountKey, n);
        PlayerPrefs.Save();
    }

    // 부정 반응 카운트 수 증가 및 업적 해금
    public void IncreaseNegativeResponse()
    {
        var n = PlayerPrefs.GetInt(negativeCountKey, 0);
        ++n;
        if (n == 10 && PlayerPrefs.GetInt(Achievements.Neg10.ToString(), 0) == 0)
        {
            AchievementUnlock(Achievements.Neg10);
        }
        PlayerPrefs.SetInt(negativeCountKey, n);
        PlayerPrefs.Save();
    }

    // 명성 값 설정 및 업적 해금
    public void SetReputation(int v)
    {
        if (v >= 10 && PlayerPrefs.GetInt(Achievements.Reput10.ToString(), 0) == 0)
        {
            AchievementUnlock(Achievements.Pos10);
        }
        PlayerPrefs.Save();
    } 
    #endregion

    // ================================================== 업적 해금
    // 주의 : 파라미터가 필요한 업적(반응, 명성)들은 이 함수를 사용하지 말고 대신 위 함수들을 사용하시오.
    public void AchievementUnlock(Achievements achvm)
    {
        if (PlayerPrefs.GetInt(achvm.ToString(), 0) == 1) return;

        PlayerPrefs.SetInt(achvm.ToString(), 1);
        PlayerPrefs.Save();

        achievementUItext.text = achievementList[(int)achvm].achievementTitle;
        achievementUI.PopInAndOut();
    }

    #region resetData
    // ============================================= 데이터 리셋 관련
    
    // 업적 관련 전부 초기화
    public void ResetAllAchievements()
    {
        foreach(var i in Enum.GetValues(typeof(Achievements)))
        {
            PlayerPrefs.SetInt(i.ToString(), 0);
        }
        PlayerPrefs.SetInt(positiveCountKey, 0);
        PlayerPrefs.SetInt(negativeCountKey, 0);
        PlayerPrefs.SetInt(neutralCountKey, 0);
        PlayerPrefs.Save();
    }

    // 특정 업적 초기화
    // 파라미터가 사용되는 업적은 그 파라미터도 함께 초기화
    public void ResetAchievement(Achievements a)
    {
        PlayerPrefs.SetInt(a.ToString(), 0);

        if(a == Achievements.Pos10)
        {
            PlayerPrefs.SetInt(positiveCountKey, 0);
        }
        else if (a == Achievements.Neutral10)
        {
            PlayerPrefs.SetInt(neutralCountKey, 0);
        }
        else if (a == Achievements.Neg10)
        {
            PlayerPrefs.SetInt(negativeCountKey, 0);
        }
        PlayerPrefs.Save();
    }

    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /*
    void Start()
    {
        //hiddenPosition = new Vector2(0f, achievementUI.GetComponent<RectTransform>().rect.height);
        //visiblePosition = Vector2.zero;
    }

    // ================================= 팝업 관련은 Popup 스크립트 사용
    
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
    */
}
