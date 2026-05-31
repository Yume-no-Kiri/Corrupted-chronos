using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class DialogueInvoker : MonoBehaviour
{
    [Header("Branca:")]
    [SerializeField] private string branca;

    [Header("Mode:")]
    [SerializeField] private int mode;
    [SerializeField]
    public InteractionType ThisInteraction;

    private bool able_talk=false;
 

    private void Awake()
    {
        if (branca == "cinem1" || branca=="cinemF")
        {

            GameEventsManager.instance.dialogue_events.EnterDialogue(branca, 0);
        }
    }
 
    private void Start()
    {
        if (branca == "cinem1" || branca == "cinemF")
        {

            GameEventsManager.instance.dialogue_events.EnterDialogue(branca, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            able_talk = true;
            Player i = other.GetComponentInParent<Player>();
            Debug.Log("this is your Jugador" + i.gameObject.name);
            if (i != null)
            {
                //canvia l'interacci�, canviar de pilot nau, nau pilot, entrar sortir de garatge, xarlar, etc...
                i.AddInteraction(ThisInteraction);
                i.AddDialogueInfo(branca, mode);


            }   
            else
            {
                Debug.LogWarning("Object entered the trigger but does not have YourScriptType: " + other.gameObject.name);
            }

            //playerInputActions.Global.Interactua += other. nau_pilot();
            Debug.Log("Player can talk.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            able_talk = false;

            Player i = other.GetComponentInParent<Player>();

            if (i != null)
            {

                i.SubInteraction(ThisInteraction);

            }
            else
            {
                Debug.LogWarning("Object entered the trigger but does not have YourScriptType: " + other.gameObject.name);
            }

            //playerInputActions.Global.Interactua += other. nau_pilot();
            Debug.Log("Player left special zone.");
        }
    }

    private void Update()
    {
        if (!able_talk) return;
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            GameEventsManager.instance.dialogue_events.EnterDialogue(branca, 0);
        }
    }
}
