using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ShipMovement : MonoBehaviour
{
    [Header("Parameters")]
    public bool canMove;
    public bool canBoost;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference boostAction;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float boostMultiplier = 2f;

    [Header("Smoothing")]
    [SerializeField] private float directionSmoothing = 10f;
    [SerializeField] private float boostSmoothing = 5f;

    private CharacterController controller;

    private Vector3 currentMoveVector;
    private Vector3 targetMoveVector;

    private float currentSpeedMultiplier = 1f;
    private float targetSpeedMultiplier = 1f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        boostAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        boostAction.action.Disable();
    }

    private void Update()
    {
        if (!canMove)
            return;

        ReadInput();
        SmoothDirection();
        if (canBoost)
            SmoothBoost();
        ApplyMovement();
    }

    private void ReadInput()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        targetMoveVector = new Vector3(input.x, 0f, input.y);

        if (targetMoveVector.sqrMagnitude > 1f)
            targetMoveVector.Normalize();

        bool isBoosting = boostAction.action.IsPressed();
        targetSpeedMultiplier = isBoosting ? boostMultiplier : 1f;
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
        Vector3 finalVelocity = currentMoveVector * moveSpeed * currentSpeedMultiplier;
        controller.Move(finalVelocity * Time.deltaTime);
    }
}