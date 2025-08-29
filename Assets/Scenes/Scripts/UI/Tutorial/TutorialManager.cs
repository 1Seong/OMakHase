using UnityEngine;
using UnityEngine.UI;
using static Ingredient;
using static UnityEditor.Progress;

public class TutorialManager : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Tutorial") && PlayerPrefs.GetInt("Turoail") == 1)
            return;
        else
            button.onClick.AddListener(CheckTutorialCondition);
    }

    public void CheckTutorialCondition()
    {
        //Debug
        ResetTutorial();

        if (PlayerPrefs.HasKey("Tutorial") && PlayerPrefs.GetInt("Turoail") == 1)
            button.onClick.RemoveListener(CheckTutorialCondition);
        else
            startTutorial();
    }

    private void startTutorial()
    {
        //PlayerPrefs.SetInt("Tutorial", 1);
        GameManager.instance.TutorialActive = true;
    }


    // Debug Functions

    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("Tutorial");
    }
}
