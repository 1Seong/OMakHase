using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UnlockButtonController : MonoBehaviour
{
    public enum Mode { Base, Cook, MeatFish, Vege }

    public Mode mode;

    // moved to UnlockManager due to delayed instantiation ;(
    /*
    private void Awake()
    {
        
        UnlockManager.instance.ButtonUnlockAction += UnlockButton;
        Debug.Log("UnlockButtonController - Awake");

        transform.root.gameObject.SetActive(false);
        
    }
    */

    public void UnlockButton(int i, int m)
    {
        Debug.Log("UnlockButton invoked");
        if ((int)mode != m) return;

        var child = transform.GetChild(i);
        child.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        UnlockManager.instance.ButtonUnlockAction -= UnlockButton;
    }
}
