using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyUnit : MonoBehaviour
{
    #region variables
    [Header("Health")]
    [field: SerializeField] public float maxhealth { get; set; }
    [field: SerializeField] public float currentHealth { get; set; }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float stoppingDistance = 0.15f;
    [SerializeField] private float movementSmoothing = 0f;
    [SerializeField] private float rotationSmoothing = 0f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 720f; // grados por segundo

    [Header("References")]
    public Vector3 moveTarget;
    public Transform lookTarget;
    private Rigidbody rb;

    [Header("Control")]
    public bool hasMoveTarget;
    public bool hasLookTarget;

    

    //Private Variables
    private Vector3 currentVelocity;
    private GameObject obj;
    private float fixedX;
    private float fixedZ;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 initialEuler = transform.eulerAngles;
        fixedX = initialEuler.x;
        fixedZ = initialEuler.z;
        obj = this.gameObject;
    }
    
    private void Start()
    {
        currentHealth = maxhealth;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    #region Enemy Logic
    public void Damage(float damageAmount)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void primaryAttack()
    {
        //pew pew
        print("pew pew");
    }

    #endregion

    #region Public API
    // ========================
    // PUBLIC API (para la dupla)
    // ========================

    public void SetMoveTarget(Vector3 target)
    {
        moveTarget = target;
        hasMoveTarget = true;
    }

    public void StopMoving()
    {
        hasMoveTarget = false;
        currentVelocity = Vector2.zero;
    }

    public void SetLookTarget(Transform target)
    {
        lookTarget = target;
        hasLookTarget = true;
    }

    public void ClearLookTarget()
    {
        hasLookTarget = false;
    }

    #endregion

    #region Internal Logic
    // ========================
    // INTERNAL LOGIC
    // ========================
    private void HandleMovement()
    {
        if (!hasMoveTarget)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        Vector3 flatTarget = new Vector3(moveTarget.x, transform.position.y, moveTarget.z);
        Vector3 toTarget = flatTarget - transform.position;
        toTarget.y = 0f;

        float distance = toTarget.magnitude;

        if (distance <= stoppingDistance)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        // ---- NUEVO BLOQUE DE SUAVIZADO ----
        float dynamicSpeed = moveSpeed;

        if (movementSmoothing > 0f)
        {
            float slowRadius = movementSmoothing;
            float t = Mathf.Clamp01(distance / slowRadius);
            dynamicSpeed *= t;
        }

        Vector3 desiredVelocity = toTarget.normalized * dynamicSpeed;

        currentVelocity = Vector3.MoveTowards(
            new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z),
            desiredVelocity,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(
            currentVelocity.x,
            rb.linearVelocity.y,
            currentVelocity.z
        );
    }

    private void HandleRotation()
    {
        Vector3 direction;

        if (hasLookTarget && lookTarget != null)
        {
            direction = lookTarget.position - transform.position;
        }
        else if (hasMoveTarget)
        {
            Vector3 target = new Vector3(moveTarget.x, transform.position.y, moveTarget.z);
            direction = target - transform.position;
        }
        else
        {
            return;
        }

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        float targetYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float currentYaw = transform.eulerAngles.y;

        float newYaw;

        if (rotationSmoothing <= 0f)
        {
            newYaw = Mathf.MoveTowardsAngle(
                currentYaw,
                targetYaw,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
        else
        {
            float t = 1f - Mathf.Exp(-rotationSmoothing * Time.fixedDeltaTime);
            newYaw = Mathf.LerpAngle(currentYaw, targetYaw, t);
        }

        transform.rotation = Quaternion.Euler(fixedX, newYaw, fixedZ);
    }
    #endregion
}