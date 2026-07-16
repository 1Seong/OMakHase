using System;
using UnityEngine;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    [SerializeField] private GameObject[] otherObjects;
    private Button[] otherButtons;
    [SerializeField] private Button skipButton;
    [SerializeField] private GameObject fadePanel;

    private void Awake()
    {
        otherButtons = new Button[otherObjects.Length];
        for (int i = 0; i < otherObjects.Length; ++i)
        {
            otherButtons[i] = otherObjects[i].GetComponent<Button>();
        }
    }

    private void Update()
    {
        if (fadePanel.activeSelf) return;
        
        if (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return))
        {
            return;
        }

        for (int i = 0; i < otherObjects.Length; ++i)
        {
            if (!otherObjects[i].activeSelf)
            {
                continue;
            }

            otherButtons[i].onClick.Invoke();
            return;
        }
        skipButton.onClick.Invoke();
    }
}
