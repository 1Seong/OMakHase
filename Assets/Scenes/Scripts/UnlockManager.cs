using System;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager instance;

    public event Action OnUnlock;

    // 아래 버튼들 따로 컴포넌트에 분리해서 버튼 부모에 직접 부착하기
    [SerializeField] private GameObject _baseButtonParent;
    [SerializeField] private GameObject _cookButtonParent;
    [SerializeField] private GameObject _meatFishParent;
    [SerializeField] private GameObject _vegeParent;

    private Button[] _baseButtons; // Last button is 'next' button
    private Button[] _cookButtons; // Last two buttons are buttons that change screen
    private Button[] _meatFishButtons;
    private Button[] _vegeButtons;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        _baseButtons = _baseButtonParent.GetComponentsInChildren<Button>();
        _cookButtons = _cookButtonParent.GetComponentsInChildren<Button>();
        _meatFishButtons = _meatFishParent.GetComponentsInChildren <Button>();
        _vegeButtons = _vegeParent.GetComponentsInChildren<Button>();
    }

    // Add this method to game start(Load game) button 
    public void InitButtons()
    {
        foreach (Ingredient.Base i in Enum.GetValues(typeof(Ingredient.Base)))
            if (IsUnlocked(i))
                OnUnlock.Invoke();

        foreach (Ingredient.Cook i in Enum.GetValues(typeof(Ingredient.Cook)))
            if (IsUnlocked(i))
                OnUnlock.Invoke();

        foreach (Ingredient.MeatFish i in Enum.GetValues(typeof(Ingredient.MeatFish)))
            if (IsUnlocked(i))
                OnUnlock.Invoke();

        foreach (Ingredient.Vege i in Enum.GetValues(typeof(Ingredient.Vege)))
            if (IsUnlocked(i))
                OnUnlock.Invoke();
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Unlock(Ingredient.Base baseIngred)
    {
        PlayerPrefs.SetInt(baseIngred.ToString(), 1);
        OnUnlock.Invoke();
    }

    public void Unlock(Ingredient.Cook cook)
    {
        PlayerPrefs.SetInt(cook.ToString(), 1);
        OnUnlock.Invoke();
    }

    public void Unlock(Ingredient.MeatFish meatFish)
    {
        PlayerPrefs.SetInt(meatFish.ToString(), 1);
        OnUnlock.Invoke();
    }

    public void Unlock(Ingredient.Vege vege)
    {
        PlayerPrefs.SetInt(vege.ToString(), 1);
        OnUnlock.Invoke();
    }

    private void UnlockButton(Ingredient.Base baseIngred)
    {
        _baseButtons[(int)baseIngred].gameObject.SetActive(true);
    }

    private void UnlockButton(Ingredient.Cook cook)
    {
        _cookButtons[(int)cook].gameObject.SetActive(true);
    }

    private void UnlockButton(Ingredient.MeatFish meatFish)
    {
        _meatFishButtons[(int)meatFish].gameObject.SetActive(true);
    }

    private void UnlockButton(Ingredient.Vege vege)
    {
        _cookButtons[(int)vege].gameObject.SetActive(true);
    }

    private bool IsUnlocked<T>(T item)
    {
        return PlayerPrefs.HasKey(item.ToString()) && PlayerPrefs.GetInt(item.ToString()) == 1;
    }
}
