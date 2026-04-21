using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    
    public PlayerInputActions playerInputActions;

    
    //potser això canviar-ho de lloc
    public Vector3 MousePositionGarage;
    // public SlotInventory[] LockSlotSelected;
    // public SlotInventory SlotSelected;
    // public Vector3 MousePositionInventory;


    private Camera cameraGarage;
    [SerializeField]
    private Camera cameraGameplay;
    private Vector3 mousePos;
    
    
    // public event Action<ItemData> AddNewItem;
    public event Action OnClick, OnExit;
    public event Action<InputAction.CallbackContext> OnShotLeft, OnShotRight;
    public event Action<SlotInventory> PrimaryClick, SecondaryClick;

    public event Action<InputAction.CallbackContext> RotateLeft, RotateRight;
    
    public event Action OpenInventory;

    
    private void Awake()
    {
        // SlotSelected=null;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Garage.OnClick.performed += CallOnClick;
        playerInputActions.Garage.OnExit.performed += CallOnExit;
        playerInputActions.Nau.OnShotLeft.performed +=ctx=> OnShotLeft?.Invoke(ctx);
        playerInputActions.Nau.OnShotRight.performed += ctx=> OnShotRight?.Invoke(ctx);
        
        playerInputActions.AccesInventory.OpenInventory.performed += ctx=>OpenInventory?.Invoke();

        playerInputActions.Garage.RotateLeft.performed +=ctx=> RotateLeft?.Invoke(ctx);
        playerInputActions.Garage.RotateRight.performed += ctx=> RotateRight?.Invoke(ctx);

        playerInputActions.Inventory.PrimaryClick.performed += ctx=>PrimaryClick?.Invoke(GetUIObjectSotaRatoli());
        playerInputActions.Inventory.SecondaryClick.performed += ctx=>SecondaryClick?.Invoke(GetUIObjectSotaRatoli());


        // LockSlotSelected= new SlotInventory[2];
        // playerInputActions.
        // OnActionStatusChange
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraGarage=GameManager.Instance.playerInstance.GetComponent<Player>().returnCameraGarage();
    }

    // Update is called once per frame
    void Update()
    {
       
        mousePos = playerInputActions.Global.MousePosition.ReadValue<Vector2>();

        if (playerInputActions.Garage.enabled)
        {
            //Camera camGarage = _garage.GetComponentInChildren<Camera>();
            
            mousePos.z = cameraGarage.nearClipPlane;
        
            Ray ray = cameraGarage.ScreenPointToRay(mousePos);
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Default");
        
            if (Physics.Raycast(ray, out hit,300,mask))
            {
                MousePositionGarage= hit.point;
                //Debug.Log("BBBBBBBB");
            }
        }
        
       /*      print (EventSystem.current.IsPointerOverGameObject()? "eo detects ui": "eo not detects ui" );
        if (EventSystem.current.IsPointerOverGameObject())
        {
            GetUIObjectSotaRatoli();
 */
            // Debug.Log("eo mous inventory calls"+.name);
            // mousePos.z = cameraGameplay.nearClipPlane;
        


           /*  Ray ray = cameraGameplay.ScreenPointToRay(mousePos);
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("UI");
            if (Physics.Raycast(ray, out hit,300,mask))
            {
                MousePositionInventory=hit.point;
                Debug.Log("eo mous point:"+hit.transform.position);
                if (hit.transform.CompareTag("Slot"))
                {
                    Debug.LogWarning("eo detecta slot");
                }
            } */
            
/*         }
 */            // {
       
    }
    
/*     private void OnMouseDrag() {
        Debug.LogWarning("I am draging");   
    }
 */

    //odio aquest puto mètode
    //Millorar en un futur
    
    private SlotInventory GetUIObjectSotaRatoli()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();
        List<RaycastResult> resultats = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, resultats);

        SlotInventory slotActual=null;
        foreach (var item in resultats)
        {
            Debug.Log("eo jdetect "+item.gameObject.name);
            // if (item.gameObject.name == "Marc")
            // {
                slotActual=item.gameObject.GetComponentInParent<SlotInventory>();
                // Debug.Log("eo slotSelected"+ SlotSelected.name);
                // if(slotActual==false) //Debug.LogError("aaaaaaaa");
                return slotActual;
            // }
            // else
            // {
            //    Debug.Log("eo not slotSelected"+ SlotSelected);

                // SlotSelected=null;
            // }
            // Debug.Log("eo slotSelected"+ SlotSelected);
            // if(item.gameObject.GetComponent())
        }
        return null;

        // Si la llista té algun element, el primer [0] és el que està més a sobre
        // return resultats.Count > 0 ? resultats[0].gameObject : null;
    }


    //crec que això es per el garatge
    public bool IsPointerOverUI()=>EventSystem.current.IsPointerOverGameObject();
    void CallOnClick(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
            StartCoroutine(DelayedClick());
       // OnClick?.Invoke();
    }
    private IEnumerator DelayedClick()
    {
        yield return null; // Wait one frame
        OnClick?.Invoke(); // Now this is called AFTER UI system updated
    }
    
    
    private void CallOnExit(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
            StartCoroutine(DelayedExit());
        //OnExit?.Invoke();
    }
    private IEnumerator DelayedExit()
    {
        yield return null; // Wait one frame
        OnExit?.Invoke(); // Now this is called AFTER UI system updated
    }
    
    
    /* public void CallAddNewItem(ItemData item)
    {
        AddNewItem?.Invoke(item);
    } */



}
