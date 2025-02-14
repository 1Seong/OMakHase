using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    // 엔딩 모드 클리어?
    public bool sawEnding;

    // 업적 목록
    public List<bool> achievements;

    // 시간
    public enum Time {Morning, Afternoon, Night};

    // 날짜 별 고객 수 목록
    [SerializeField]
    private List<int> customerPerDay;


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
        day = 1;
        customerNum = 1;
        reputation = 1;
        money = 1;
    }

    public void nextCustomer() {
        if (day > customerPerDay.Count)
        {
            return;
        }
        if (customerNum < customerPerDay[day - 1])
        {
            customerNum++;
        }
        else {
            day++;
            customerNum = 1;
        }
    }
}
