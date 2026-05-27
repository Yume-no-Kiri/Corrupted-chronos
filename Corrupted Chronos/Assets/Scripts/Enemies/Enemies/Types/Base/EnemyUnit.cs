using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyUnit : MonoBehaviour, IDamageable
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
    public Transform _lookTarget;
    private Rigidbody rb;

    [Header("Enemy Properties")]
    public NameBulletPreset projectile;
    public float fireRate = 1f;

    [Header("Control")]
    public bool hasMoveTarget;

    [Header("Particles")]
    public ParticleSystem hitParticles;
    [SerializeField] private float fadeInTime = 0.05f;
    [SerializeField] private float fadeOutTime = 0.15f;

    private Color originalColor;
    private Coroutine flashRoutine;

    //Private Variables
    private Vector3 currentVelocity;
    private GameObject obj;
    private float fixedX;
    private float fixedZ;
    private float lastFireTime;
    private SpriteRenderer spr;
    private Vector3 externalForce = Vector3.zero;

    protected bool CanFire()
    {
        return Time.time >= lastFireTime + (1f / fireRate);
    }

    protected void RegisterFire()
    {
        lastFireTime = Time.time;
    }

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 initialEuler = transform.eulerAngles;
        fixedX = initialEuler.x;
        fixedZ = initialEuler.z;
        obj = this.gameObject;

        currentHealth = maxhealth;
        if (!spr)
            spr = GetComponentInChildren<SpriteRenderer>();

        originalColor = spr.color;

        hitParticles = this.GetComponentInChildren<ParticleSystem>();
    }
    
    private void Start()
    {
        currentHealth = maxhealth;
    }

    private void Update()
    {
        if (externalForce.magnitude > 0.01f)
        {
            externalForce = Vector3.Lerp(externalForce, Vector3.zero, GameManager.Instance.knockbackResistance * Time.deltaTime);
        }
        else
        {
            externalForce = Vector3.zero; // Forcem el zero per estalviar c�lculs quan �s molt petita
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    #region Enemy Logic
    public void TakeDamage(float damageAmount)
    {
        if (currentHealth <= 0) Die();
        AddKnockback(-transform.forward, statsManager.instance.GetShipStat(Stat.StatTypeGeneral.Knockback)); // Ejemplo de knockback, ajusta la direcci�n y fuerza seg�n tus necesidades
        hitParticles.Stop();
        hitParticles.Play();
        spriteFlash();
        currentHealth -=damageAmount;
        
    }

    public void Die()
    {
        GameManager.Instance.EnemyKilled(transform.position);
        Destroy(this.gameObject);
    }
    public void AddKnockback(Vector3 dir, float force)
    {
        externalForce += dir.normalized * force * 1.3f;
    }

    public void spriteFlash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // fade to white (fast)
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            spr.color = Color.Lerp(originalColor, Color.white, t / fadeInTime);
            yield return null;
        }

        spr.color = Color.white;

        // fade back to original (slower)
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            spr.color = Color.Lerp(Color.white, originalColor, t / fadeOutTime);
            yield return null;
        }

        spr.color = originalColor;
        flashRoutine = null;
    }

    public abstract void primaryAttack();

    public abstract void secondarySkill();

    #endregion

    #region Internal Logic
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

        if (_lookTarget != null)
        {
            direction = _lookTarget.position - transform.position;
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