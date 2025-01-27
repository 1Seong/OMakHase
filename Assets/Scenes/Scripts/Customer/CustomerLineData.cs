using UnityEngine;

[CreateAssetMenu(fileName = "CustomerLineData", menuName = "Scriptable Objects/CustomerLineData")]
public class CustomerLineData : ScriptableObject
{
    [SerializeField]
    private int _id;
    public int id { get => id; }

    [SerializeField]
    [TextArea] private string _line;
    public string line { get => _line; }
}