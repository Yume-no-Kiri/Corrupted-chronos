using System;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionScriptAdder : MonoBehaviour
{
   

    //aquest es el mètode que s'activa la possiblitat d'activar quan hi passar el jugador per el trigger
    [SerializeField]
    public InteractionType ThisInteraction;

    [Space(2)]
    [Header("if interaction is +position")]
    Vector3 positionPilot;
    Vector3 positionNau;



    private void Awake()
    {
        //inputActions = new PlayerInputActions();
    }

    //quan l'objecte entri en la zona afegeix la possibilitat d'interactuar
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Player entered");

        //comprovem que es jugador
        if (other.CompareTag("Player"))
        {
            
            //trigger detecta nau o pilot, pero no el seu pare player, qeu l'escript que ens interessa
            Player i= other.GetComponentInParent<Player>();
            // Debug.Log("this is your Jugador" +i.gameObject.name);
            if (i != null)
            {
                //canvia l'interacció, canviar de pilot nau, nau pilot, entrar sortir de garatge, xarlar, etc...
                //escriure aquí positionPilot i PositionNau, si la interacció l'utilitzen perf, si no encara millor
                i.AddInteraction(ThisInteraction);
            }
            else
            {
                Debug.LogWarning("Object entered the trigger but does not have YourScriptType: " + other.gameObject.name);
            }

            //playerInputActions.Global.Interactua += other. nau_pilot();
            // Debug.Log("Player entered special zone.");
        }
    }

    //quan l'objecte surti de la zona treu la possibilitat d'interactuar
    private void OnTriggerExit(Collider other)
    {
        //mètode de seguretat, per si les mosques.
        //moltes vegades al desactivar objectes aquest mètode no es cridara
        // Debug.Log("Player left");

        if (other.CompareTag("Player"))
        {
            Player i= other.GetComponentInParent<Player>();

            if (i != null)
            {

                i.SubInteraction(ThisInteraction);

            }else
            {
                Debug.LogWarning("Object entered the trigger but does not have YourScriptType: " + other.gameObject.name);
            }

            //playerInputActions.Global.Interactua += other. nau_pilot();
            // Debug.Log("Player left special zone.");
        }
    }


    
}
