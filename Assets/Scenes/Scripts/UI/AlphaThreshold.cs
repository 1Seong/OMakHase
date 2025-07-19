using UnityEngine;
using UnityEngine.UI;

public class AlphaThreshold : MonoBehaviour
{
    public float alphaThreshold = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var image = GetComponent<Image>();
        if (image != null)
        {
            image.alphaHitTestMinimumThreshold = alphaThreshold;
        }
    }
}
