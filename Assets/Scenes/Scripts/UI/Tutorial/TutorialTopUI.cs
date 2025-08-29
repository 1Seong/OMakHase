using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurotialTopUI : MonoBehaviour
{
    private Button button;
    [SerializeField] private TextMeshProUGUI TargetDialogue;
    [SerializeField] private Button SkipButton;
    [SerializeField] private Button TutorialStartButton;
    [SerializeField] private string keyword = "이제 본격적으로 시작할 차례야.";


    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        ResetTutorial();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Tutorial") && PlayerPrefs.GetInt("Tutorial") == 1)
        {
            //Debug.Log("Tutorial Top UI : Already has PlayerPref");
            return;
        }

        else
        {
            //Debug.Log("Tutorial Top UI : Start function");
            button.onClick.AddListener(CheckTutorialCondition);
        }
    }

    private void CheckTutorialCondition()
    {
        if (GameManager.instance.TutorialActive && checkKeyWordInDialogue())
        {
            Debug.Log("Tutorial Button Active");
            //SkipButton.gameObject.SetActive(false);
            TutorialStartButton.gameObject.SetActive(true);
        }
        //else
            //Debug.Log("Tutorial Button InActive " + GameManager.instance.TutorialActive.ToString());
    }

    private bool checkKeyWordInDialogue()
    {
        var text = TargetDialogue.text;
        //Debug.Log(text);
        if (text.Contains(keyword))
            return true;
        else
            return false;
    }

    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("Tutorial");
    }
}
