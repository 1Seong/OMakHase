using UnityEngine;

[CreateAssetMenu(fileName = "customerData", menuName = "Scriptable Objects/customerData")]
public class customerData : ScriptableObject
{
    [SerializeField]
    private new string name;
    public string Name { get { return name; } }

    [SerializeField]
    private bool isSpecial;
    public bool IsSpecial { get { return isSpecial; } }

    [SerializeField]
    private bool isRequire; // 요구사항 만족 여부
    public bool IsRequire { get { return isRequire; } }

    [SerializeField]
    private foodData require; // 요구사항
    public foodData Require { get { return require; } } 

    public enum Personality { Picky, Normal, Generous }

    [SerializeField]
    private Personality customerPersonality;
    public Personality CustomerPersonality { get { return customerPersonality; } }
}
