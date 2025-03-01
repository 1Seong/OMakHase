using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    //public TextAsset CSV;

    public Dialogue[] Parse(TextAsset CSV)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); // ��ȭ ����Ʈ ����.
        TextAsset csvData = CSV; // csv ���� ������
        //Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[] { '\n' }); // ���� �������� �ɰ�(���������Ʈ�� �ະ�� �ɰ�)
        
        for (int i = 1; i < data.Length;)
        {
            //Debug.Log(data[i]);
            string[] row = data[i].Split(new char[] { ',' });

            
            Dialogue dialogue = new Dialogue(); // ��� ������ ����

            
            dialogue.dialogueID = row[0]; // ��� ID
            //Debug.Log(row[0]);
            dialogue.nextDialogueID = row[1]; // ���� ��� ID
            //Debug.Log(row[1]);
            dialogue.name = row[2]; // ���� ���ġ�� �ι��� �̸�
            //Debug.Log(row[2]);
            dialogue.contexts = row[3]; // ���� ���
            //Debug.Log(row[3]);
            dialogue.spriteID = row[4]; // ĳ���� ��������Ʈ ID
            //Debug.Log(row[4]);
            dialogue.type = row[5]; // ��ȭ ����
            //Debug.Log(row[5]);
            dialogue.desireMain = row[6]; // �䱸�ϴ� �����
            //Debug.Log(row[6]);
            dialogue.desireCategory = row[7]; // �䱸�ϴ� ī�װ�
            //Debug.Log(row[7]);
            

            dialogueList.Add(dialogue);

        }
        
        return dialogueList.ToArray();

    }

}
