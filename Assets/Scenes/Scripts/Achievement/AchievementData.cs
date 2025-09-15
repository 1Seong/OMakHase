using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementData", menuName = "Scriptable Objects/AchievementData")]
public class AchievementData : ScriptableObject
{
    // 업적명
    [TextArea]
    public string achievementTitle;
    // 업적 설명
    [TextArea]
    public string achievementDescription;

    // 보상 데이터 - 추후 확장을 위해 남겨둠
    // public RewardData reward;

}
