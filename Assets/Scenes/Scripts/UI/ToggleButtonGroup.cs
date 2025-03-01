using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonGroup : MonoBehaviour
{
    public Button[] buttons;
    private Button activeButton = null;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    void Start()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => ToggleButton(btn));
        }
    }

    void ToggleButton(Button clickedButton)
    {
        if (activeButton != null)
            activeButton.interactable = true;

        clickedButton.interactable = false;
        activeButton = clickedButton;
    }
}