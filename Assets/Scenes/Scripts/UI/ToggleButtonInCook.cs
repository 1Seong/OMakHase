using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonInCook : ToggleButtonGroup
{
    protected override void Awake()
    {
        
    }

    protected override void ReleaseBehavior()
    {
        activeButton.GetComponent<ButtonSprites>().OnDeselected();
    }

    protected override void ButtonSelectedBehavior(Button clickedButton)
    {
        clickedButton.GetComponent<ButtonSprites>().OnSelected();
    }
}
