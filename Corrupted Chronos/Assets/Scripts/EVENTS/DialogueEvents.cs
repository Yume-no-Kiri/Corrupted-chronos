using UnityEngine;
using System;


public class DialogueEvents
{
    public event Action<string, string, int> onEnterDialogue;
    public void EnterDialogue(string character, string emotion, int mode)
    {
        if (onEnterDialogue != null)
        {
            onEnterDialogue(character, emotion, mode);
        }
    }

}
