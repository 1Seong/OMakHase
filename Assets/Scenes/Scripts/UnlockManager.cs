using System;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager instance;

    public event Action OnUnlock;

    [SerializeField] private GameObject _baseButtonParent;
    [SerializeField] private GameObject _cookButtonParent;
    [SerializeField] private GameObject _meatFishParent;
    [SerializeField] private GameObject _vegeParent;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void Unlock(Ingredient.Base baseIngred)
    {
        PlayerPrefs.SetInt(baseIngred.ToString(), 1);
    }

    public void Unlock(Ingredient.Cook cook)
    {
        PlayerPrefs.SetInt(cook.ToString(), 1);
    }

    public void Unlock(Ingredient.MeatFish meatFish)
    {
        PlayerPrefs.SetInt(meatFish.ToString(), 1);
    }

    public void Unlock(Ingredient.Vege vege)
    {
        PlayerPrefs.SetInt(vege.ToString(), 1);
    }

    public bool IsUnlocked(Ingredient.Base baseIngred)
    {
        return PlayerPrefs.HasKey(baseIngred.ToString()) && PlayerPrefs.GetInt(baseIngred.ToString()) == 1;
    }

    public bool IsUnlocked(Ingredient.Cook cook)
    {
        return PlayerPrefs.HasKey(cook.ToString()) && PlayerPrefs.GetInt(cook.ToString()) == 1;
    }

    public bool IsUnlocked(Ingredient.MeatFish meatFish)
    {
        return PlayerPrefs.HasKey(meatFish.ToString()) && PlayerPrefs.GetInt(meatFish.ToString()) == 1;
    }

    public bool IsUnlocked(Ingredient.Vege vege)
    {
        return PlayerPrefs.HasKey(vege.ToString()) && PlayerPrefs.GetInt(vege.ToString()) == 1;
    }
}
