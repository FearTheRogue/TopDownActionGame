using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    private Vector2 inputVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputVector.x, inputVector.y) * moveSpeed;
    }

    private void Update()
    {
        inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();

        Vector2 mouseScreen = playerInputActions.Player.MousePosition.ReadValue<Vector2>();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        Vector2 direction = mouseWorld - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
