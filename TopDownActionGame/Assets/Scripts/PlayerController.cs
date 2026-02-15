using System;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform visuals;
    [SerializeField] private Transform armPivot;

    [Header("Aim Smoothing")]
    [SerializeField] private float aimSmoothing;

    private Rigidbody2D rb;
    //private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    private Camera cam;

    private Vector2 inputVector;
    private float targetAngle;
    private float currentAngle;

    private SpriteFlipper flipper;

    [SerializeField] private float knockbackDecay = 12f;
    private Vector2 externalVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        flipper = GetComponent<SpriteFlipper>();
        //playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();
    }

    private void FixedUpdate()
    {
        //rb.linearVelocity = new Vector2(inputVector.x, inputVector.y) * moveSpeed;
        ////transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //rb.MoveRotation(targetAngle);

        Vector2 move = inputVector;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        // decay external velocity smoothly
        externalVelocity = Vector2.Lerp(externalVelocity, Vector2.zero, Time.fixedDeltaTime * knockbackDecay);

        rb.linearVelocity = (move * moveSpeed) + externalVelocity;
    }

    public void AddKnockback(Vector2 impulse)
    {
        externalVelocity += impulse;
    }

    private void Update()
    {
        inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();

        //if (visuals != null)
        //    visuals.rotation = Quaternion.Euler(0f, 0f, targetAngle);
    }

    private void LateUpdate()
    {
        if (visuals == null || cam == null) return;

        Vector2 mouseScreen = playerInputActions.Player.MousePosition.ReadValue<Vector2>();
        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        Vector2 direction = (Vector2)mouseWorld - (Vector2)armPivot.position;

        if (direction.sqrMagnitude < 0.0001f) return;

        targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (aimSmoothing > 0f)
            currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * aimSmoothing);
        else
            currentAngle = targetAngle;

        //visuals.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
        if(flipper != null)
            flipper.FaceDirection(direction);

        armPivot.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    public void ClearExternalVelocity()
    {
        externalVelocity = Vector2.zero;
    }

    private void OnEnable()
    {
        playerInputActions?.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions?.Player.Disable();
    }
}
