using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{

    public void GetDialogue()
    {
        DialogueManager.Instance.GetNextDialogue();

    }
}
