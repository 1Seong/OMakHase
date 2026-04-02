
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{

    public void GetDialogue()
    {
        DialogueManager.Instance.GetNextDialogue();

    }
}
