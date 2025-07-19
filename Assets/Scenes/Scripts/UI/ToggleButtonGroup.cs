using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonGroup : MonoBehaviour
{
    public Button[] buttons;
    protected Button activeButton = null;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    private void Start()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => ToggleButton(btn));
        }
    }

    private void ToggleButton(Button clickedButton)
    {
        if (activeButton != null)
            ReleaseBehavior();

        ButtonSelectedBehavior(clickedButton);
        activeButton = clickedButton;
    }

    // Template method pattern

    protected virtual void ReleaseBehavior()
    {
        activeButton.interactable = true;
    }

    protected virtual void ButtonSelectedBehavior(Button clickedButton)
    {
        clickedButton.interactable = false;
    }
}