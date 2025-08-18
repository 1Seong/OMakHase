using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonInCook : ToggleButtonGroup
{

    protected override void ReleaseBehavior()
    {
        if(ActiveButton.GetComponent<ButtonSprites>() is not null)
            ActiveButton.GetComponent<ButtonSprites>().OnDeselected();
        else if(ActiveButton.GetComponent<ButtonSpritesMultiple>() is not null)
            ActiveButton.GetComponent<ButtonSpritesMultiple>().OnDeselected();
    }

    protected override void ButtonSelectedBehavior(Button clickedButton)
    {
        if (clickedButton.GetComponent<ButtonSprites>() is not null)
            clickedButton.GetComponent<ButtonSprites>().OnSelected();
        else if (clickedButton.GetComponent<ButtonSpritesMultiple>() is not null)
            clickedButton.GetComponent<ButtonSpritesMultiple>().OnSelected();
    }

    
}
