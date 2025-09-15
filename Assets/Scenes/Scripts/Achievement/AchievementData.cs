using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementData", menuName = "Scriptable Objects/AchievementData")]
public class AchievementData : ScriptableObject
{
    // ������
    [TextArea]
    public string achievementTitle;
    // ���� ����
    [TextArea]
    public string achievementDescription;

    // ���� ������ - ���� Ȯ���� ���� ���ܵ�
    // public RewardData reward;

}
