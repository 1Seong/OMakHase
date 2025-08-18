using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleSkipController : MonoBehaviour
{
    private readonly int ScaleThreshold = 7;
    private readonly float MiddleScaleFactor = 0.2f;
    private readonly float ButtonScaleFactor = 0.1f;

    private Button[] buttons;
    private TextMeshProUGUI[] tmps;

    [SerializeField]
    private Image middle;

    [SerializeField]
    private bool horizontalScaleActive;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        tmps = GetComponentsInChildren<TextMeshProUGUI>();
    }

    /*
    private void Start()
    {
        DialogueManager.Instance.MultipleTextSetEvent += setText;
    }
    */
    public void SetText(string text1, string text2)
    {
        var maxLength = text1.Length > text2.Length ? text1.Length : text2.Length;

        if(horizontalScaleActive)
            horizontalScale(maxLength);

        tmps[0].text = text1;
        tmps[1].text = text2;
    }

    private void horizontalScale(int len)
    {
        var middleTargetScale = default(Vector3);
        var buttonTargetScale = default(Vector3);

        if(len > ScaleThreshold)
        {
            var diff = len - ScaleThreshold;
            middleTargetScale = new Vector3(1 + diff * MiddleScaleFactor, 1, 1);
            buttonTargetScale = new Vector3(1 + diff * ButtonScaleFactor, 1, 1);
        }
        else
        {
            middleTargetScale = Vector3.one;
            buttonTargetScale = Vector3.one;
        }

        foreach(var button in buttons)
        {
            button.transform.localScale = buttonTargetScale;
        }

        middle.transform.localScale = middleTargetScale;

    }

    // for debug purpose
    public void SetTextDebug(int num)
    {
        var text1 = "";
        for (int i = 0; i != num; i++)
            text1 += "°¡";

        SetText(text1, "°¡");
    }
}
