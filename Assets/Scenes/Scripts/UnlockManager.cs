using System;
using System.Collections.Generic;
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

    [SerializeField]
    private List<Button> _baseButtons; // Last button is 'next' button
    [SerializeField]
    private List<Button> _cookButtons; // Last two buttons are buttons that change screen
    [SerializeField]
    private List<Button> _meatFishButtons;
    [SerializeField]
    private List<Button> _vegeButtons;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        foreach(RectTransform child in _baseButtonParent.transform) {
            _baseButtons.Add(child.gameObject.GetComponent<Button>());
        }
        _baseButtons.RemoveAt(_baseButtons.Count-1);

        foreach (RectTransform child in _cookButtonParent.transform)
        {
            _cookButtons.Add(child.gameObject.GetComponent<Button>());
        }
        _cookButtons.RemoveAt(_cookButtons.Count - 1);
        _cookButtons.RemoveAt(_cookButtons.Count - 1);

        foreach (RectTransform child in _meatFishParent.transform)
        {
            _meatFishButtons.Add(child.gameObject.GetComponent<Button>());
        }

        foreach (RectTransform child in _vegeParent.transform)
        {
            _vegeButtons.Add(child.gameObject.GetComponent<Button>());
        }


        // 비활성화된 오브젝트 접근 불가 코드
        //_baseButtons = _baseButtonParent.GetComponentsInChildren<Button>();
        //_cookButtons = _cookButtonParent.GetComponentsInChildren<Button>();
        //_meatFishButtons = _meatFishParent.GetComponentsInChildren <Button>();
        //_vegeButtons = _vegeParent.GetComponentsInChildren<Button>();
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
        UnlockButton(baseIngred);
        OnUnlock?.Invoke();
    }

    public void Unlock(Ingredient.Cook cook)
    {
        PlayerPrefs.SetInt(cook.ToString(), 1);
        UnlockButton(cook);
        OnUnlock?.Invoke();
    }

    public void Unlock(Ingredient.MeatFish meatFish)
    {
        PlayerPrefs.SetInt(meatFish.ToString(), 1);
        UnlockButton(meatFish);
        OnUnlock?.Invoke();
    }

    public void Unlock(Ingredient.Vege vege)
    {
        PlayerPrefs.SetInt(vege.ToString(), 1);
        UnlockButton(vege);
        OnUnlock?.Invoke();
    }

    private void UnlockButton(Ingredient.Base baseIngred)
    {
        _baseButtons[(int)baseIngred-1].gameObject.SetActive(true);
    }

    private void UnlockButton(Ingredient.Cook cook)
    {
        _cookButtons[(int)cook - 1].gameObject.SetActive(true);
    }

    private void UnlockButton(Ingredient.MeatFish meatFish)
    {
        _meatFishButtons[(int)meatFish-1].gameObject.SetActive(true);
    }

    private void UnlockButton(Ingredient.Vege vege)
    {
        _vegeButtons[(int)vege-1].gameObject.SetActive(true);
    }

    public bool IsUnlocked<T>(T item)
    {
        return PlayerPrefs.HasKey(item.ToString()) && PlayerPrefs.GetInt(item.ToString()) == 1;
    }
}

