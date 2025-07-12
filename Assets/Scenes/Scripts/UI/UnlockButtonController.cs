using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UnlockButtonController : MonoBehaviour
{
    public enum Mode { Base, Cook, MeatFish, Vege }

    public Mode mode;

    

    public void UnlockButton(int i, int m)
    {
        //Debug.Log("UnlockButton invoked");
        if ((int)mode != m) return;

        var child = transform.GetChild(i);
        child.gameObject.SetActive(true);
    }

    public void ClearButton()
    {
        var count = GetButtonCount();

        for(int i = 0; i != count; ++i)
        {
            var child = transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }

    protected virtual int GetButtonCount()
    {
        var count = mode switch
        {
            Mode.Base => transform.childCount - 1,
            Mode.Cook => transform.childCount - 2,
            Mode.MeatFish => transform.childCount,
            Mode.Vege => transform.childCount,
            _ => throw new System.NotImplementedException()
        };

        return count;
    }

    private void OnDestroy()
    {
        if (UnlockManager.instance == null) return;
        UnlockManager.instance.ButtonUnlockAction -= UnlockButton;
        UnlockManager.instance.ClearAction -= ClearButton;
    }


    // moved to UnlockManager due to delayed instantiation ;(
    /*
    private void Awake()
    {
        
        UnlockManager.instance.ButtonUnlockAction += UnlockButton;
        Debug.Log("UnlockButtonController - Awake");

        transform.root.gameObject.SetActive(false);
        
    }
    */
}
