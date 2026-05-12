using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }
    public DialogueEvents dialogue_events;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Ja hi ha un gameEventsManager en l'escena");
        }
        instance = this;


        dialogue_events = new DialogueEvents();
    }

   
}
