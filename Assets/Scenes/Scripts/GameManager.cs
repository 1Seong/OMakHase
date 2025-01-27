using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    // 레시피 목록
    public List<foodData> Recipes;

    // 고객 목록
    public List<CustomerData> Customers;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
