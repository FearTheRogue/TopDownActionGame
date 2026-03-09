using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Tooltip("How quickly external velocity (knockback/dash) decays back to zero.")]
    [SerializeField] private float externalVelocityDecay = 12f;

    [Header("Aim")]
    [SerializeField] private Transform visuals;
    [SerializeField] private Transform armPivot;

    [Tooltip("Higher = faster aim smoothing. Set to O for instant aim.")]
    [SerializeField] private float aimSmoothing;

    // --- Cached references ---
    private Rigidbody2D rb;
    private Camera cam;
    private SpriteFlipper flipper;
    private PlayerInputActions playerInputActions;

    // --- Input state ---
    private Vector2 moveInput;

    // --- Aim state ---
    private Vector2 aimDirection = Vector2.right;
    private float targetAngle;
    private float currentAngle;

    // --- External forces (dash/knockback) ---
    // These are added on top of player movement and decay over time.
    private Vector2 externalVelocity;

    private bool Blocked => PauseManager.Paused || GameOverManager.GameOverActive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        flipper = GetComponent<SpriteFlipper>();

        playerInputActions = new PlayerInputActions();

        if (visuals == null)
            visuals = transform;
        if (armPivot == null)
            armPivot = transform;
    }

    private void OnEnable()
    {
        playerInputActions?.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions?.Player.Disable();
    }
    private void Update()
    {
        if (Blocked)
            return;

        // Read raw movement input here; physics uses it in FixedUpdate.
        moveInput = playerInputActions.Player.Movement.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (Blocked)
        {
            // Hard-stop while paused/game-over
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 move = moveInput;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        // External velocity it used for knockback + dash
        // We decay it separately so it feels responsive but not slippery
        externalVelocity = Vector2.Lerp(externalVelocity, Vector2.zero, Time.fixedDeltaTime * externalVelocityDecay);

        rb.linearVelocity = (move * moveSpeed) + externalVelocity; 
    }

    private void LateUpdate()
    {
        if (Blocked)
            return;

        if (armPivot == null || cam == null)
            return;

        // MOuse input is screen space; convert to world aim in 2D space
        Vector2 mouseScreen = playerInputActions.Player.MousePosition.ReadValue<Vector2>();
        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        Vector2 direction = (Vector2)mouseWorld - (Vector2)armPivot.position;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        aimDirection = direction.normalized;

        targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentAngle = aimSmoothing > 0f ? Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * aimSmoothing) : targetAngle;

        // Flip visuals for left/right facing (keeps sprite readable in 2.5D view)
        flipper?.FaceDirection(direction);

        // Rotate the arm pivot only
        armPivot.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    // --- External velocity API ---
    // These impulses work because they are applied through the same movement pipeline,
    // rather than fighting the rigidbody velocity assignment.
    public void AddKnockback(Vector2 impulse) => externalVelocity += impulse;

    public void AddDash(Vector2 impulse) => externalVelocity += impulse;

    public void ClearExternalVelocity() => externalVelocity = Vector2.zero;

    // --- Data access for abilities ---
    public Vector2 GetMoveInput() => moveInput;

    public Vector2 GetAimDirection()
    {
        // Always return something sensible even if aim hasn't updated yet
        return aimDirection.sqrMagnitude > 0.001f ? aimDirection : transform.right;
    }

}
