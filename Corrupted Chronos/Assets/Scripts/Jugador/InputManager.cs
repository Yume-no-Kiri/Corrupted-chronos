using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    
    public PlayerInputActions playerInputActions;

    public Vector3 MousePosition;

    
    public event Action OnClick, OnExit;

    
    private void Awake()
    {
        
        playerInputActions = new PlayerInputActions();
        playerInputActions.Garage.OnClick.performed += CallOnClick;
        playerInputActions.Garage.OnExit.performed += CallOnExit;

    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public bool IsPointerOverUI()=>EventSystem.current.IsPointerOverGameObject();
    void CallOnClick(InputAction.CallbackContext callbackContext)
    {
        OnClick?.Invoke();
    }
    private void CallOnExit(InputAction.CallbackContext obj)
    {
        OnExit?.Invoke();
    }

}
