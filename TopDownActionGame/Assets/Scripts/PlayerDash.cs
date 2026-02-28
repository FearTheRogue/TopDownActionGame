using System.Collections;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerDash : MonoBehaviour, IAbilityCooldown
{
    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 12f;
    [SerializeField] private float dashCooldown = 2f;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerableTime = 0.2f;

    private PlayerController controller;
    private Rigidbody2D rb;
    private PlayerInputActions playerInput;

    private float cooldownTimer;

    private bool canDash = true;

    private float invulnTimer;
    public bool IsInvulnerable => invulnTimer > 0f;
    public bool IsReady => cooldownTimer <= 0f;
    public float CooldownRemaining => Mathf.Max(0f, cooldownTimer);
    public float CooldownDuration => dashCooldown;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        playerInput = new PlayerInputActions();
        playerInput.Player.Enable();
        playerInput.Player.Dash.performed += OnDash;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (!canDash || !IsReady)
            return;

        Vector2 dir = controller.GetMoveInput();

        if (dir.sqrMagnitude < 0.01f)
        {
            // dash in aim direction
            dir = controller.GetAimDirection();
        }

        dir.Normalize();

        controller.AddDash(dir * dashForce);

        invulnTimer = invulnerableTime;

        //StartCoroutine(CooldownRoutine());
        cooldownTimer = dashCooldown;
    }

    private void Update()
    {
        if (invulnTimer > 0f)
            invulnTimer -= Time.deltaTime;

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    private IEnumerator CooldownRoutine()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnDestroy()
    {
        playerInput.Player.Dash.performed -= OnDash;
        playerInput.Disable();
    }
}
