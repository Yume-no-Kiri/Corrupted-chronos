using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Moviment : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        
        playerInputActions = new PlayerInputActions();
        playerInputActions.Nau.Enable();
        playerInputActions.Nau.CanviCapa.performed += moveCapa;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector2 movement =playerInputActions.Nau.Move.ReadValue<Vector2>().normalized;
        //float moveSpeed = 5f;
        //rb.AddForce(new Vector3(movement.x, 0, movement.y) * moveSpeed, ForceMode.Force);
        transform.Translate(new Vector3(movement.x, 0, movement.y));

    }


    // Update is called once per frame
    void Update()
    {
        //float moveX = Input.GetAxisRaw("Horizontal");
        //float moveZ = Input.GetAxisRaw("Vertical");
        //movement = new Vector3(moveX, 0f, moveZ).normalized;

    }

    private void moveCapa(InputAction.CallbackContext context)
    {
        float movement = playerInputActions.Nau.CanviCapa.ReadValue<float>();
        
        transform.Translate(new Vector3(0, movement, 0));
        //rb.AddForce(new Vector3(0, movement,0), ForceMode.Force);
        //Debug.Log("canvi capa"+ movement);

    }

    /*public void movementinputAAA(InputAction.CallbackContext context)
    {
        Debug.Log("move input detected");
        if (context.performed)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * moveSpeed, ForceMode.Force);
        }
        //Vector2 inputVector = RTeadValue<Vector2>();
        //rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * moveSpeed, ForceMode.Force);
    }*/
}
