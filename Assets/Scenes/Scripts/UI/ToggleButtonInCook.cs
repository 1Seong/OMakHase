using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonInCook : ToggleButtonGroup
{

    protected override void ReleaseBehavior()
    {
        if(activeButton.GetComponent<ButtonSprites>() is not null)
            activeButton.GetComponent<ButtonSprites>().OnDeselected();
        else if(activeButton.GetComponent<ButtonSpritesMultiple>() is not null)
            activeButton.GetComponent<ButtonSpritesMultiple>().OnDeselected();
    }

    protected override void ButtonSelectedBehavior(Button clickedButton)
    {
        if (clickedButton.GetComponent<ButtonSprites>() is not null)
            clickedButton.GetComponent<ButtonSprites>().OnSelected();
        else if (clickedButton.GetComponent<ButtonSpritesMultiple>() is not null)
            clickedButton.GetComponent<ButtonSpritesMultiple>().OnSelected();
    }

    
}
