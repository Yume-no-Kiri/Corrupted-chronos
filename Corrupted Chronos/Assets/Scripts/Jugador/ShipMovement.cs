using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ShipMovement : MonoBehaviour,IDamageable 
{
    [Header("Parameters")]
    public bool canMove=true;
    public bool canBoost;

    /* [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference boostAction; */

    private InputAction moveAction= new InputAction();
    private InputAction moveDown= new InputAction();

    private InputAction moveUp= new InputAction();

    private InputAction boostAction= new InputAction();


    // [Header("Movement")]
    // private float moveSpeed;
    // [SerializeField] private float boostMultiplier = 2f;

    [Header("Smoothing")]
    [SerializeField] private float directionSmoothing = 10f;
    [SerializeField] private float boostSmoothing = 5f;

    private CharacterController controller;

    private Vector3 currentMoveVector;
    private Vector3 targetMoveVector;

    private float currentSpeedMultiplier = 1f;
    private float targetSpeedMultiplier = 1f;
    private Vector3 externalForce= Vector3.zero;


    private void Awake()
    {
        controller = GetComponentInParent<CharacterController>();
        if(controller) Debug.Log("ss controller assigned");
        else Debug.Log("ss controller not assigned");
    }

    public void SetUpShipMovement(PlayerInputActions.NauActions nau)
    {
        moveAction=nau.MoveNau;
        moveDown=nau.Down;
        moveUp=nau.Up;
        Debug.Log("enter setup movement");

    }
    private void OnEnable()
    {
        /* moveAction.action.Enable();
        boostAction.action.Enable(); */
    }

    private void OnDisable()
    {
        /* moveAction.action.Disable();
        boostAction.action.Disable(); */
    }

    private void Update()
    {
        // Debug.Log("ss does update work? 1");
        if (!canMove)
        {    return;}
        ReadInput();
        SmoothDirection();
        if (canBoost)
        {   SmoothBoost();}

        if (externalForce.magnitude > 0.01f)
        {
            externalForce = Vector3.Lerp(externalForce, Vector3.zero, GameManager.Instance.knockbackResistance * Time.deltaTime);
        }
        else
        {
            externalForce = Vector3.zero; // Forcem el zero per estalviar càlculs quan és molt petita
        }

        ApplyMovement();
        // Debug.Log("ss does update work? 2");

    }

    private void ReadInput()
    {
        // moveSpeed=GameManager.Instance.moveSpeedNau;
        Vector2 inputMove=Vector2.zero;
        //Debug.Log("ss Do you enter");
        if (moveAction != null)
        {
            inputMove = moveAction.ReadValue<Vector2>();
            //Debug.Log("ss move it move it"+inputMove.ToString());

        }
        
        float inputDownUp=0;
        if(moveDown!=null || moveUp != null){        
            inputDownUp =- moveDown.ReadValue<float>();
            inputDownUp += moveUp.ReadValue<float>();
            if (inputDownUp > 0){

                GameManager.Instance.MoveUpStamina();
            }
            //Debug.Log("ss move down UP"+inputDownUp.ToString());

        }


        targetMoveVector = new Vector3(inputMove.x, inputDownUp, inputMove.y);

        if (targetMoveVector.sqrMagnitude > 1f)
        { targetMoveVector.Normalize();}
        // bool isBoosting = boostAction.IsPressed();


        // targetSpeedMultiplier = isBoosting ? boostMultiplier : 1f;
    }

    private void SmoothDirection()
    {
        currentMoveVector = Vector3.Lerp(
            currentMoveVector,
            targetMoveVector,
            directionSmoothing * Time.deltaTime
        );
    }
    private void SmoothBoost()
    {
        currentSpeedMultiplier = Mathf.Lerp(
            currentSpeedMultiplier,
            targetSpeedMultiplier,
            boostSmoothing * Time.deltaTime
        );
    }

    private void ApplyMovement()
    {
        Vector3 finalVelocity = currentMoveVector * statsManager.instance.GetShipStat(Stat.StatTypeGeneral.Speed) * currentSpeedMultiplier+externalForce;


        controller.Move(finalVelocity * Time.deltaTime);
        // controller.Move(new Vector3(5, 0, 0) * Time.deltaTime);
        // Debug.Log("ss final velocity:"+ finalVelocity.ToString());
    }
    #region interface
    public void TakeDamage(float damageAmount)
    {
        statsManager.instance.AddModifier(Stat.StatTypeGeneral.Health, new StatModifier(-damageAmount, StatModifier.ModifierType.Add));
        // currentHealth-=damageAmount;
        if(statsManager.instance.GetShipStat(Stat.StatTypeGeneral.Health)<=0) Die();
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void AddKnockback(Vector3 dir, float force) {
        externalForce += dir.normalized * force*1.3f;
        // Debug.LogWarning("enter do knockback dir:"+dir.ToString()+" force:"+force);
        // Debug.LogWarning("externalForce:"+ externalForce.ToString());
    }
    #endregion
    void toFly()
    {
        /* if (!lockDown)
        {
            GameManager.Instance.want2Fly=true;
        }
        else
        {
            GameManager.Instance.want2Fly=false;
        } */
    }

    //all method
      private void GetInputNau()
    {
        /* si afegim speed d'alguna mena 
        //is boosting movespeed=2, else movespeed=4
        // float moveSpeed=isBoosting ? 10f: 4f;
        */

        //Debug.Log("estàs amb la nau");

      /*   rb.linearVelocity = Vector3.zero;
        
        Vector2 movement =inputManager.playerInputActions.Nau.MoveNau.ReadValue<Vector2>().normalized;

       
        _newMovePosition+= new Vector3(movement.x, 0, movement.y) * moveSpeedNau *Time.deltaTime;
         */
    }
}