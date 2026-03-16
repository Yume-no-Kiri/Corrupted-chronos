      

   using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ink.Parsed;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
   

    [Header("player v2 variables:")]
    [SerializeField] 
    private bool StartAsPilot = true;
    // [SerializeField]
    [SerializeField] 
    private bool StartInGarage = true;
    private ShipMovement shipMovement;
    private ShipLook shipLook;
    // [SerializeField]
    private PilotMovement pilotMovement;

    private PartAdder garageAdder;


    public InputManager inputManager { get; private set; }

    //if diferent naus and pilots, should change stats then i would get a script with stats change? 
    private GameObject _nauGO;
    private GameObject _pilotGO;
    //maybe canviar a garageAdder, pero resulta més fàcil aquí demoment 
    [SerializeField] private GameObject _adderGO;
    [SerializeField] private GameObject _garageGO;

    private PartsDatabaseSO _database;

    //interactive
    InteractiveMethods interactiveMethods;
    List<InteractionType> whatsToInteract;

    //dialge
    [HideInInspector] public string branca;
    [HideInInspector] public int mode;

    void Awake()
    {
        shipMovement=GetComponent<ShipMovement>();
        shipLook= GetComponent<ShipLook>();
        pilotMovement=GetComponent<PilotMovement>();
        garageAdder=GetComponent<PartAdder>();
        interactiveMethods= GetComponent<InteractiveMethods>();
   

    }
    void Start()
    {
        // if(shipMovement==null) Debug.LogError("aaaaAAAAAAAAA");
        
        inputManager=GameManager.Instance.inputManager;
        if(inputManager==null) Debug.LogError("LA CONCHA DE LA LORA");
        _database=GameManager.Instance.dataBaseParts;
       
        

        _nauGO = Instantiate(_database.AllNaus[0].PrefabGaratge, new Vector3(0, 0, 0), quaternion.identity);
        _pilotGO= Instantiate(_database.AllPilots[0].PrefabGaratge,new Vector3(0, 0, 0), quaternion.identity);
        _nauGO.transform.SetParent(this.transform,false);
        _pilotGO.transform.SetParent(this.transform,false);
        

        // _adderGO=_nauGO.transform.Find("AdderPart").gameObject;
        garageAdder.AssignAdder(_adderGO,_nauGO);
        // interactiveMethods= gameObject.AddComponent<InteractiveMethods>();
        whatsToInteract = new List<InteractionType>();

        inputManager.playerInputActions.Global.Enable();
        inputManager.playerInputActions.Global.Interactua.performed += Interact;

        // shipMovement.SetUpShipMovement(inputManager.playerInputActions.Nau);
        //hauria de fer el mateix per el moviment del pilot
        ChangePilot(StartAsPilot);
        ChangeNau(!StartAsPilot);
        ChangeGarage(StartInGarage);
        // ApplyChangeInputMap(StartAsPilot,false);
        /* if (StartAsPilot)
        {
            inputManager.playerInputActions.Nau.Disable();
            shipMovement.enabled=false;
            inputManager.playerInputActions.Pilot.Enable();
            pilotMovement.enabled=true;
            
            _pilotGO.SetActive(true);
            
        }
        else
        {
            inputManager.playerInputActions.Nau.Enable();
            shipMovement.enabled=true;
            inputManager.playerInputActions.Pilot.Disable();
            pilotMovement.enabled=false;

            _nauGO.SetActive(true);

        } */
        shipMovement.SetUpShipMovement(inputManager.playerInputActions.Nau);
        pilotMovement.SetUpPilotMovement(inputManager.playerInputActions.Pilot);
        // inputManager.playerInputActions.OnActionStatusChange += AccesChangeInputMap;
    }

    void Update()
    {
        
    }


    public void ChangeGarage(bool activateGarage)
    {
        if (activateGarage)
        {
            inputManager.playerInputActions.Garage.Enable();
            garageAdder.enabled=true;
            _garageGO.SetActive(true);
            //falta activar càmera garage i desactivar inmotion Camera

            //desactivar tot?

        }else{
            inputManager.playerInputActions.Garage.Disable();
            garageAdder.enabled=false;
            _garageGO.SetActive(false);
            //falta desactivar càmera garage i activar inmotion Camera
        }
    }

    public void ChangePilot(bool activate)
    {
        
        if (activate)
        {
           /*  inputManager.playerInputActions.Nau.Disable();
            shipMovement.enabled=false;
            shipLook.enabled=false;
            _nauGO.SetActive(false); */

            
            inputManager.playerInputActions.Pilot.Enable();
            pilotMovement.enabled=true;
            _pilotGO.SetActive(true);
        }
        else
        {
            /* inputManager.playerInputActions.Nau.Enable();
            shipMovement.enabled=true;
            shipLook.enabled=true;
            _nauGO.SetActive(true); */

            inputManager.playerInputActions.Pilot.Disable();
            pilotMovement.enabled=false;
            _pilotGO.SetActive(false);
            
        }
    }

    public void ChangeNau(bool activate)
    {
        if (activate)
        {
            inputManager.playerInputActions.Nau.Enable();
            shipMovement.enabled=true;
            shipLook.enabled=true;
            _nauGO.SetActive(true);
        }
        else
        {
            inputManager.playerInputActions.Nau.Disable();
            shipMovement.enabled=false;
            shipLook.enabled=false;
            _nauGO.SetActive(false);
        }
    }

    public void AccesChangeInputMap(NameInputAction inputMap, bool enableIt=true)
    {
         string mapName = inputMap.ToString();
        
        // InputActionMap targetMap = inputManager.playerInputActions.asset.FindActionMap(mapName);



        switch (inputMap)
        {
           
            case NameInputAction.Garage:
                if (enableIt)
                {
                    ChangeGarage(true);
                    // targetMap.Enable();
                }
                else
                {
                    ChangeGarage(false);

                    // targetMap.Disable();
                }
            break;
            case NameInputAction.Nau:
                if (enableIt)
                {
                    ChangeNau(true);
                    /* targetMap.Enable();
                    ChangePilot(false); */
                }
                else
                {
                    ChangeNau(false);
                    // targetMap.Disable();
                    
                }
            break;
            case NameInputAction.Pilot:
                if (enableIt)
                {
                    ChangePilot(true);
                    // // targetMap.Enable();
                    // ChangePilot(true);
                }
                else
                {
                    ChangePilot(false);
                    // targetMap.Disable();
                    
                }
            break;

            default:
            Debug.LogError("you should not be here");
            break;
        }


        // targetMap.Enable();
        //modificar el script que feia interaccions dinamiques

        // ApplyChangeInputMap();
       /* if (targetMap != null)
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
        */
        
    }
    
     //es crida cada vegada que hi ha un canvi en el inputs, no es pot fer automatic s'ha d'escriure per cridar-ho
    /* private void ApplyChangeInputMap()
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

        } 
    } */

    /* Maybe move to new script, for the moment here */
    

    public void CloseGarage()
    {
        // _garage.SetActive(false);
    }

    #region Interaccions dinamiques
    //Interacció amb objectes del escenari o coses especials
    void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("you press F");
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
    public void AddDialogueInfo(string branca, int mode)
    {
        this.branca = branca;
        this.mode = mode;   
    }
    #endregion

    public Camera returnCameraGarage()
    {
        return _garageGO.GetComponentInChildren<Camera>();
    }
    /* public GameObject returnNau()
    {
        return _nauGO;
    } */
}
