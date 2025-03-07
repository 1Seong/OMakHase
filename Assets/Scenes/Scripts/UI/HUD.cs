using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public enum InfoType { Day, Reputation, Money, ReputationRising }
    public InfoType type;

    TextMeshProUGUI myText;

    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Day:
                myText.text = string.Format("Day {0:F0}", GameManager.instance.day);
                break;
            case InfoType.Reputation:
                myText.text = string.Format("{0:F0}", GameManager.instance.reputation);
                break;
            case InfoType.Money:

                break;
            case InfoType.ReputationRising:
                myText.text = string.Format("평판이 {0:F0} 증가했습니다. ->", CookManager.instance.ReputationRise);
                break;

        }
    }
}
