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
        
        for (int i = 1; i < data.Length; i++)
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
            dialogue.line = row[3]; // 현재 대사
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

    public RandomDialogue[] RandomParse(TextAsset CSV)
    {
        List<RandomDialogue> dialogueList = new List<RandomDialogue>(); // 대화 리스트 생성.
        TextAsset csvData = CSV; // csv 파일 가져옴
        //Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[] { '\n' }); // 엔터 기준으로 쪼갬(스프레드시트의 행별로 쪼갬)

        for (int i = 1; i < data.Length; i++)
        {
            //Debug.Log(data[i]);
            string[] row = data[i].Split(new char[] { ',' });


            RandomDialogue dialogue = new RandomDialogue(); // 대사 데이터 생성


            dialogue.line = row[0]; // 텍스트
            //Debug.Log(row[0]);
            dialogue.type = row[1]; // 대화 유형
            //Debug.Log(row[1]);
            dialogue.desireMain = row[2]; // 요구하는 주재료
            //Debug.Log(row[2]);
            dialogue.desireCategory = row[3]; // 요구하는 카테고리
            //Debug.Log(row[3]);
            dialogue.afterDayN = row[4]; // N day 후
            //Debug.Log(row[4]);
            dialogue.isFormat = row[5]; // 포맷화 여부
            //Debug.Log(row[5]);
            //dialogue.spriteID = row[6]; // 캐릭터 스프라이트 ID
            //Debug.Log(row[6]);


            dialogueList.Add(dialogue);

        }

        return dialogueList.ToArray();

    }


    public RandomReactionDialogue[][] RandomReactionParse(TextAsset CSV)
    {
        List<RandomReactionDialogue[]> dialogueList = new List<RandomReactionDialogue[]>(); // 대화 리스트 생성.

        List<RandomReactionDialogue> positiveReactionList = new List<RandomReactionDialogue>(); // 긍정 반응 리스트 생성.
        List<RandomReactionDialogue> neutralReactionList = new List<RandomReactionDialogue>(); // 긍정 반응 리스트 생성.
        List<RandomReactionDialogue> negativeReactionList = new List<RandomReactionDialogue>(); // 긍정 반응 리스트 생성.

        TextAsset csvData = CSV; // csv 파일 가져옴
        //Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[] { '\n' }); // 엔터 기준으로 쪼갬(스프레드시트의 행별로 쪼갬)

        for (int i = 1; i < data.Length; i++)
        {
            //Debug.Log(data[i]);
            string[] row = data[i].Split(new char[] { ',' });


            RandomReactionDialogue dialogue = new RandomReactionDialogue(); // 대사 데이터 생성


            dialogue.line = row[0]; // 텍스트
            //Debug.Log(row[0]);
            dialogue.type = row[1]; // 대화 유형
            //Debug.Log(row[1]);
            dialogue.desireBase = row[2]; // 요구하는 베이스
            //Debug.Log(row[2]);
            dialogue.desireMain = row[3]; // 요구하는 주재료
            //Debug.Log(row[3]);
            dialogue.desireCook = row[4]; // 조리법
            //Debug.Log(row[4]);
            dialogue.isFormat = row[5]; // 포맷화 여부
            //Debug.Log(row[5]);

            if(dialogue.type == "positive")
                positiveReactionList.Add(dialogue);
            else if (dialogue.type == "neutral")
                neutralReactionList.Add(dialogue);
            else
                negativeReactionList.Add(dialogue);

        }

        dialogueList.Add(positiveReactionList.ToArray());
        dialogueList.Add(neutralReactionList.ToArray());
        dialogueList.Add(negativeReactionList.ToArray());
        return dialogueList.ToArray();

    }


    public EndingDialogue[] EndingParse(TextAsset CSV)
    {
        List<EndingDialogue> dialogueList = new List<EndingDialogue>(); // 대화 리스트 생성.
        TextAsset csvData = CSV; // csv 파일 가져옴
        //Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[] { '\n' }); // 엔터 기준으로 쪼갬(스프레드시트의 행별로 쪼갬)

        for (int i = 1; i < data.Length; i++)
        {
            //Debug.Log(data[i]);
            string[] row = data[i].Split(new char[] { ',' });


            EndingDialogue dialogue = new EndingDialogue(); // 대사 데이터 생성


            dialogue.dialogueID = row[0]; // 대사 ID
            //Debug.Log(row[0]);
            dialogue.nextDialogueID = row[1]; // 다음 대사 ID
            //Debug.Log(row[1]);
            dialogue.name = row[2]; // 현재 대사치는 인물의 이름
            //Debug.Log(row[2]);
            dialogue.line = row[3]; // 현재 대사
            //Debug.Log(row[3]);
            dialogue.spriteID = row[4]; // 캐릭터 스프라이트 ID
            //Debug.Log(row[4]);
            dialogue.directing = row[5]; // 연출
            //Debug.Log(row[5]);
            dialogue.background = row[6]; // 배경
            //Debug.Log(row[6]);


            dialogueList.Add(dialogue);

        }

        return dialogueList.ToArray();

    }
}
