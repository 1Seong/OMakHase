using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // 날짜 변경시의 이벤트
    //public event Action DayPassEvent;

    [Header("PlayerInfo")]
    // 플레이어 이름
    public string player_name;
    // 날짜
    public int day;
    // 고객 수
    public int customerNum;
    // 평판 수치
    public int reputation;
    // 돈 수치
    public int money;

    // 업적 목록
    public List<bool> achievements;

    // 시간
    public enum Time {Morning, Afternoon, Night};

    // 날짜 별 고객 수 목록
    [SerializeField]
    private List<int> customerPerDay;
    public int GetCustomerNum(int index) { return customerPerDay[index]; }

    [Header("EndingInfo")]
    public int GoodEndingCriteria;
    public int NormalCriteria;
    // 엔딩 모드 클리어?
    public bool sawEnding;

    // 캔버스 ui
    [Header("UI")]
    [SerializeField]
    private RectTransform _Order_Canvas;
    public RectTransform Order_Canvas { get => _Order_Canvas; }
    [SerializeField]
    private RectTransform _Fade_Panel;
    public RectTransform Fade_Panel { get => _Fade_Panel; }
    //Tutorial
    [Header("Tutorial")]
    public bool TutorialActive = false;
    public void SetTutorialActive(bool b)
    {
        TutorialActive = b;
    }
    public void SaveTutorialFinish()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    [SerializeField]
    private RectTransform _Base_Canvas;
    public RectTransform Base_Canvas { get => _Base_Canvas; }
    [SerializeField]
    private RectTransform _Cook_Canvas;
    public RectTransform Cook_Canvas { get => _Cook_Canvas; }
    [SerializeField]
    private RectTransform _Dialogue_Canvas;
    public RectTransform Dialogue_Canvas { get => _Dialogue_Canvas; }

    [Header("Flags")]
    public bool sneakyAdsFlag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        initGame();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initGame() {
        day = 0;
        customerNum = 1;
        reputation = 0;
        money = 0;

        AudioManager.Instance.BGMTrackChange("restaurant");
        UnlockManager.instance.ResetData(); // this is full reset (deletes all playerprefs data)

    }

    public void nextCustomer() {
        if (day > customerPerDay.Count)
        {
            return;
        }

        if (customerNum < customerPerDay[day])
        {
            customerNum++;
        }
        else {
            //Fade_Panel.gameObject.SetActive(true);
            //DayPassEvent();

            day++;
            if (GameManager.instance.day == 2)
            {
                UnlockManager.instance.Unlock(Ingredient.MeatFish.chicken);
            }
            else if (GameManager.instance.day == 4) {
                UnlockManager.instance.Unlock(Ingredient.Vege.mushroom);
            }
            else if (GameManager.instance.day == 5)
            {
                UnlockManager.instance.Unlock(Ingredient.Base.noodle);
            }
            else if (GameManager.instance.day == 6)
            {
                UnlockManager.instance.Unlock(Ingredient.MeatFish.salmon);
            }
            else if (GameManager.instance.day == 8)
            {
                UnlockManager.instance.Unlock(Ingredient.Vege.carrot);
            }
            else if (GameManager.instance.day == 10)
            {
                UnlockManager.instance.Unlock(Ingredient.MeatFish.beef);
            }
            customerNum = 1;
        }
    }

    public void setFlag(bool flag, bool Bool)
    {
        flag = Bool;
    }

}
