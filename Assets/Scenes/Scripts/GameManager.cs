using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // 플레이어 이름
    public string player_name;
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


    // 고객 목록
    public List<CustomerData> Customers;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
