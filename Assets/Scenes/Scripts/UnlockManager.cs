using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager instance;

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

    }

    public void Unlock(Ingredient.Cook cook)
    {

    }

    public void Unlock(Ingredient.MeatFish meatFish)
    {

    }

    public void Unlock(Ingredient.Vege vege)
    {

    }


}
