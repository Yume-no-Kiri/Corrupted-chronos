using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    
    public PlayerInputActions playerInputActions;

    
    public Vector3 MousePosition;

    
    public event Action OnClick, OnExit;
    public event Action<InputAction.CallbackContext> OnShotLeft, OnShotRight;
    public event Action<InputAction.CallbackContext> RotateLeft, RotateRight;


    
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Garage.OnClick.performed += CallOnClick;
        playerInputActions.Garage.OnExit.performed += CallOnExit;
        playerInputActions.Nau.OnShotLeft.performed +=ctx=> OnShotLeft?.Invoke(ctx);
        playerInputActions.Nau.OnShotRight.performed += ctx=> OnShotRight?.Invoke(ctx);

        playerInputActions.Garage.RotateLeft.performed +=ctx=> RotateLeft?.Invoke(ctx);
        playerInputActions.Garage.RotateRight.performed += ctx=> RotateRight?.Invoke(ctx);
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
    
    
    
}
