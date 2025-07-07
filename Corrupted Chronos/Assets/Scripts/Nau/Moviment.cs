using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Moviment : MonoBehaviour
{
    
    
    
    //físiques 
    private Rigidbody rb;
    [SerializeField] 
    private float checkRadius = 0.45f;
    [SerializeField] 
    private LayerMask collisionMask;
    
    //Controls/inputs
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    //moure capa
    //no segur del private, recomanció de rider
    private Collider[] hitsAbove;
    private Collider[] hitsBelow;
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
        //si necesito actualitzar això en el menú, a fixed Update
        hitsAbove = Physics.OverlapSphere(new Vector3(rb.position.x,rb.position.y+1,rb.position.z), checkRadius, collisionMask);
        hitsBelow = Physics.OverlapSphere(new Vector3(rb.position.x,rb.position.y-1,rb.position.z), checkRadius, collisionMask);

        Vector2 movement =playerInputActions.Nau.Move.ReadValue<Vector2>().normalized;
        float moveSpeed = 5f;
        //transform.Translate(new Vector3(movement.x, 0, movement.y));

        rb.MovePosition(rb.position+ new Vector3(movement.x, 0, movement.y) * moveSpeed *Time.deltaTime);
    }
    void OnDrawGizmos()
    {
        if (rb == null) return; // Avoid errors if not set in Inspector
        Vector3 targetPos = rb.position + Vector3.down;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPos, checkRadius);
        Vector3 targetPos1 = rb.position + Vector3.up;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPos1, checkRadius);
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

        if (movement > 0)
        {
            if (hitsAbove.Length > 0)
            {
                Debug.Log("Can't move ABOVE. Blocked by:");

                foreach (Collider col in hitsAbove)
                {
                    Debug.Log("- " + col.gameObject.name + " (tag: " + col.tag + ")");
                }

                return;
            }
        }else if (movement < 0)
        {
            if (hitsBelow.Length > 0)
            {
                Debug.Log("Can't move BELOW. Blocked by:");

                foreach (Collider col in hitsBelow)
                {
                    Debug.Log("- " + col.gameObject.name + " (tag: " + col.tag + ")");
                }

                return;
            }
        }
        
        Vector3 newPosition = rb.position + new Vector3(0, movement, 0);
        //transform.Translate(new Vector3(0, movement, 0));
        rb.MovePosition(newPosition);
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
