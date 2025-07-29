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
    //revisar la separació entre aquest script player i inputmanager, aquest script encara controla massa coses 
    
    
    //físiques 
    private Rigidbody rb;
   
    
    //Controls/inputs
    //private PlayerInput playerInput;
    [SerializeField]
    public GameObject inputMangerObject;
    
    public InputManager inputManager;

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
    public GameObject pointAddNau;
    private GameObject _nau;
    private GameObject _pilot;
    [SerializeField]
    private PartsDatabaseSO _database;
    [SerializeField]
    private Vector3 spawnPosition;
    [SerializeField]
    private Camera _playerCamera;
    [SerializeField] 
    private bool ComencaComPilot = true;
    
    [Space(3)]

    [Header("Garage stuff")]
    [SerializeField]
    private GameObject _garage;
    [Space(1)]
    
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    private Vector3Int gridPosition;
    [SerializeField]
    private Grid grid;
    private Camera _garageCamera;

    private Vector3 mousePos;
    private Vector3 mousedir;
    private Quaternion mouseRotation;       
    public float rotationSpeed = 40f;

    //[SerializeField]
    //private Camera camGarage;

    //parts stuff
    private List<GameObject> AddedParts;
    
    //Dialeg:
    public string branca;
    public int mode;
    
    
    private void Awake()
    {
        //crec que hauria d'estructurar, que va a awake i que a start
        inputManager = inputMangerObject.GetComponent<InputManager>();
        
        if (inputManager == null){Debug.LogError("aaaaAAAA inputmanager");}
        
        //varaibles generals
        moveDuration = 0.06f;
        isMoving = false;
        elapsedTime = 0f;
        lockUp = false;
        lockLow = false;
        
        
        //assignar _nau i _pilot amb databaseSO
        //REVISAR EL TEMA DEL TRANSFORM I POSICIÓ D'SPAWN, es bastant irregular
        //_database.AllNaus.FindIndex(data=>data.ID=id_a_buscar)
        //pointAddNau= transform.Find("AddNau").gameObject;
        //if(_playerCamera == null) Debug.LogError("not AddNau");
        
        //revisar aquest offset quan revisi el punt de pivot per garatge i mode normal
        //NO FUNCIOONA EL PUTO OFFSET NO HO ENTENC, SI AGAFA EL ADDNAU OBJECT, PERQUE COLL NO ÉS FILL, JO EM TORNO BOIG
        //ara funcioan pero no es la solucio pel problema del pivot al rotar, hauré de crear algun traductor de posició per el grid,
        //o crear 2 objectes per cada part, per grid i per col·locar
        
        
        //Vector3 spawnOffset = new Vector3(1, 0f, 1); // example offset (1 unit up)
         //_nau =Instantiate(_database.AllNaus[0].Prefab);
         //_nau.transform.SetParent (_pointAddNau.transform);
         //_nau.transform.position = new Vector3(0, 0, 0); 
         //_nau.transform.localRotation = Quaternion.identity;
         
        //_nau =Instantiate(_database.AllNaus[0].Prefab, pointAddNau.transform, true);
        //_nau.transform.position = pointAddNau.transform.position;
        _nau = Instantiate(_database.AllNaus[0].Prefab);
        _pilot= Instantiate(_database.AllTravelers[0].Prefab, this.transform, false);
        _nau.transform.position = new Vector3(0, spawnPosition.y,0);
        _pilot.transform.position = new Vector3(0, spawnPosition.y, 0);
        
        //OHHHHH that's why
        _nau.transform.SetParent(this.transform,false);


        //components and stuff
        rb = GetComponent<Rigidbody>();
        myColliderNau = _nau.transform.Find("Collisions").transform.Find("Collider").GetComponentInChildren<Collider>();
        myColliderPilot = _pilot.GetComponent<Collider>();
        //_playerCamera = GetComponentInParent<Camera>();
        _garageCamera= _garage.GetComponentInChildren<Camera>();
        if(_garageCamera == null) Debug.LogError("not garage camera");
        if(_playerCamera == null) Debug.LogError("not player camera");
        
        
        if (myColliderNau == null || myColliderPilot == null){ Debug.LogError("No collider attached!"); return; }
        //playerInput = GetComponent<PlayerInput>();
        
        
        
        //connexions amb els interactiveMethods
        interactiveMethods= gameObject.AddComponent<InteractiveMethods>();
        whatsToInteract = new List<InteractionType>();
        
        AddedParts= new List<GameObject>();
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
        
        
        _garage.SetActive(false);
        //maps inputs i connexions
        if (ComencaComPilot)
        {
            inputManager.playerInputActions.Pilot.Enable();
            inputManager.playerInputActions.Nau.Disable();

            _pilot.SetActive(true);
            
        }
        else
        {
            inputManager.playerInputActions.Nau.Enable();
            inputManager.playerInputActions.Pilot.Disable();
            _nau.SetActive(true);

        }
        inputManager.playerInputActions.Nau.CanviCapa.performed += moveCapa;
        inputManager.playerInputActions.Global.Enable();
        inputManager.playerInputActions.Global.Interactua.performed += Interact;
        
        //això demoment activat al inici per fer proves
        inputManager.playerInputActions.Garage.Disable();
        //s'actualitza quan moc el mouse
        inputManager.playerInputActions.Global.MousePosition.performed += PositionMouse;
        //inputManager.playerInputActions.Garage.OnClick.performed += OnClickGarage;


    }

    void Update()
    {
        //això no és el més eficient del mon, perque ho està revisant amb el update, i no fa falta revisar-ho tant,
        //amb excepció de la segona part garage enabled
        //control de modes
        if (inputManager.playerInputActions.Nau.enabled)
        {
            _playerCamera.enabled = true;
            _garageCamera.enabled = false;
            
            //fix, em sortia un missatge estrany d'error
            _playerCamera.GetComponent<AudioListener>().enabled = true;
            _garageCamera.GetComponent<AudioListener>().enabled = false;
            
            _nau.SetActive(true);
            _pilot.SetActive(false);
        }else if (inputManager.playerInputActions.Pilot.enabled)
        {
            _playerCamera.enabled = true;
            _garageCamera.enabled = false;

            _playerCamera.GetComponent<AudioListener>().enabled = true;
            _garageCamera.GetComponent<AudioListener>().enabled = false;
            
            _nau.SetActive(false);
            _pilot.SetActive(true);
        }else if (inputManager.playerInputActions.Garage.enabled)
        { 
            //Debug.Log("mousePOS:"+ MousePosition.ToString()+"  mouseIndi:"+mouseIndicator.transform.position.ToString());
            _playerCamera.enabled = false;
            _garageCamera.enabled = true;
            
            _playerCamera.GetComponent<AudioListener>().enabled = false;
            _garageCamera.GetComponent<AudioListener>().enabled = true;
            
            _nau.SetActive(false);
            _pilot.SetActive(false);
            _garage.SetActive(true);
            //_garage.enabled = true;
            
            /*això potser s'hauria de moure a placementSystem
            gridPosition = grid.WorldToCell(inputManager.MousePosition);
            mouseIndicator.transform.position = inputManager.MousePosition;
            cellIndicator.transform.position = grid.CellToWorld(gridPosition);
            */
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
        //transform.right=mousedir;
        //transform.rotation = Quaternion.Euler(0f, angleCamera,0f );
        transform.rotation = Quaternion.Lerp(transform.rotation, mouseRotation, Time.deltaTime * rotationSpeed);

        if (inputManager.playerInputActions.Nau.enabled)
        {
            MoveNau();
        }else if (inputManager.playerInputActions.Pilot.enabled)
        {
            MovePilot();
        }
        
    }
    private void PositionMouse(InputAction.CallbackContext context)
    {
       // Debug.Log("AAAAAAAAAA");
        

        mousePos = inputManager.playerInputActions.Global.MousePosition.ReadValue<Vector2>();


        if (inputManager.playerInputActions.Garage.enabled)
        {
            //Camera camGarage = _garage.GetComponentInChildren<Camera>();
            mousePos.z = _garageCamera.nearClipPlane;
        
            Ray ray = _garageCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Default");
        
            if (Physics.Raycast(ray, out hit,300,mask))
            {
                inputManager.MousePosition= hit.point;
                //Debug.Log("BBBBBBBB");
            }
            //Debug.Log("AAAAAAAAAA "+mouse.ToString() );

        }else if (inputManager.playerInputActions.Nau.enabled || inputManager.playerInputActions.Pilot.enabled)
        {
            //Camera camPlayer = _playerCamera.GetComponentInChildren<Camera>();
            //mouse.z = _playerCamera.nearClipPlane;
            //dona igual si nau o player, estan conectats
            //mousePos.y = _nau.transform.position.y;
            //mousePos= _playerCamera.ScreenToWorldPoint(mousePos);
            //mousedir = (mousePos - transform.position); 
            //angleCamera = Mathf.Atan2(mousedir.y, mousedir.x) * Mathf.Rad2Deg;
            //mousedir= new Vector2(mousePos.x-transform.position.x, mousePos.z-transform.position.z);
            mousePos.z = Mathf.Abs(_playerCamera.transform.position.y - transform.position.y);
            Vector3 mouseWorldPosition = _playerCamera.ScreenToWorldPoint(mousePos);
            
            mousedir= mouseWorldPosition - transform.position;
            mousedir.y = 0;
            if (mousedir != Vector3.zero)
            {
                mouseRotation = Quaternion.LookRotation(mousedir);
            }
            
        }
    }
    
    public void CloseGarage()
    {
        _garage.SetActive(false);
    }

    

    void MoveNau()
    {
        //Debug.Log("estàs amb la nau");

        rb.linearVelocity = Vector3.zero;
        
        // això del playerInputAction
        Vector2 movement =inputManager.playerInputActions.Nau.MoveNau.ReadValue<Vector2>().normalized;
        float moveSpeed = 5f;
        //transform.Translate(new Vector3(movement.x, 0, movement.y));

        rb.MovePosition(rb.position+ new Vector3(movement.x, 0, movement.y) * moveSpeed *Time.deltaTime);
        
    }

    void MovePilot()
    {
        //Debug.Log("estàs amb el pilot");
        
        rb.linearVelocity = Vector3.zero;
        Vector2 movement =inputManager.playerInputActions.Pilot.MovePilot.ReadValue<Vector2>().normalized;
        float moveSpeed = 2f;
        //transform.Translate(new Vector3(movement.x, 0, movement.y));

        rb.MovePosition(rb.position+ new Vector3(movement.x, 0, movement.y) * moveSpeed *Time.deltaTime);
        
    }
    

    private void moveCapa(InputAction.CallbackContext context)
    {
       
        
        
        
        Debug.Log("rodeta ratolí detectat");
        float movement = inputManager.playerInputActions.Nau.CanviCapa.ReadValue<float>();
        
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

  

    public void AddPart(GameObject gb, Vector3 origin)
    {
        //Aquesta marabunda de codi funciona :D
        GameObject part = Instantiate(gb);
        Vector3 newOrigin = transform.position;
        Vector3 relative =gb.transform.position;
        Vector3 finalWorldPosition = relative -origin ;
        finalWorldPosition+=newOrigin;
        finalWorldPosition.y=_nau.transform.position.y;
        part.transform.position = finalWorldPosition;
        Transform ToAdd = _nau.transform.Find("Added");
        if (ToAdd != null)
        {
            part.transform.SetParent(ToAdd.transform, true);
        }
        Transform coll = part.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(true);
        
        AddedParts.Add(part);
    }
    
    
    public void RemovePart()
    {
        PartActions pa;

        Transform ToRemove = _nau.transform.Find("Added");
        foreach (Transform child in ToRemove.transform)
        {
            
            
            //potser el remove conexions hauria de ser el seu propi mètode
            pa = child.gameObject.GetComponent<PartActions>();
            switch (pa.GetTypePart())
            {
                case TypePart.Mele:
                    Debug.Log("part mele no acabat");
                    break;
                case TypePart.Moveable:
                    Debug.Log("part movable no acabat");
                    break;
                case TypePart.ShootableLeft:
                    inputManager.OnShotLeft -= pa.DoShot;
                    break;
                case TypePart.ShootableRight:
                    inputManager.OnShotRight -= pa.DoShot;
                    break;
            }
            
            
            Destroy(child.gameObject);
        }
        AddedParts.Clear();

       
    }

    public void ActivateParts()
    {
        PartActions pa;
        foreach (GameObject part in AddedParts)
        {
            //no estic segur de que part actions segui lo millor per invocar aquests mètodes,
            //revisar explicació escrita en EachPartScript per futur REFACTORITZACIÓ
            pa = part.GetComponent<PartActions>();
            switch (pa.GetTypePart())
            {
                case TypePart.Mele:
                    Debug.LogWarning("part mele no acabat");
                    break;
                case TypePart.Moveable:
                    Debug.LogWarning("part movable no acabat");
                    break;
                case TypePart.ShootableLeft:
                    inputManager.OnShotLeft += pa.DoShot;
                    break;
                case TypePart.ShootableRight:
                    inputManager.OnShotRight += pa.DoShot;
                    break;
            }
        }
    }


  
    
    /*void OnClickGarage(InputAction.CallbackContext context)
    {
        //OnClicked?.Invoke();
    }*/
    public void AddDialogueInfo(string branca, int mode)
    {
        this.branca = branca;
        this.mode = mode;   
    }

    
}
