using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;

    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    private PlayerShooting playerShooting;

    public Transform firePoint;
    public GameObject bullet;
    public float timeBetweenShots;
    public float shotCounter;
    private bool isShooting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerShooting = GetComponent<PlayerShooting>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        //playerInputActions.Player.Shoot.performed += Shoot_performed;
        //playerInputActions.Player.Shoot.canceled += Shoot_canceled;

        //playerInputActions.Player.Shoot.performed += playerShooting.OnShootPerformed;
        //playerInputActions.Player.Shoot.canceled += playerShooting.OnShootCanceled;
    }

    //private void Shoot_performed(InputAction.CallbackContext obj)
    //{
    //    isShooting = true;
    //}

    //private void Shoot_canceled(InputAction.CallbackContext obj)
    //{
    //    isShooting = false;
    //}

    private void FixedUpdate()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(inputVector.x, inputVector.y) * moveSpeed;
    }

    private void Update()
    {
        Vector2 mouseScreen = playerInputActions.Player.MousePosition.ReadValue<Vector2>();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        Vector2 direction = mouseWorld - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        //if (!isShooting) return;

        //shotCounter -= Time.deltaTime;

        //if (shotCounter <= 0)
        //{
        //    FireBullet();
        //    shotCounter = timeBetweenShots;
        //}
    }

    private void FireBullet()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}
