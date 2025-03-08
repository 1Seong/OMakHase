using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] // 커스텀 클래스를 인스펙터창에서 수정하기 위해서 필요
public class Dialogue
{
    [Tooltip("대사 ID")]
    public string dialogueID;

    [Tooltip("다음 대사 ID")]
    public string nextDialogueID;

    [Tooltip("발화자 이름")]
    public string name;

    [Tooltip("텍스트")]
    public string line;

    [Tooltip("캐릭터 스프라이트 ID")]
    public string spriteID;

    [Tooltip("대화 유형")]
    public string type;

    [Tooltip("요구하는 주재료")]
    public string desireMain;

    [Tooltip("요구하는 카테고리")]
    public string desireCategory;
}

[System.Serializable] // 커스텀 클래스를 인스펙터창에서 수정하기 위해서 필요
public class RandomDialogue {

    [Tooltip("텍스트")]
    public string line;

    [Tooltip("대화 유형")]
    public string type;

    [Tooltip("요구하는 주재료")]
    public string desireMain;

    [Tooltip("요구하는 카테고리")]
    public string desireCategory;

    [Tooltip("N day 후")]
    public string afterDayN;

    [Tooltip("포맷화 여부")]
    public string isFormat;

    [Tooltip("캐릭터 스프라이트 ID")]
    public string spriteID;
}

[System.Serializable]
public class DialogueEvent
{

    public string name;

    public Vector2 line; // 가져올 대사 start, end 인덱스
    public Dialogue[] dialogues;
}
