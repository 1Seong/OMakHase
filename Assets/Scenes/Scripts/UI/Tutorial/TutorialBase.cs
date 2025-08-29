using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialBase : MonoBehaviour
{
    [Serializable]
    public struct ButtonAndEvent
    {
        public UnityEvent ButtonOnClickUnityEvent;
        public Button TargetButton;
        [NonSerialized] public UnityAction CachedAction;
    }

    //public TutorialDirection Direction;
    public GameObject StartTutorialPanel;
    public ButtonAndEvent[] ButtonAndEvents;
    public ButtonAndEvent FinishEvent;


    private void Start()
    {
        if (!GameManager.instance.TutorialActive)
            return;

        StartTutorialPanel.SetActive(true);

        for(int i = 0; i != ButtonAndEvents.Length; ++i)
        {
            var index = i; // need to make a local variable because of the lambda closure issue
            ButtonAndEvents[index].CachedAction = () => ButtonAndEvents[index].ButtonOnClickUnityEvent?.Invoke();
            ButtonAndEvents[index].TargetButton.onClick.AddListener(ButtonAndEvents[index].CachedAction);
        }

        FinishEvent.CachedAction = () => FinishEvent.ButtonOnClickUnityEvent?.Invoke();
        FinishEvent.TargetButton.onClick.AddListener(FinishEvent.CachedAction);
        FinishEvent.TargetButton.onClick.AddListener(tutorialFinish);
    }

    private void tutorialFinish()
    {
        foreach (var i in ButtonAndEvents)
        {
            i.TargetButton.onClick.RemoveListener(i.CachedAction);
        }

        FinishEvent.TargetButton.onClick.RemoveListener(FinishEvent.CachedAction);
        FinishEvent.TargetButton.onClick.RemoveListener(tutorialFinish);
    }

    /*
    public void SetDirection(int i)
    {

    }
    */

}
