using System;
using Unity.VisualScripting;
using UnityEngine;

public class changenau_pilot : MonoBehaviour
{
    private BoxCollider boxCollider;
    //private PlayerInputActions inputActions;
    private bool PlayerInside = false;

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

    private void OnTriggerEnter(Collider other)
    {
        //revisar, per els problemes que te que onTriggerExit no es crida al desactivar el objecte
        Debug.Log("Player entered");

        if (other.CompareTag("Jugador"))
        {
            Player i= other.GetComponentInParent<Player>();
            Debug.Log("this is your Jugador" +i.gameObject.name);
            if (i != null)
            {
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

    private void OnTriggerExit(Collider other)
    {
        //Moltes vegades quan un objecte es desactivi això no es cridara, pero crec que es bona practica mantenir això aquí
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
