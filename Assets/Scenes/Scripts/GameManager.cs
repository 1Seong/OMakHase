using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // �÷��̾� �̸�
    public string player_name;
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


    // �� ���
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
