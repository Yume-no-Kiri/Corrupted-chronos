using UnityEngine;
using UnityEngine.InputSystem;

public class Moviment : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 movement;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       


        //float moveX = Input.GetAxisRaw("Horizontal");
        //float moveZ = Input.GetAxisRaw("Vertical");
        //movement = new Vector3(moveX, 0f, moveZ).normalized;

    }

    void FixedUpdate()
    {
        // Apply movement
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void movementinputAAA(InputAction.CallbackContext context)
    {
        Debug.Log("move input detected");
        if (context.performed)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * moveSpeed, ForceMode.Force);
        }
        //Vector2 inputVector = RTeadValue<Vector2>();
        //rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * moveSpeed, ForceMode.Force);
    }
}
