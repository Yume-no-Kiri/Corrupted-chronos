using UnityEngine;
using System;

public class DialogueInvoker : MonoBehaviour
{
    [Header("Branca:")]
    [SerializeField] private string branca;

    [Header("Mode:")]
    [SerializeField] private int mode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Invoca el dialeg
        if (branca != null) 
            {
              GameEventsManager.instance.dialogue_events.EnterDialogue(branca, mode);
            } 
    }

}
