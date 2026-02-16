using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyFacing))]
public class EnemyBrain : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float stopRange = 1.2f;

    [Header("Speeds")]
    [SerializeField] private float wanderSpeed = 1.5f;
    [SerializeField] private float chaseSpeed = 3.0f;

    [Header("Optional Modules")]
    [SerializeField] private EnemyPatrol patrol;
    [SerializeField] private EnemyWander wander;
    [SerializeField] private EnemyAmbushStalker ambush;
    [SerializeField] private EnemyMeleeAttack melee;

    private Transform player;
    private EnemyMovement movement;
    private EnemyFacing facing;
    private CombatState combatState;

    private float combatPingTimer;
    [SerializeField] private float combatPingInterval = 0.25f;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        facing = GetComponent<EnemyFacing>();

        // Auto-fill modules if not assigned
        if (patrol == null)
            patrol = GetComponent<EnemyPatrol>();
        if (wander == null)
            wander = GetComponent<EnemyWander>();
        if (ambush == null)
            ambush = GetComponent<EnemyAmbushStalker>();
        if (melee == null)
            melee = GetComponent<EnemyMeleeAttack>();
    }

    private void Start()
    {
        combatState = FindFirstObjectByType<CombatState>();

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    private void Update()
    {
        // If this enemy has dedicated ambush module, let it control itself
        if (ambush != null && ambush.enabled)
            return;

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
            return;
        }

        Vector2 toPlayer = (Vector2)(player.position - transform.position);
        float dist = toPlayer.magnitude;

        // Idle/Patrol/Wander when far
        if (dist > detectionRange)
        {
            movement.SetSpeed(wanderSpeed);

            if (patrol != null)
                patrol.Patrol();
            else if (wander != null)
                wander.Wander();
            else
                movement.Stop();

            return;
        }

        // Engage zone (ping combat, chase, stop+face
        combatPingTimer -= Time.deltaTime;

        if (combatPingTimer <= 0)
        {
            combatState?.NotifyCombat("EnemyBrain");
            combatPingTimer = combatPingInterval;
        }

        if (dist <= stopRange)
        {
            movement.Stop();

            if (toPlayer.sqrMagnitude > 0.01f)
                facing.FaceDirection(toPlayer);

            // Attacks are separate modules: they decide if/when to hit
            return;
        }

        movement.SetSpeed(chaseSpeed);
        movement.SetMoveDirection(toPlayer);
        facing.FaceDirection(toPlayer);
    }
}
