using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    //public TextAsset CSV;

    public Dialogue[] Parse(TextAsset CSV)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); // 대화 리스트 생성.
        TextAsset csvData = CSV; // csv 파일 가져옴
        //Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[] { '\n' }); // 엔터 기준으로 쪼갬(스프레드시트의 행별로 쪼갬)
        
        for (int i = 1; i < data.Length;)
        {
            //Debug.Log(data[i]);
            string[] row = data[i].Split(new char[] { ',' });

            
            Dialogue dialogue = new Dialogue(); // 대사 데이터 생성

            
            dialogue.dialogueID = row[0]; // 대사 ID
            //Debug.Log(row[0]);
            dialogue.nextDialogueID = row[1]; // 다음 대사 ID
            //Debug.Log(row[1]);
            dialogue.name = row[2]; // 현재 대사치는 인물의 이름
            //Debug.Log(row[2]);
            dialogue.contexts = row[3]; // 현재 대사
            //Debug.Log(row[3]);
            dialogue.spriteID = row[4]; // 캐릭터 스프라이트 ID
            //Debug.Log(row[4]);
            dialogue.type = row[5]; // 대화 유형
            //Debug.Log(row[5]);
            dialogue.desireMain = row[6]; // 요구하는 주재료
            //Debug.Log(row[6]);
            dialogue.desireCategory = row[7]; // 요구하는 카테고리
            //Debug.Log(row[7]);
            

            dialogueList.Add(dialogue);

        }
        
        return dialogueList.ToArray();

    }

}
