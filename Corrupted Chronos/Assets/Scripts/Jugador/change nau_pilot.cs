using System;
using Unity.VisualScripting;
using UnityEngine;

public class changenau_pilot : MonoBehaviour
{
    
    //private PlayerInputActions inputActions;
    //private bool PlayerInside = false;

    //aquest es el mètode que s'activa la possiblitat d'activar quan hi passar el jugador per el trigger
    [SerializeField]
    public InteractionType ThisInteraction;
    
    
    private void Awake()
    {
        //inputActions = new PlayerInputActions();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    //quan l'objecte entri en la zona afegeix la possibilitat d'interactuar
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered");

        //comprovem que es jugador
        if (other.CompareTag("Jugador"))
        {
            
            //trigger detecta nau o pilot, pero no el seu pare player, qeu l'escript que ens interessa
            Player i= other.GetComponentInParent<Player>();
            Debug.Log("this is your Jugador" +i.gameObject.name);
            if (i != null)
            {
                //canvia l'interacció, canviar de pilot nau, nau pilot, entrar sortir de garatge, xarlar, etc...
                i.AddInteraction(ThisInteraction);
            }
            else
            {
                Debug.LogWarning("Object entered the trigger but does not have YourScriptType: " + other.gameObject.name);
            }

            //playerInputActions.Global.Interactua += other. nau_pilot();
            Debug.Log("Player entered special zone.");
        }
    }

    //quan l'objecte surti de la zona treu la possibilitat d'interactuar
    private void OnTriggerExit(Collider other)
    {
        //mètode de seguretat, per si les mosques.
        //moltes vegades al desactivar objectes aquest mètode no es cridara
        Debug.Log("Player left");

        if (other.CompareTag("Jugador"))
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
            Debug.Log("Player left special zone.");
        }
    }

   /* bool CanGetPlayer(var i)
    {
        

        if (i != null)
        {
            Debug.Log("error get Player");
            return false;
            
        }
        else
        {
            Debug.Log("get Player");
            return true;
        }
        
    }*/
    
    /* void OnDrawGizmos()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.color = Color.cyan; // Change color if needed
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }*/
    
}
