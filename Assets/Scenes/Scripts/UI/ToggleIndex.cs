using UnityEngine;
using UnityEngine.UI;

public class ToggleIndex : ToggleButtonGroup
{

    protected override void ReleaseBehavior()
    {
        activeButton.interactable = true;

        //activeButton.transform.Translate(-0.2f, 0f, 0f);
        //activeButton.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    protected override void ButtonSelectedBehavior(Button clickedButton)
    {
        clickedButton.interactable = false;

        //clickedButton.transform.localScale = new Vector3(1.2f, 1f, 1f);
        //clickedButton.transform.Translate(0.2f, 0f, 0f);
    }
}
