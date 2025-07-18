using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ink.Parsed;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    
    
    //físiques 
    private Rigidbody rb;
    private Collider myCollider;
    //[SerializeField] 
    private float checkRadius = 0.5f;
    [SerializeField] 
    private LayerMask collisionMask;
    
    //Controls/inputs
    //private PlayerInput playerInput;
    public PlayerInputActions playerInputActions;

    //moure capa
    //no segur del private, recomanció de rider
    private Collider[] hitsAbove;
    private Collider[] hitsBelow;
    
    //altres scripts
    InteractiveMethods interactiveMethods;
    List<InteractionType> whatsToInteract;

    //demoment serialitzat, pero es probable que quan això es torni més complexe s'hagui de passar per codi i no per inspector
    [SerializeField]
    private GameObject _nau;
    [SerializeField]
    private GameObject _pilot;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        if (myCollider == null){ Debug.LogError("No collider attached!"); return; }
        //playerInput = GetComponent<PlayerInput>();
        
        playerInputActions = new PlayerInputActions();
        playerInputActions.Nau.Enable();
        _nau.SetActive(true);
        playerInputActions.Global.Enable();
        playerInputActions.Global.Interactua.performed += Interact;
        
        playerInputActions.Nau.CanviCapa.performed += moveCapa;

        interactiveMethods= new InteractiveMethods();
        whatsToInteract = new List<InteractionType>();
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Update()
    {

        if (playerInputActions.Nau.enabled)
        {
            _nau.SetActive(true);
            _pilot.SetActive(false);
        }else if (playerInputActions.Pilot.enabled)
        {
            _nau.SetActive(false);
            _pilot.SetActive(true);
        }
    }

    void FixedUpdate()
    {

        if (playerInputActions.Nau.enabled)
        {
            MoveNau();
        }else if (playerInputActions.Pilot.enabled)
        {
            MovePilot();
        }
        
    }


    void MoveNau()
    {
        //Debug.Log("estàs amb la nau");

        rb.linearVelocity = Vector3.zero;
        
        float spawnsp= 0.5f;
        hitsAbove = Physics.OverlapSphere(new Vector3(rb.position.x,rb.position.y+spawnsp,rb.position.z), checkRadius, collisionMask);
        hitsBelow = Physics.OverlapSphere(new Vector3(rb.position.x,rb.position.y-spawnsp,rb.position.z), checkRadius, collisionMask);

        //això del playerInputAction
        Vector2 movement =playerInputActions.Nau.MoveNau.ReadValue<Vector2>().normalized;
        float moveSpeed = 5f;
        //transform.Translate(new Vector3(movement.x, 0, movement.y));

        rb.MovePosition(rb.position+ new Vector3(movement.x, 0, movement.y) * moveSpeed *Time.deltaTime);
        
    }

    void MovePilot()
    {
        //Debug.Log("estàs amb el pilot");
        
        rb.linearVelocity = Vector3.zero;
        Vector2 movement =playerInputActions.Pilot.MovePilot.ReadValue<Vector2>().normalized;
        float moveSpeed = 2f;
        //transform.Translate(new Vector3(movement.x, 0, movement.y));

        rb.MovePosition(rb.position+ new Vector3(movement.x, 0, movement.y) * moveSpeed *Time.deltaTime);
        
    }
    

    private void moveCapa(InputAction.CallbackContext context)
    {
       
        //Debug.Log("rodeta ratolí detectat");
        float movement = playerInputActions.Nau.CanviCapa.ReadValue<float>();
        
        if (movement > 0 )
        {
            if (hitsAbove.Length > 1 )//&& !hitsBelow.Contains(myCollider))
            {
                //Debug.Log("Can't move ABOVE. Blocked by:");

                foreach (Collider col in hitsAbove)
                {
                    if (col != myCollider)
                    {
                        Debug.Log("- " + col.gameObject.name + " (tag: " + col.tag + ")");
                    }
                    else
                    {
                        return;

                    }
                }

            }
        }else if (movement < 0)
        {
            if (hitsBelow.Length > 1)// && !hitsBelow.Contains(myCollider))
            {
                //Debug.Log("Can't move BELOW. Blocked by:");

                foreach (Collider col in hitsBelow)
                {
                    if (col != myCollider)
                    {
                        Debug.Log("- " + col.gameObject.name + " (tag: " + col.tag + ")");
                    }else
                    {
                        return;

                    }
                }

            }
        }
        
        //Vector3 newPosition = rb.position + new Vector3(0, movement, 0);
        //transform.Translate(new Vector3(0, movement, 0));
        rb.MovePosition(rb.position + new Vector3(0, movement, 0));
        //rb.AddForce(new Vector3(0, movement,0), ForceMode.Force);
        //Debug.Log("canvi capa"+ movement);

    }
    void OnDrawGizmos() //els detectors de col·lisió basicament
    {
        if (rb == null) return; // Avoid errors if not set in Inspector
        Vector3 targetPos = rb.position + Vector3.down;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPos, checkRadius);
        Vector3 targetPos1 = rb.position + Vector3.up;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPos1, checkRadius);
    }

    
    
    
    
    //Interacció amb objectes del escenari o coses especials
    void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("you press E");
        interactiveMethods.DoInteractions(whatsToInteract,this);
        
    }
    public void AddInteraction(InteractionType type)
    {
        Debug.Log("added");
        whatsToInteract.Add(type);
    }
    
    public void SubInteraction(InteractionType type)
    {
        whatsToInteract.Remove(type);
    }
    
    
    
}
