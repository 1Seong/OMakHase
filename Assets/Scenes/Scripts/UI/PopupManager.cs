using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject _popUpUI;
    public void OnClickBase()
    {
        if (CookManager.instance.baseIngred != Ingredient.Base.noCondition)
            return;

        var popup = Instantiate(_popUpUI, transform).GetComponent<Popup>();
        popup.Init(0);
    }

    public void OnClickMain()
    {
        if (CookManager.instance.meatfish != Ingredient.MeatFish.none || CookManager.instance.vege != Ingredient.Vege.none)
            return;

        var popup = Instantiate(_popUpUI, transform).GetComponent<Popup>();
        popup.Init(1);
    }

    public void OnClickCook()
    {
        if (CookManager.instance.cook != Ingredient.Cook.noCondition)
            return;

        var popup = Instantiate(_popUpUI, transform).GetComponent<Popup>();
        popup.Init(2);
    }
}
