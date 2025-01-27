using UnityEngine;

[CreateAssetMenu(fileName = "CategoryData", menuName = "Scriptable Objects/CategoryData")]
public class CategoryData : ScriptableObject
{
    [SerializeField]
    private Ingredient.Base _baseIngred;
    public Ingredient.Base baseIngred { get => _baseIngred; }

    [SerializeField]
    private Ingredient.Cook _cook;
    public Ingredient.Cook cook { get => _cook; }

    [SerializeField]
    private Sprite _sprite;
    public Sprite sprite { get => _sprite; }

    public bool Equals(CategoryData other)
    {
        if (baseIngred == other.baseIngred && cook == other.cook) return true;
        else return false;
    }
}
