using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ink.Parsed;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //TODO
    //revisar la separació entre aquest script player i inputmanager, aquest script encara controla massa coses 
    //separar stats en un arxiu public que pugui ser modificat per parts de nau i altres components 
    //reworkejar moviment a little bit (quan et mous es nota que va a salts), maybe incorporar leap
    
    //físiques 
    private Rigidbody rb;
   
    
    //Controls/inputs
    //private PlayerInput playerInput;
    [SerializeField]
    public GameObject inputMangerObject;
    
    public InputManager inputManager;

    //moure capa
    private bool lockUp;
    private bool lockDown;
    private float moveDuration;
    private Vector3 targetPosition;
    private bool isMoving;
    private bool isBarrelRoll;

    bool keepDown=false;
    Coroutine goDownCoroutine;
    private Vector3 _newMovePosition;
    private Vector3 _GravityForce;
    private Vector3 _TotalForces;


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

    private Quaternion mouseRotation;       

    private Vector3 mousePos;
    private Vector3 mousedir;
    Quaternion targetRotation ;
     Vector3 mouseWorldPosition;
    float angle;
    // private Quaternion mouseRotation;       
    // public float rotationSpeed = 0.5f;


    private bool _gravity=false;
    //[SerializeField]
    //private Camera camGarage;

    //parts stuff
    private List<GameObject> AddedParts;
    
    private List<GameObject> DetectorsCapa;
    //Dialeg:
    public string branca;
    public int mode;


    ///Variables a moure al singleton

    private float moveSpeedNau=10f;
    private float moveSpeedPilot=4f;


    //hauria d'haver algo per a que passat x umbrals passi x coses, segons coses afegides

    
    //todo arnau: revisar el mètode awake
    private void Awake()
    {
        //crec que hauria d'estructurar, que va a awake i que a start
        inputManager = inputMangerObject.GetComponent<InputManager>();
        
        if (inputManager == null){Debug.LogError("aaaaAAAA inputmanager");}
        
        //varaibles generals
        moveDuration = 0.1f;
        isMoving = false;
        // isBoosting=false;
        elapsedTime = 0f;
        lockUp = false;
        lockDown = false;
        
        
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
        _nau = Instantiate(_database.AllNaus[0].PrefabGaratge, new Vector3(0, 0, 0), quaternion.identity);
        _pilot= Instantiate(_database.AllPilots[0].PrefabGaratge,new Vector3(0, 0, 0), quaternion.identity);
        _nau.transform.SetParent(this.transform,false);
        _pilot.transform.SetParent(this.transform,false);
        

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
        DetectorsCapa = new List<GameObject>();
        
    }

    #region Start
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
        // inputManager.playerInputActions.Nau.CanviCapa.performed += moveCapa;

        inputManager.playerInputActions.Nau.Up.performed+=GoUp;

        inputManager.playerInputActions.Nau.Down.started+=GoDown;
        inputManager.playerInputActions.Nau.Down.performed+=GoDown;
        inputManager.playerInputActions.Nau.Down.canceled+=GoDown;


        inputManager.playerInputActions.Nau.Barrelroll.started+=DoBarrelRoll;
        inputManager.playerInputActions.Nau.Barrelroll.performed+=DoBarrelRoll;
        inputManager.playerInputActions.Nau.Barrelroll.canceled+=DoBarrelRoll;


        inputManager.playerInputActions.Global.Enable();
        inputManager.playerInputActions.Global.Interactua.performed += Interact;
        
        //això demoment activat al inici per fer proves
        inputManager.playerInputActions.Garage.Disable();
        //s'actualitza quan moc el mouse
        inputManager.playerInputActions.Global.MousePosition.performed += PositionMouse;
        //inputManager.playerInputActions.Garage.OnClick.performed += OnClickGarage;

        // inputManager.playerInputActions.Nau.
        //    InputActionChange input+= HangleChangeInputMap;

        ApplyChangeInputMap();


    }
    #endregion

    #region Update 
    void Update()
    {
        ApplyGravity();

        /* if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            if (t >= 1f)
            {
                Debug.Log("enter if in the fixed update ");
                isMoving = false; // Movement finished
            }
        } */
        /* transform.rotation =Quaternion.Euler(0f, 0f, angle);
        Debug.DrawLine(transform.position, mouseWorldPosition, Color.white, Time.deltaTime); */
        if (mousedir != Vector3.zero)
        {
            // GameManager.Instance.rotationSpeed;
            // 4. Aplicar la rotació en l'eix Y (el que fa girar horitzontalment)
            // transform.rotation= Quaternion.LookRotation(mousedir);

            float dot= Vector3.Dot(transform.right, mousedir.normalized);
            if (dot > 0)
            {
                 Debug.Log("rotationSpeed  ___>0___");
                GameManager.Instance.rotationSpeed= GameManager.Instance.rotationSpeedRight ;
            }
            if (dot < 0)
            {
                Debug.Log("rotationSpeed  ___<0___");
                GameManager.Instance.rotationSpeed= GameManager.Instance.rotationSpeedLeft ;
                
            }

            // GameManager.Instance.rotationSpeed = (dot > 0) ?  GameManager.Instance.rotationSpeedRight :  GameManager.Instance.rotationSpeedLeft;
            Debug.Log("rotationSpeed, dot:"+ dot+" speed"+GameManager.Instance.rotationSpeed);

            targetRotation = Quaternion.LookRotation(mousedir);
            transform.rotation = Quaternion.RotateTowards(
            transform.rotation, 
            targetRotation, 
            GameManager.Instance.rotationSpeed * Time.deltaTime
            );
        }
        Debug.DrawRay(transform.position, transform.right * 2f, Color.red);
        Debug.DrawLine(transform.position, mouseWorldPosition, Color.white);

        if (!inputManager.playerInputActions.Nau.enabled) return;
        if (lockDown)// && !isBoosting)
        {
            Debug.Log("GRAVITY off");

            _gravity = false;
 
        }else
        {
            if (GameManager.Instance.staminaInUse == false)
            {
                // _gravity=true;

                StartCoroutine(ActivateGravity());
            }
            //gastar energia per mantenir-se en la capa
        }


        }
    #endregion

    #region FixedUpdate

    void FixedUpdate()
    {

        //això s'haurà de moure al seu script rotar mira
        
        // transform.rotation = Quaternion.Lerp(transform.rotation, mouseRotation, Time.deltaTime *  GameManager.Instance.rotationSpeed);
    
        GetInputPlayer();

        if(keepDown){
            goDownCoroutine=StartCoroutine(GoDownCoroutine());
        }



        //aquí aplica el moviment, probablement moure en un script separat

        //sumem forces
        //is barrel roll on, stoping all other forces
        if (isBarrelRoll)
        {
            if (_newMovePosition == Vector3.zero)
            {
                //que? mou enrere o 
            }

            rb.AddForce(_newMovePosition*GameManager.Instance.dashingForce);
        }
        else
        {
            _TotalForces=_newMovePosition+_GravityForce;

            // rb.MovePosition(rb.position + _TotalForces);
            rb.transform.position+=40* _TotalForces*Time.deltaTime;
            _newMovePosition = Vector3.zero;
            _GravityForce = Vector3.zero;
        }

        toFly();
    }
    #endregion
   

    #region changeInputMap
    public void AccesChangeInputMap(NameInputAction inputMap, bool enableIt)
    {
        string mapName = inputMap.ToString();
        
        InputActionMap targetMap = inputManager.playerInputActions.asset.FindActionMap(mapName);

        if (targetMap != null)
        {
            if (enableIt)
            {
                targetMap.Enable();
                ApplyChangeInputMap();
            }
            else
            {
                targetMap.Disable();
                ApplyChangeInputMap();
            }

        }
        else
        {
            Debug.LogError("wtf input map no detectat erroooooor");
        }
    
        
    }

    //es crida cada vegada que hi ha un canvi en el inputs, no es pot fer automatic s'ha d'escriure per cridar-ho
    private void ApplyChangeInputMap()
    {
        if (inputManager.playerInputActions.Nau.enabled)
        {
            _playerCamera.enabled = true;
            _garageCamera.enabled = false;

            //fix, em sortia un missatge estrany d'error
            _playerCamera.GetComponent<AudioListener>().enabled = true;
            _garageCamera.GetComponent<AudioListener>().enabled = false;

            _nau.SetActive(true);
            _pilot.SetActive(false);

        }
        else if (inputManager.playerInputActions.Pilot.enabled)
        {
            _playerCamera.enabled = true;
            _garageCamera.enabled = false;

            _playerCamera.GetComponent<AudioListener>().enabled = true;
            _garageCamera.GetComponent<AudioListener>().enabled = false;

            _nau.SetActive(false);
            _pilot.SetActive(true);


        }
        else if (inputManager.playerInputActions.Garage.enabled)
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
    }
    public void CloseGarage()
    {
        _garage.SetActive(false);
    }

    #endregion

    
    #region moviment basic
    //canviar a public si es el cas
    private void GetInputPlayer()
    {
        if (inputManager.playerInputActions.Nau.enabled)
        {
            GetInputNau();
        }
        else if (inputManager.playerInputActions.Pilot.enabled)
        {
            GetInputPilot();
        }
    }
    private void GetInputNau()
    {
        //Debug.Log("estàs amb la nau");

        rb.linearVelocity = Vector3.zero;
        
        Vector2 movement =inputManager.playerInputActions.Nau.MoveNau.ReadValue<Vector2>().normalized;

        /* si afegim speed d'alguna mena 
        //is boosting movespeed=2, else movespeed=4
        // float moveSpeed=isBoosting ? 10f: 4f;
        */

        _newMovePosition+= new Vector3(movement.x, 0, movement.y) * moveSpeedNau *Time.deltaTime;
        
    }

    private void GetInputPilot()
    {
        //Debug.Log("estàs amb el pilot");
        
        rb.linearVelocity = Vector3.zero;
        Vector2 movement =inputManager.playerInputActions.Pilot.MovePilot.ReadValue<Vector2>().normalized;

        _newMovePosition+= new Vector3(movement.x, 0, movement.y) * moveSpeedPilot *Time.deltaTime;
        
    }
    #endregion

    //acabar revisió de moviment
    #region Moviment més complexe

    private void ApplyGravity()
    {
        if (_gravity)
        {

            //depen de com es vuglusi jugar amb la gravetat, si aquesta creix quan no estem entre enters i disminueix quan estem en enters
            
            float decimalPart =  rb.transform.position.y % 1.0f;
            // float resultat = Mathf.Abs((decimalPart * 2) - 1);
            float resultat =Mathf.Lerp(1f,0.2f, (Mathf.Cos(decimalPart * 2 * Mathf.PI) + 1f) / 2f);
            print("gravity result"+ resultat);
            // if(resultat<=0.2) resultat=0.2f;
            _GravityForce += Vector3.down * resultat*Time.deltaTime;
            
            // _newMovePosition += Vector3.down *Time.deltaTime;

        }
    }

    private void PositionMouse(InputAction.CallbackContext context)
    {
       // Debug.Log("AAAAAAAAAA");
        
        print("t'executes?");
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

        }else if (inputManager.playerInputActions.Nau.enabled)
        {
        
            Ray ray = _playerCamera.ScreenPointToRay(mousePos);
    
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                // Aquesta és la posició real del ratolí en el món 3D
                mouseWorldPosition = ray.GetPoint(rayDistance);

                // 3. Calcular la direcció (ignorant l'altura Y per no inclinar el personatge)
                mousedir = mouseWorldPosition - transform.position;
                mousedir.y = 0; 

                
                //smoth
                /* float smoothSpeed = Mathf.Lerp(lastSpeed, speedTarget, Time.deltaTime * 5f);
                lastSpeed = smoothSpeed; */
            }

            
        }
    }
    
    //probablement modificar per suavitzar una mica
    /* private void setPositionToMove()
    {
        Vector3 pos= transform.position;
        transform.position = new Vector3(pos.x, Mathf.RoundToInt(pos.y), pos.z); 
    } */

    private void GoUp(InputAction.CallbackContext context)
    {
        //reaprofitar parcialment move capa
        //si shift apretat no gravetat
     
        // setPositionToMove();
        if (lockUp==true )//&& !hitsBelow.Contains(myCollider))
        {
            Debug.Log("enter is lockUP");
            return;
        }
        _gravity=false;
        _newMovePosition+= new Vector3(0, 1, 0);

        GameManager.Instance.MoveUpStamina();
    }

  
    private void GoDown(InputAction.CallbackContext context)
    {
        //Potser estaria bé que quan baixem deixem de gastar stamina per 0.5 segons

        // setPositionToMove();
        if (context.started)
        {
            Debug.Log("enter started");

            if (lockDown)// && !hitsBelow.Contains(myCollider))
            {
                Debug.Log("enter is lockLOW");
                keepDown=false; 
                /* if (goDownCoroutine != null) {
                    StopCoroutine(goDownCoroutine);
                } */
                return;
            }
            // _newMovePosition+= new Vector3(0, -1, 0);
            _newMovePosition+= new Vector3(0, -1, 0);
            keepDown=true;

        }

        if (context.canceled)
        {
            Debug.Log("enter canceled");
            keepDown=false;
            if (goDownCoroutine != null) {
                StopCoroutine(goDownCoroutine);
            }
        }


        // rb.MovePosition(rb.position + new Vector3(0, -1, 0));
    }
    //anar a baix mantenint el boto
    IEnumerator GoDownCoroutine()
    {
        yield return new WaitForSeconds(1.3f);
        if (lockDown){
            keepDown=false; 
        }else{
            _newMovePosition+= new Vector3(0, -1, 0);
        }
        // goDownCoroutine=StartCoroutine(GoDownCoroutine());
    }

    IEnumerator ActivateGravity()
    {
        yield return new WaitForSeconds(0.5f);
        _gravity=true;
    }
   
    private void DoBarrelRoll(InputAction.CallbackContext context)
    {
        isBarrelRoll=true;
        StartCoroutine(BarrelRollActive());

        //ara mateix barrelRoll hauria de ser petit dash cap a una direcció
        /* boost de velocitat, desactualitzat
        if (context.started)
        {
            Debug.Log("is    boosting");
            isBoosting=true;
        }else if (context.canceled)
        {
            Debug.Log("isn't boosting");
            isBoosting=false;
        } */
    }

    IEnumerator BarrelRollActive()
    {
        yield return new WaitForSeconds(0.5f);
        isBarrelRoll=false;
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
            if (lockDown==true)// && !hitsBelow.Contains(myCollider))
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

    void toFly()
    {
        if (!lockDown)
        {
            GameManager.Instance.want2Fly=true;
        }
        else
        {
            GameManager.Instance.want2Fly=false;
        }
    }

    #endregion
    #region Capes Detector

    public void DetectorCapaResponse(DetectCanviCapaType detect, bool isLocked, GameObject detector)
    {
        if (detect == DetectCanviCapaType.Up)
        {
            lockUp = isLocked;
        }else if (detect == DetectCanviCapaType.Low)
        {
            lockDown = isLocked;
        }

        if (!DetectorsCapa.Contains(detector))
        {
            DetectorsCapa.Add(detector);
        }
        
    }


    //es millor revisar les leyers de fisiques que implementar aquest mètode
    public void ClearDetectorsCapa()
    {
        if (DetectorsCapa != null && DetectorsCapa.Count > 0)
        {
            foreach (var detect in DetectorsCapa)
            {
                DetectorCapa script = detect.GetComponent<DetectorCapa>();
                if (script != null) script.ClearObjects();
            }
        }
    }



    #endregion

    
    #region Interaccions dinamiques
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
        if (!whatsToInteract.Contains(type))
        {
            whatsToInteract.Add(type);

        }
    }

    
    public void SubInteraction(InteractionType type)
    {
        if (whatsToInteract.Contains(type))
        {
            whatsToInteract.Remove(type);
        }
        
    }

    #endregion


    #region called from placement system


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
            
            pa = child.gameObject.GetComponent<PartActions>();
            switch (pa.GetTypePart())
            {
                case TypePart.Mele:
                    Debug.LogError("part mele no acabat");
                    break;
                case TypePart.Moveable:
                    Debug.LogError("part movable no acabat");
                    break;
                case TypePart.ShootableLeft:
                    inputManager.OnShotLeft -= pa.DoShot;
                    break;
                case TypePart.ShootableRight:
                    inputManager.OnShotRight -= pa.DoShot;
                    break;
            }
            
            GameObject col=  child.transform.Find("Collisions").gameObject;
            if (col != null)
            {
                //List<GameObject> dettors = new List<GameObject>();
                DetectorCapa[] script= col.transform.GetComponentsInChildren<DetectorCapa>();
                foreach (var s in script)
                {
                    if (DetectorsCapa.Contains(s.gameObject))
                    {
                        DetectorsCapa.Remove(s.gameObject);
                    }
                }
                
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


    #endregion
  
    
     public void AddDialogueInfo(string branca, int mode)
    {
        this.branca = branca;
        this.mode = mode;   
    }

    
}
