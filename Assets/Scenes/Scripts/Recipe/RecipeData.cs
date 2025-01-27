using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "Scriptable Objects/RecipeData")]
public class RecipeData : RecipeBase
{
    [SerializeField]
    private string _recipeName;
    public string recipeName { get => _recipeName; }

    [SerializeField]
    [TextArea]private string _desc;
    public string desc { get => _desc; }

    [SerializeField]
    private int _taste;
    public int taste { get => taste; }

    public bool isNew;
}
