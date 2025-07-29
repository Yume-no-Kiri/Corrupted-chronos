using UnityEngine;
using System;
using Ink.Runtime;
using Ink.Parsed;
using System.Collections.Generic;

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

    public void DialogueStarted()
    {
        
        if (onDialogueStarted != null)
        {
            onDialogueStarted();
        }
    }

    public event Action onDialogueFinished;

    public void DialogueFinished()
    {
        if (onDialogueFinished != null)
        {
            onDialogueFinished();
        }
    }

    public event Action<string, List<Ink.Runtime.Choice>> onDisplayDialogue;

    public void DisplayDialogue(string dialogue_line, List<Ink.Runtime.Choice> dialogue_choices)
    {
        if (onDisplayDialogue != null)
        {
            onDisplayDialogue(dialogue_line, dialogue_choices);
        }
    }

    public event Action<int> onUpdateChoice;

    public void UpdateChoice(int value)
    {
        if (onUpdateChoice != null)
        {
            onUpdateChoice(value);
        }
    }
}
