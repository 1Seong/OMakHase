using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    // ĵ���� ui
    [SerializeField]
    private RectTransform _Order_Canvas;
    public RectTransform Order_Canvas { get => _Order_Canvas; }

    [SerializeField]
    private RectTransform _Base_Canvas;
    public RectTransform Base_Canvas { get => _Base_Canvas; }
    [SerializeField]
    private RectTransform _Cook_Canvas;
    public RectTransform Cook_Canvas { get => _Cook_Canvas; }

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
        reputation = 0;
        money = 0;
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
