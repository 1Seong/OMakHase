using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager instance;

    public event Action<int, int> ButtonUnlockAction; // < child id, mode >
    public event Action OnUnlockAction;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

    }

    // Add this method to game start(Load game) button 
    public void InitButtons()
    {
        foreach (Ingredient.Base i in Enum.GetValues(typeof(Ingredient.Base)))
            if (IsUnlocked(i))
            {
                ButtonUnlockAction?.Invoke((int)i - 1, 0);
                
            }

        foreach (Ingredient.Cook i in Enum.GetValues(typeof(Ingredient.Cook)))
            if (IsUnlocked(i))
            {
                ButtonUnlockAction?.Invoke((int)i - 1, 1);
            }

        foreach (Ingredient.MeatFish i in Enum.GetValues(typeof(Ingredient.MeatFish)))
            if (IsUnlocked(i))
            {
                ButtonUnlockAction?.Invoke((int)i - 1, 2);
            }

        foreach (Ingredient.Vege i in Enum.GetValues(typeof(Ingredient.Vege)))
            if (IsUnlocked(i))
            {
                ButtonUnlockAction?.Invoke((int)i - 1, 3);
            }
              
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Unlock(Ingredient.Base baseIngred)
    {
        PlayerPrefs.SetInt(baseIngred.ToString(), 1);
        ButtonUnlockAction?.Invoke((int)baseIngred - 1, 0);
        OnUnlockAction?.Invoke();
    }

    public void Unlock(Ingredient.Cook cook)
    {
        PlayerPrefs.SetInt(cook.ToString(), 1);
        ButtonUnlockAction?.Invoke((int)cook - 1, 1);
        OnUnlockAction?.Invoke();
    }

    public void Unlock(Ingredient.MeatFish meatFish)
    {
        PlayerPrefs.SetInt(meatFish.ToString(), 1);
        ButtonUnlockAction?.Invoke((int)meatFish - 1, 2);
        OnUnlockAction?.Invoke();
    }

    public void Unlock(Ingredient.Vege vege)
    {
        PlayerPrefs.SetInt(vege.ToString(), 1);
        ButtonUnlockAction?.Invoke((int)vege - 1, 3);
        OnUnlockAction?.Invoke();
    }

    public bool IsUnlocked<T>(T item)
    {
        return PlayerPrefs.HasKey(item.ToString()) && PlayerPrefs.GetInt(item.ToString()) == 1;
    }
}

