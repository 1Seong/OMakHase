using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static Ingredient;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("PlayerInfo")]
    // �÷��̾� �̸�
    public string player_name;
    // ��¥
    public int day;
    // �� ��
    public int customerNum;
    // ���� ��ġ
    public int reputation;
    // �� ��ġ
    public int money;
    // ���� ��� Ŭ����?
    public bool sawEnding;

    // ���� ���
    public List<bool> achievements;

    // �ð�
    public enum Time {Morning, Afternoon, Night};

    // ��¥ �� �� �� ���
    [SerializeField]
    private List<int> customerPerDay;
    public int GetCustomerNum(int index) { return customerPerDay[index]; }

    // ĵ���� ui
    [Header("UI")]
    [SerializeField]
    private RectTransform _Order_Canvas;
    public RectTransform Order_Canvas { get => _Order_Canvas; }

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
