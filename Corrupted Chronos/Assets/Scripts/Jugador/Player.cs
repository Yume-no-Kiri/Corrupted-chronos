      

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
    private InventoryManager inventoryManager;

    public InputManager inputManager { get; private set; }

    //if diferent naus and pilots, should change stats then i would get a script with stats change? 
    private GameObject _nauGO;
    private GameObject _pilotGO;
    //maybe canviar a garageAdder, pero resulta més fàcil aquí demoment 
    [SerializeField] private GameObject _adderGO;
    [SerializeField] private GameObject _garageGO;
    [SerializeField] private GameObject _inventoryGO;


    private PlacementDatabaseSO _database;

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
        inventoryManager= GetComponent<InventoryManager>();

    }
    void Start()
    {        
        inputManager=GameManager.Instance.inputManager;
        if(inputManager==null) Debug.LogError("LA CONCHA DE LA LORA");
        _database=GameManager.Instance.placementDataBase;

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

        ChangePilot(StartAsPilot);
        ChangeNau(!StartAsPilot);
        ChangeGarage(StartInGarage);
        ChangeInventory(false);
        shipMovement.SetUpShipMovement(inputManager.playerInputActions.Nau);
        pilotMovement.SetUpPilotMovement(inputManager.playerInputActions.Pilot);
    
         GameManager.Instance.inputManager.OpenInventory+=ChangeInventory;
    }


    void Update()
    {
        
    }

   
 


    #region Interaccions dinamiques
    private void ChangeInventory()
    {
        //actual activemap should be saved to be deactive or activate later
        //should pause the game
        if (inputManager.playerInputActions.Inventory.enabled)
        {
            AccesChangeInputMap(NameInputAction.Inventory,false);
            AccesChangeInputMap(NameInputAction.Nau,true);

        }else
        {
            AccesChangeInputMap(NameInputAction.Nau,false);
            AccesChangeInputMap(NameInputAction.Inventory,true);
        }
        
    }

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

    public PlacementSystem returnPlacementSystem()
    {
        return _garageGO.GetComponent<PlacementSystem>();
    }

     #region change map/move to interactive methods o new script
    public void ChangeGarage(bool activateGarage)
    {
        if (activateGarage)
        {
            inputManager.playerInputActions.Garage.Enable();
            garageAdder.enabled=true;
            _garageGO.SetActive(true);
           

        }else{
            inputManager.playerInputActions.Garage.Disable();
            garageAdder.enabled=false;
            _garageGO.SetActive(false);
        }
    }

    public void ChangePilot(bool activate)
    {
        
        if (activate)
        {
            inputManager.playerInputActions.Pilot.Enable();
            pilotMovement.enabled=true;
            _pilotGO.SetActive(true);
        }
        else
        {
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

    public void ChangeInventory(bool activate)
    {
        if (activate)
        {
            inputManager.playerInputActions.Inventory.Enable();
            inventoryManager.OnActivate();
            // inve.enabled=true;
            // shipLook.enabled=true;
            _inventoryGO.SetActive(true);
        }
        else
        {
            inputManager.playerInputActions.Inventory.Disable();
            inventoryManager.OnDeactivate();
            // shipMovement.enabled=false;
            // shipLook.enabled=false;
            _inventoryGO.SetActive(false);
        }
    }
    public void AccesChangeInputMap(NameInputAction inputMap, bool enableIt=true)
    {
        //  string mapName = inputMap.ToString();
        switch (inputMap)
        {
            case NameInputAction.Garage:
                if (enableIt)  ChangeGarage(true);
                else ChangeGarage(false);
            break;
            case NameInputAction.Nau:
                if (enableIt) ChangeNau(true);
                else ChangeNau(false);
            break;
            case NameInputAction.Pilot:
                if (enableIt)ChangePilot(true);
                else ChangePilot(false);                    
            break;
            case NameInputAction.Inventory:
                if (enableIt)ChangeInventory(true);
                else ChangeInventory(false);                    
            break;
            default:
            Debug.LogError("you should not be here");
            break;
        }
    }

    #endregion


}
