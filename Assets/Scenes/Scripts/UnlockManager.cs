using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    private static UnlockManager _instance;

    // controllers for base, ingredient, and cook buttons
    [SerializeField] private List<UnlockButtonController> unlockButtonControllers;

    public event Action<int, int> ButtonUnlockAction; // < child id, mode >
                                                      // mode - base : 0, cook : 1, MeatFish : 2, Vege : 3
    public event Action OnUnlockAction; // for the actions when unlocked for the first time
    public event Action ClearAction;

    public static UnlockManager instance
    {
        get
        {
            if (_instance == null)
            {
                // find in scene
                _instance = FindFirstObjectByType<UnlockManager>();

            }

            return _instance;
        }
    }
   
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitControllers();
    }

    // initialize button controllers - had to do in here becuase of delayed instantiation (moved from UnlockButtonController)
    public void InitControllers()
    {
        foreach(var con in unlockButtonControllers)
        {
            ButtonUnlockAction += con.UnlockButton;
            ClearAction += con.ClearButton;
        }
    }

    // default unlocks
    private void InitUnlocks()
    {
        //UnlockManager.instance.Unlock(Ingredient.Base.noCondition);
        Unlock(Ingredient.Base.rice);
        Unlock(Ingredient.Base.bread);

        Unlock(Ingredient.Cook.none);
        Unlock(Ingredient.Cook.stirFry);
        Unlock(Ingredient.Cook.roast);

        //UnlockManager.instance.Unlock(Ingredient.MeatFish.noCondition);
        Unlock(Ingredient.MeatFish.none);
        Unlock(Ingredient.MeatFish.pork);
        Unlock(Ingredient.MeatFish.tuna);

        //UnlockManager.instance.Unlock(Ingredient.Vege.noCondition);
        Unlock(Ingredient.Vege.none);
        Unlock(Ingredient.Vege.potato);
        Unlock(Ingredient.Vege.tomato);
    }

    // reset data (full reset)
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        ClearAction?.Invoke();
        InitUnlocks();
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

    // data load
    public void Load()
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
    

}

