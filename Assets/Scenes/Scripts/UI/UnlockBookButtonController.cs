using UnityEngine;
using UnityEngine.UI;

public class UnlockBookButtonController : UnlockButtonController
{
    private void Awake()
    {
        UnlockManager.instance.ButtonUnlockAction += UnlockButton;
        UnlockManager.instance.ClearAction += ClearButton;
        //Debug.Log("UnlockButtonController - Awake");

    }

    protected override int GetButtonCount()
    {
        return transform.childCount;
    }
}
