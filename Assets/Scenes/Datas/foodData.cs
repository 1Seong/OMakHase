using UnityEngine;

[CreateAssetMenu(fileName = "foodData", menuName = "Scriptable Objects/foodData")]
public class foodData : ScriptableObject
{
    public enum Ingredient { rice, bread, noodles }
    public enum Cook { none, stirFry, roast }

    [SerializeField]
    private new string name;
    public string Name { get { return name; } }

    [SerializeField]
    private GameObject sprite;

    [SerializeField]
    private int taste;
    public int Taste { get { return taste; } }

    [SerializeField]
    private Ingredient foodIngrdient;
    public Ingredient FoodIngrdient { get {  return foodIngrdient; } }

    [SerializeField]
    private Cook foodCook;
    public Cook FoodCook { get { return foodCook; } }
}
