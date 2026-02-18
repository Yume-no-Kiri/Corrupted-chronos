using UnityEngine;
using UnityEngine.InputSystem;

public class ShipLook : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotationSmoothing = 10f;

    [Header("Parameters")]
    public bool canRotate = true;

    [Header("Rotation Offset")]
    private float xOffset = 90f;
    private float zOffset = 0f;

    private Camera mainCamera;
    private float currentAngle;
    private float targetAngle;

    private void Awake()
    {
        mainCamera = Camera.main;

        // Inicializamos el ángulo actual con la rotación Y existente
        currentAngle = transform.eulerAngles.y;
        xOffset = transform.eulerAngles.x;
        zOffset = transform.eulerAngles.z;

    }

    private void Update()
    {
        if (canRotate)
        {
            UpdateTargetAngle();
            SmoothRotate();
        }     
    }

    private void UpdateTargetAngle()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 shipScreenPos = mainCamera.WorldToScreenPoint(transform.position);

        Vector2 direction = mouseScreenPos - shipScreenPos;

        if (direction.sqrMagnitude > 0.001f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        }
    }

    private void SmoothRotate()
    {
        currentAngle = Mathf.LerpAngle(
            currentAngle,
            targetAngle,
            rotationSmoothing * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(
            xOffset,
            currentAngle,
            zOffset
        );
    }
}
