using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] // Ŀ���� Ŭ������ �ν�����â���� �����ϱ� ���ؼ� �ʿ�
public class Dialogue
{
    [Tooltip("��� ID")]
    public string dialogueID;

    [Tooltip("���� ��� ID")]
    public string nextDialogueID;

    [Tooltip("��ȭ�� �̸�")]
    public string name;

    [Tooltip("�ؽ�Ʈ")]
    public string line;

    [Tooltip("ĳ���� ��������Ʈ ID")]
    public string spriteID;

    [Tooltip("��ȭ ����")]
    public string type;

    [Tooltip("�䱸�ϴ� �����")]
    public string desireMain;

    [Tooltip("�䱸�ϴ� ī�װ�")]
    public string desireCategory;
}

[System.Serializable] // Ŀ���� Ŭ������ �ν�����â���� �����ϱ� ���ؼ� �ʿ�
public class RandomDialogue {

    [Tooltip("�ؽ�Ʈ")]
    public string line;

    [Tooltip("��ȭ ����")]
    public string type;

    [Tooltip("�䱸�ϴ� �����")]
    public string desireMain;

    [Tooltip("�䱸�ϴ� ī�װ�")]
    public string desireCategory;

    [Tooltip("N day ��")]
    public string afterDayN;

    [Tooltip("����ȭ ����")]
    public string isFormat;

    [Tooltip("ĳ���� ��������Ʈ ID")]
    public string spriteID;
}

[System.Serializable]
public class DialogueEvent
{

    public string name;

    public Vector2 line; // ������ ��� start, end �ε���
    public Dialogue[] dialogues;
}
