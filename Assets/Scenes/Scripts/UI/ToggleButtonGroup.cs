using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleButtonGroup : MonoBehaviour
{
    public UnityEvent ActiveButtonSetUnityEvent;
    public UnityEvent ActiveButtonNoneUnityEvent;

    public Button[] buttons;
    private Button _activeButton = null;
    protected Button ActiveButton
    {
        get => _activeButton;
        set
        {
            _activeButton = value;
            if(value is null)
            {
                ActiveButtonNoneUnityEvent?.Invoke();
            }
            else
            {
                ActiveButtonSetUnityEvent?.Invoke(); 
            }
        }
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
        if (ActiveButton != null)
            ReleaseBehavior();

        ButtonSelectedBehavior(clickedButton);
        ActiveButton = clickedButton;
    }

    // Template method pattern

    protected virtual void ReleaseBehavior()
    {
        ActiveButton.interactable = true;
    }

    protected virtual void ButtonSelectedBehavior(Button clickedButton)
    {
        clickedButton.interactable = false;
    }

    public virtual void Clear()
    {
        if (ActiveButton == null)
            return;

        ReleaseBehavior();

        ActiveButton = null;
    }
}