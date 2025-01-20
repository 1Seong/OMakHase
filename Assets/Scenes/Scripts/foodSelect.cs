
using UnityEngine;

public class foodSelect : MonoBehaviour
{
    public foodData FoodData;

    public void chooseFood() {
        Debug.Log(FoodData.Name);
        Debug.Log(FoodData.FoodIngrdient);
        Debug.Log(FoodData.FoodCook);
        Debug.Log("---------------------");
    }
}
