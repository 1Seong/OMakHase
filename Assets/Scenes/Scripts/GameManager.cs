using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    // ������ ���
    public List<foodData> Recipes;

    // �� ���
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
