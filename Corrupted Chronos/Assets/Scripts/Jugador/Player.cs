using System;
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
    //TODO després del llançament de la demo
    //separar aquest script en player i inputmanager, aquest script controla massa coses 
    
    
    //físiques 
    private Rigidbody rb;
   
    
    //Controls/inputs
    //private PlayerInput playerInput;
    public PlayerInputActions playerInputActions;

    //moure capa
    private bool lockUp;
    private bool lockLow;
    private float moveDuration;
    private Vector3 targetPosition;
    private bool isMoving;
    private float elapsedTime;
    
    
    //colliders and stuff
    private Collider myColliderNau;
    
    private Collider myColliderPilot;

    
    //altres scripts
    InteractiveMethods interactiveMethods;
    List<InteractionType> whatsToInteract;
   
    
    //demoment serialitzat, pero es probable que quan això es torni més complexe s'hagui de passar per codi i no per inspector
    [Space(10)]
    [Header("Variables")]
    private GameObject _nau;
//    [SerializeField]
    private GameObject _pilot;
    [SerializeField]
    private PartsDatabaseSO _database;
    [SerializeField]
    private Vector3 spawnPosition;
    private Camera _playerCamera;
    [SerializeField] 
    private bool ComençaComPilot = true;
    
    [Space(3)]

    [Header("Garage stuff")]
    [SerializeField]
    private GameObject _garage;
    [Space(1)]
    
    private Vector3 MousePosition;
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    private Vector3Int gridPosition;
    [SerializeField]
    private Grid grid;
    private Camera _garageCamera;

    public event Action OnClicked, OnExit;
    
    //[SerializeField]
    //private Camera camGarage;

    
    
    
    private void Awake()
    {
        //varaibles generals
        moveDuration = 0.06f;
        isMoving = false;
        elapsedTime = 0f;
        lockUp = false;
        lockLow = false;
        
        
        //assignar _nau i _pilot amb databaseSO
        //REVISAR EL TEMA DEL TRANSFORM I POSICIÓ D'SPAWN, es bastant irregular
        //_database.AllNaus.FindIndex(data=>data.ID=id_a_buscar)
         _nau =Instantiate(_database.AllNaus[0].Prefab);
        _pilot= Instantiate(_database.AllPilots[0].Prefab);
        _nau.transform.position = new Vector3(0, spawnPosition.y,0);
        _pilot.transform.position = new Vector3(0, spawnPosition.y, 0);
        
        _nau.transform.SetParent(this.transform,false);
        _pilot.transform.SetParent(this.transform,false);
     
        
        //components and stuff
        rb = GetComponent<Rigidbody>();
        myColliderNau = _nau.GetComponent<Collider>();
        myColliderPilot = _pilot.GetComponent<Collider>();
        _playerCamera = GetComponentInChildren<Camera>();
        _garageCamera= _garage.GetComponentInChildren<Camera>();
        if(_garageCamera == null) Debug.LogError("not garage camera");
        if(_playerCamera == null) Debug.LogError("not player camera");
        
        
        if (myColliderNau == null || myColliderPilot == null){ Debug.LogError("No collider attached!"); return; }
        //playerInput = GetComponent<PlayerInput>();
        
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
        playerInputActions.Nau.CanviCapa.performed += moveCapa;
        playerInputActions.Global.Enable();
        playerInputActions.Global.Interactua.performed += Interact;
        
        //això demoment activat al inici per fer proves
        playerInputActions.Garage.Enable();
        //s'actualitza quan moc el mouse
        playerInputActions.Garage.MousePosition.performed += PositionMouse;
        playerInputActions.Garage.OnClick.performed += OnClickGarage;
        
        
        
        
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
        //això no és el més eficient del mon, perque ho està revisant amb el update, i no fa falta revisar-ho tant,
        //amb excepció de la segona part garage enabled
        //control de modes
        if (playerInputActions.Nau.enabled)
        {
            _playerCamera.enabled = true;
            _nau.SetActive(true);
            _pilot.SetActive(false);
        }else if (playerInputActions.Pilot.enabled)
        {
            _playerCamera.enabled = true;
            _nau.SetActive(false);
            _pilot.SetActive(true);
        }else if (playerInputActions.Garage.enabled)
        { 
            //Debug.Log("mousePOS:"+ MousePosition.ToString()+"  mouseIndi:"+mouseIndicator.transform.position.ToString());
            _playerCamera.enabled = false;
            _nau.SetActive(false);
            _pilot.SetActive(false);
            _garage.SetActive(true);
            //_garage.enabled = true;
                
            gridPosition = grid.WorldToCell(MousePosition);
            mouseIndicator.transform.position = MousePosition;
            cellIndicator.transform.position = grid.CellToWorld(gridPosition);

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
    public void CloseGarage()
    {
        _garage.SetActive(false);
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


    void MoveNau()
    {
        //Debug.Log("estàs amb la nau");

        rb.linearVelocity = Vector3.zero;
        
        // això del playerInputAction
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
    
    
    
    //Interacció amb objectes del escenari o coses especials
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
    
    public void SubInteraction(InteractionType type)
    {
        if (whatsToInteract.Contains(type))
        {
            whatsToInteract.Remove(type);
        }
        
    }

    private void PositionMouse(InputAction.CallbackContext context)
    {
       // Debug.Log("AAAAAAAAAA");
        
        Camera camGarage = _garage.GetComponentInChildren<Camera>();

        Vector3 mouse = playerInputActions.Garage.MousePosition.ReadValue<Vector2>();
        mouse.z = camGarage.nearClipPlane;
        
        Ray ray = camGarage.ScreenPointToRay(mouse);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Default");
        
        if (Physics.Raycast(ray, out hit,300,mask))
        {
            MousePosition= hit.point;
            //Debug.Log("BBBBBBBB");
        }
        //Debug.Log("AAAAAAAAAA "+mouse.ToString() );


    }

    void OnClickGarage(InputAction.CallbackContext context)
    {
        OnClicked?.Invoke();
    }
    
    
}
