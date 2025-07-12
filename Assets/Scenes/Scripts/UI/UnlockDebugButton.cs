using UnityEngine;

public class UnlockDebugButton : MonoBehaviour
{
    public enum Mode { Base, Cook, MeatFish, Vege }

    public Mode mode;

    public void OnClick()
    {
        switch (mode)
        {
            case Mode.Base:
                UnlockManager.instance.Unlock(Ingredient.Base.noodle);
                break;
            case Mode.Cook:
                UnlockManager.instance.Unlock(Ingredient.Cook.stirFry);
                break;
            case Mode.MeatFish:
                UnlockManager.instance.Unlock(Ingredient.MeatFish.beef);
                break;
            case Mode.Vege:
                UnlockManager.instance.Unlock(Ingredient.Vege.carrot);
                break;
        }
    }
}
