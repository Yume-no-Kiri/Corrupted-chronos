using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class DialogueInvoker : MonoBehaviour
{
    [Header("Branca:")]
    [SerializeField] private string branca;

    [Header("Mode:")]
    [SerializeField] private int mode;

    [Header("player Input Actions:")]

    public PlayerInputActions playerInputActions;


    private bool enable_dialogue = false;

    private void Awake()
    {
        playerInputActions.Global.Interactua.performed += Interact;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void FixedUpdate()
    {
        //Invoca el dialeg
        if (playerInputActions.Pilot.enabled && enable_dialogue) 
            {
              GameEventsManager.instance.dialogue_events.EnterDialogue(branca, mode);
            } 
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            enable_dialogue = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            enable_dialogue = false;
        }
    }

    private void Interact(InputAction.CallbackContext patata)
    {

    }

}
