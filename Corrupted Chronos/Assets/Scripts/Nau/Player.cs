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
   
    
    //Controls/inputs
    public PlayerInputActions playerInputActions;

    //moure capa
    private bool lockUp;
    private bool lockLow;
    private float moveDuration;
    private Vector3 targetPosition;
    private bool isMoving;
    private float elapsedTime;
    
    
    //colliders 
    private Collider myColliderNau;
    private Collider myColliderPilot;

    
    //mètodes interactius
    InteractiveMethods interactiveMethods;
    //és un llista, perque en algun moment HAUREM de resoldre que passa si estàs en una àrea on, pots canviar a pilot i també pots entrara a garatge
    List<InteractionType> whatsToInteract;
    
    //aquest script controla tant l'ús de la nau com del pilot
    [SerializeField]
    private GameObject _nau;
    [SerializeField]
    private GameObject _pilot;

    [SerializeField] 
    private bool ComençaComPilot = true;

    //Dialeg:
    public string branca;
    public int mode;

    private void Awake()
    {
        //varaibles generals
        moveDuration = 0.06f;
        isMoving = false;
        elapsedTime = 0f;
        lockUp = false;
        lockLow = false;
        
        //components and stuff
        rb = GetComponent<Rigidbody>();
        myColliderNau = _nau.GetComponent<Collider>();
        myColliderPilot = _pilot.GetComponent<Collider>();

       
        
        if (myColliderNau == null || myColliderPilot == null){ Debug.LogError("No collider attached!"); return; }
        
        //maps inputs i connexions
        playerInputActions = new PlayerInputActions();
        if (ComençaComPilot)
        {
            playerInputActions.Pilot.Enable();
            _pilot.SetActive(true);
            
        }
        else
        {
            playerInputActions.Nau.Enable();
            _nau.SetActive(true);

        }
        
        //pels multiples mètodes conecta amb els inputs
        //playerInputActions.Nau.MoveNau.performed += MoveNau;
        //playerInputActions.Pilot.MovePilot.triggered += MovePilot;
        playerInputActions.Nau.CanviCapa.performed += moveCapa;
        playerInputActions.Global.Enable();
        playerInputActions.Global.Interactua.performed += Interact;
        

        //connexions amb els interactiveMethods
        interactiveMethods= gameObject.AddComponent<InteractiveMethods>();
        whatsToInteract = new List<InteractionType>();
        
        
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Update()
    {
        //mètode complementari de interactive method
        //si estem en pilot o en nau, que s'ha de mostrar
        if (playerInputActions.Nau.enabled)
        {
            _nau.SetActive(true);
            _pilot.SetActive(false);
        }else if (playerInputActions.Pilot.enabled)
        {
            _nau.SetActive(false);
            _pilot.SetActive(true);
        }


        //fix per no travessar terreny

        if (playerInputActions.Garage.enabled)
        {
            
        }
        
        

        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            if (t >= 1f)
            {
                Debug.Log("enter if in the fixed update ");
                isMoving = false; // Movement finished
            }
        }
        
        
    }

    void FixedUpdate()
    {
        //transició de capa
        /*if (isMoving)
        {
            Debug.Log("enter fixed update ");
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            Vector3 newPos = Vector3.Lerp(rb.position, new Vector3(rb.position.x, targetPosition.y, rb.position.z), t);
            rb.MovePosition(newPos);
            if (t >= 1f)
            {
                Debug.Log("enter if in the fixed update ");

                isMoving = false; // Movement finished
            }
        }*/
        //fas canvis adients segons el input map que estigui actiu
        if (playerInputActions.Nau.enabled)
        {
            MoveNau();
        }else if (playerInputActions.Pilot.enabled)
        {
            MovePilot();
        }
        
       
        
    }


    //inputs per la nau
    void MoveNau()
    {
        //Debug.Log("estàs amb la nau");

        rb.linearVelocity = Vector3.zero;
        
        // això del playerInputAction
        Vector2 movement =playerInputActions.Nau.MoveNau.ReadValue<Vector2>().normalized;
        float moveSpeed = 5f;
        //transform.Translate(new Vector3(movement.x, 0, movement.y));

        rb.MovePosition(rb.position+ new Vector3(movement.x, 0, movement.y) * (moveSpeed * Time.deltaTime));
        
    }

    //inputs per el pilot
    void MovePilot()
    {
        //Debug.Log("estàs amb el pilot");
        
        rb.linearVelocity = Vector3.zero;
        Vector2 movement =playerInputActions.Pilot.MovePilot.ReadValue<Vector2>().normalized;
        float moveSpeed = 2f;
        //transform.Translate(new Vector3(movement.x, 0, movement.y));

        rb.MovePosition(rb.position+ new Vector3(movement.x, 0, movement.y) * (moveSpeed * Time.deltaTime));
        
    }
    

    private void moveCapa(InputAction.CallbackContext context)
    {
        
        Debug.Log("rodeta ratolí detectat");
        float movement = playerInputActions.Nau.CanviCapa.ReadValue<float>();
        
        //si ja estic fent el moviment cap a una nova capa, es podria guardar en una mena de coyote time
        if (isMoving)
        {
            Debug.Log("enter is moving");
            return;
        }
        
        if (movement > 0 )
        {
            if (lockUp==true )//&& !hitsBelow.Contains(myCollider))
            {
                Debug.Log("enter is lockUP");
                return;
            }
        }else if (movement < 0)
        {
            if (lockLow==true)// && !hitsBelow.Contains(myCollider))
            {
                Debug.Log("enter is lockLOW");

                return;

            }
        }
        
        Debug.Log("enter TO THE END");
        isMoving = true;
        elapsedTime = 0f;
        //targetPosition = rb.position + new Vector3(0, movement, 0);
        rb.MovePosition(rb.position + new Vector3(0, movement, 0));

    }
    /*void OnDrawGizmos() //els detectors de col·lisió basicament
    {
        if (rb == null) return; // Avoid errors if not set in Inspector
        Vector3 targetPos = rb.position + Vector3.down;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPos, checkRadius);
        Vector3 targetPos1 = rb.position + Vector3.up;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPos1, checkRadius);
    }*/

    //Mètode cridat per els scripts que detecten capes
    public void DetectorCapaResponse(DetectCanviCapaType detect, bool isLocked)
    {
        if (detect == DetectCanviCapaType.Up)
        {
            lockUp = isLocked;
        }else if (detect == DetectCanviCapaType.Low)
        {
            lockLow = isLocked;
        }
        
        
    }
    
    
    
    
    
    //Interacció amb objectes del escenari o cases especials
    //per no tractar tots els mètodes que interactuïn amb el jugador i sobretot amb la e, aquest procés 
    //està efectuat en el seu propi script interactiveMethods
    
    //Executa les interaccions en el script corresponent
    void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("you press E");
        //revsiar aquesta part, per veure 
        interactiveMethods.DoInteractions(whatsToInteract,this);
        
    }
    

    //afegeix mètodes a interactuar i treu mètodes a interactuar

    public void AddInteraction(InteractionType type)
    {
        Debug.Log("added");
        whatsToInteract.Add(type);
    }
    
    //treu interacció
    public void SubInteraction(InteractionType type)
    {
        if (whatsToInteract.Contains(type))
        {
            whatsToInteract.Remove(type);
        }
        
    }

    public void AddDialogueInfo(string branca, int mode)
    {
        this.branca = branca;
        this.mode = mode;   
    }
    
    
    
}
