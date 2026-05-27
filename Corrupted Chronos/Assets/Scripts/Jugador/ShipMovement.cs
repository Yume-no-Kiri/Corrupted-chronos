using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ShipMovement : MonoBehaviour,IDamageable 
{
    public float health;
    public float shields;

    [Header("Parameters")]
    public bool canMove=true;
    public bool canBoost;

    private bool IsGround=false;

    private Vector3 PositionGround;
    private Vector3 LimitGound;

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
    private void Start()
    {
        Vector3 iniPos=transform.position;
        transform.position=new Vector3(iniPos.x, posLow.y,iniPos.z);
        health = statsManager.instance.GetShipStat(Stat.StatTypeGeneral.MaxHealth);
        shields = statsManager.instance.GetShipStat(Stat.StatTypeGeneral.MaxShields);
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
            externalForce = Vector3.zero;
        }

        ApplyMovement();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 4))
        {
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.red);
            if(hit.transform.CompareTag("Ground")){
                Debug.Log("distance ground"+Vector3.Distance(transform.position, hit.point));
                if (Vector3.Distance(transform.position, hit.point)<=3f)
                {
                    IsGround=true;
                    // PositionGround= hit.transform.position;
                    posLow=hit.point+ new Vector3(0,1,0);
                    posInY=posLow.y;
                    /* if(posLow.y>= transform.position.y)
                    {
                        
                    } */
                    

                }
               
                // LimitGound= PositionGround+new Vector3(0,2,0);
            }
            // GameManager.Instance.want2Fly=false;
        }else
        {
            IsGround=false;
            // GameManager.Instance.want2Fly=true;
        }
        // Ray ray = GameManager.Instance.inputManager.cameraGameplay.ScreenPointToRay



        // Debug.Log("ss does update work? 2");

    }

    Vector3 posLow= new Vector3(0,7,0);
    Vector3 posHigh=new Vector3(0,11,0);
    float posInY= 0;
    // float timeBetweenHigh=1.2f;

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
            if(!GameManager.Instance.CanFly()){
                if (transform.position.y >= posLow.y)
                {
                    inputDownUp=-1;
                }
                
            }
            if (transform.position.y >= posHigh.y && inputDownUp==1)
            {
                inputDownUp=0;
            }
            if (inputDownUp > 0){
                GameManager.Instance.MoveUpStamina(IsGround);
            }
            else
            {
                GameManager.Instance.InformIfGround(IsGround);
            }

            /* if (transform.position.y > LimitHigh.y && inputDownUp>0 )
            {
                inputDownUp=0;
            } */
        }
        if (inputDownUp < 0)
        {
            posInY=posLow.y;
        }else if (inputDownUp > 0)
        {
            posInY=posHigh.y;            
        }
        
        targetMoveVector = new Vector3(inputMove.x, 0, inputMove.y);

        if (targetMoveVector.sqrMagnitude > 1f)
        { targetMoveVector.Normalize();}
        // bool isBoosting = boostAction.IsPressed();


        // targetSpeedMultiplier = isBoosting ? boostMultiplier : 1f;
    }
    private void ApplyMovement()
    {
        Vector3 finalVelocity = currentMoveVector * statsManager.instance.GetShipStat(Stat.StatTypeGeneral.Speed) * currentSpeedMultiplier+externalForce;

        if(finalVelocity!=Vector3.zero){  GameManager.Instance.WaterMaterial.SetVector("_DirectionPlayer",finalVelocity.normalized );}

        controller.Move(finalVelocity * Time.deltaTime);

        Vector3 currentPos = transform.position;
        if(posInY!=0){
            currentPos.y = Mathf.MoveTowards(currentPos.y, posInY, 20f * Time.deltaTime);

        // Apliquem la posició vertical corregida directament al transform
            transform.position = currentPos;
        }
        /* Vector3 posicioCorregida = transform.position;
        posicioCorregida.y = Mathf.Clamp(posicioCorregida.y, alturaMinima, alturaMaxima);
        if (transform.position.y != posicioCorregida.y)
        {
            // Detenim el moviment vertical residual per evitar tremolors
            currentMoveVector.y = 0; 
            transform.position = posicioCorregida;
        } */

        // controller.Move(new Vector3(5, 0, 0) * Time.deltaTime);
        // Debug.Log("ss final velocity:"+ finalVelocity.ToString());
    }
    private void SmoothDirection()
    {
        // float finalPos=0;

       

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

   
    #region interface
    public void TakeDamage(float damageAmount)
    {
        if (statsManager.instance.GetShipStat(Stat.StatTypeGeneral.CurrentShields) > 0f)
        {
            float absorbed = Mathf.Min(statsManager.instance.GetShipStat(Stat.StatTypeGeneral.CurrentShields), damageAmount);

            statsManager.instance.decreaseStatValue(Stat.StatTypeGeneral.CurrentShields, absorbed);
            damageAmount -= absorbed;
        }

        if (damageAmount > 0f)
        {
            statsManager.instance.decreaseStatValue(Stat.StatTypeGeneral.CurrentHealth, damageAmount);
        }

        if (statsManager.instance.GetShipStat(Stat.StatTypeGeneral.CurrentHealth) <= 0f)
        {
            Die();
        }
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
   
}