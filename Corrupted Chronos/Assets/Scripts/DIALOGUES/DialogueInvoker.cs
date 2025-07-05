using UnityEngine;
using System;

public class DialogueInvoker : MonoBehaviour
{
    [Header("Character:")]
    [SerializeField] private string npc;

    [Header("Emotion:")]
    [SerializeField] private string emotion;

    [Header("Mode:")]
    [SerializeField] private int mode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Invoca el dialeg
        if (npc != null) 
            {
              GameEventsManager.instance.dialogue_events.EnterDialogue(npc, emotion, mode);
            } 
    }

}
