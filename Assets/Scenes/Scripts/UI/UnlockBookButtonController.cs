using UnityEngine;
using UnityEngine.UI;

public class UnlockBookButtonController : UnlockButtonController
{
    /*
    private void Awake()
    {
        UnlockManager.ButtonUnlockAction += UnlockButton;
        UnlockManager.ClearAction += ClearButton;
        //Debug.Log("UnlockButtonController - Awake");

    }
    */

    protected override int GetButtonCount()
    {
        return transform.childCount;
    }
}
