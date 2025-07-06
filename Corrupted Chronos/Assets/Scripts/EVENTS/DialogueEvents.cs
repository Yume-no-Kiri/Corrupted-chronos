using UnityEngine;
using System;


public class DialogueEvents
{
    public event Action<string, int> onEnterDialogue;
    public void EnterDialogue(string branca, int mode)
    {
        if (onEnterDialogue != null)
        {
            onEnterDialogue(branca, mode);
        }
    }

    public event Action onDialogueStarted;

    public void onDialogueStarted()
    {
        if (onDialogueStarted != null)
        {
            onDialogueStarted();
        }
    }

    public event Action onDialoguesFinished;

    public void onDialogueFinished()
    {
        if (onDialogueFinished != null)
        {
            onDialogueFinished();
        }
    }

    public event Action<string> onDisplayDialogue;

    public void onDisplayDialogue(string dialogue_line)
    {
        if (onDisplayDialogue != null)
        {
            onDisplayDialogue(dialogue_line);
        }
    }


}
