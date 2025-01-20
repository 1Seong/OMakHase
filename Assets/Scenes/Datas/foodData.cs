using UnityEngine;

[CreateAssetMenu(fileName = "foodData", menuName = "Scriptable Objects/foodData")]
public class foodData : ScriptableObject
{
    public enum Ingredient { rice, bread, noodles }
    public enum Cook { none, stirFry, roast }

    [SerializeField]
    private new string name;
    public string Name { get { return name; } }
    public void SetName(string name) { this.name = name; }

    [SerializeField]
    private GameObject sprite;

    [SerializeField]
    private int taste;
    public int Taste { get { return taste; } }
    public void SetTaste(int taste) { this.taste = taste; }

    [SerializeField]
    private Ingredient foodIngrdient;
    public Ingredient FoodIngrdient { get {  return foodIngrdient; } }
    public void SetIngredient(int n) 
    { 
        if(n == 0)
            foodIngrdient = Ingredient.rice;
        else if (n == 1)
            foodIngrdient = Ingredient.bread;
        else if (n == 2)
            foodIngrdient = Ingredient.noodles;
    }

    [SerializeField]
    private Cook foodCook;
    public Cook FoodCook { get { return foodCook; } }
    public void SetCook(int n)
    {
        if (n == 0)
            foodCook = Cook.none;
        else if (n == 1)
            foodCook = Cook.stirFry;
        else if (n == 2)
            foodCook = Cook.roast;
    }

}
