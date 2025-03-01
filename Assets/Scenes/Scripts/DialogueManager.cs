using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] TextAsset StoryCSV;
    //string csv_FileName;

    [SerializeField] TextMeshProUGUI nameUI;
    [SerializeField] TextMeshProUGUI dialogueUI;


    Dictionary<string, Dialogue> dialogueDic = new Dictionary<string, Dialogue>();

    public static bool isFinish = false;

    [SerializeField]
    private int _Day;
    public int Day { get { return _Day; } }

    [SerializeField]
    private int _Customer;
    public int Customer { get { return _Customer; } }

    [SerializeField]
    private string _Sequence;
    public string Sequence { get { return _Sequence; } }

    [SerializeField]
    private string _Multiple;
    public string Multiple { get { return _Multiple; } }

    [SerializeField] 
    private string _currentID;
    public string currentID { get { return _currentID; } }

    [SerializeField]
    private string _currentDialogue;
    public string currentDialogue {  get { return _currentDialogue; } }

    private void Awake()
    {
        Instance = this;

        _Day = 0;
        _Customer = 1;
        _Sequence = string.Format("{0:D2}", 1);

        _currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", _Customer) + "_" + _Sequence + "_";

        DialogueParser theParser = gameObject.GetComponent<DialogueParser>();
        
        Dialogue[] dialogues = theParser.Parse(StoryCSV);

        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogueDic.Add(dialogues[i].dialogueID, dialogues[i]);
        }
        isFinish = true;

        nameUI.text = dialogueDic[currentID].name;
        dialogueUI.text = dialogueDic[currentID].line.Replace('`', ','); ;
        Debug.Log(dialogueDic[currentID].line);
        
    }

    public string GetNextDialogue()
    {

        if (dialogueDic[currentID].nextDialogueID == "EOD") 
        {

            string[] tmp = _currentID.Split(new char[] { '_' });

            _Day = int.Parse(tmp[0].Substring(1)) + 1;

            _currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", 1) + "_" + "01" + "_";

        }

        else if (dialogueDic[currentID].nextDialogueID == "EOO")
        {
            string[] tmp = _currentID.Split(new char[] { '_' });

            _Customer = int.Parse(tmp[1].Substring(1)) + 1;

            _currentID = "D" + string.Format("{0:D2}", 1) + "_C" + string.Format("{0:D2}", _Customer) + "_" + "01" + "_";
        }

        else
            _currentID = dialogueDic[currentID].nextDialogueID;

        nameUI.text = dialogueDic[currentID].name;
        dialogueUI.text = dialogueDic[currentID].line.Replace('`', ',');
        Debug.Log(dialogueDic[currentID].line);
        return dialogueDic[currentID].line;

    }
}
