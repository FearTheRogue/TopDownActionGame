using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyFacing))]
public class EnemyBehaviour : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float stopRange;

    [Header("Speeds")]
    [SerializeField] private float wanderSpeed;
    [SerializeField] private float chaseSpeed;

    [Header("Combat Ping")]
    [SerializeField] private float combatPingInterval = 0.25f;

    // Cached references
    private Transform player;
    private EnemyMovement movement;
    private EnemyFacing facing;

    // Optional behaviour modules
    private EnemyWander wander;
    private EnemyPatrol patrol;

    private CombatState combatState;
    private float combatPingTimer;

    private bool HasPlayer => player != null;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        facing = GetComponent<EnemyFacing>();

        wander = GetComponent<EnemyWander>();
        patrol = GetComponent<EnemyPatrol>();
    }

    private void Start()
    {
        combatState = FindFirstObjectByType<CombatState>();
        TryFindPlayer();
    }

    private void Update()
    {
        if (!HasPlayer)
        {
            TryFindPlayer();
            return;
        }

        Vector2 toPlayer = (Vector2)(player.position - transform.position);
        float distance = toPlayer.magnitude;

        // 1) Out of range -> idle behaviour (patrol/wander/stop)
        if (distance > detectionRange)
        {
            DoIdleBehaviour();
            return;
        }

        // 2) In detection range -> optionally ping combat
        PingCombatIfEngaging(distance);

        // 3) Close enough -> stop moving but keep facing
        if (distance < stopRange)
        {
            movement.Stop();

            if (toPlayer.sqrMagnitude > 0.01f)
                facing.FaceDirection(toPlayer);

            return;
        }

        // 4) Otherwise chase
        movement.SetSpeed(chaseSpeed);
        movement.SetMoveDirection(toPlayer);
        facing.FaceDirection(toPlayer);
    }

    // ----------------------------
    // Behaviour blocks
    // ----------------------------

    private void DoIdleBehaviour()
    {
        if (patrol != null)
        {
            patrol.Patrol();
            return;
        }

        if (wander != null)
        {
            movement.SetSpeed(wanderSpeed);
            wander.Wander();
            return;
        }

        // No idle module attached -> do nothing
        movement.Stop();
    }

    private void PingCombatIfEngaging(float distance)
    {
        // Only "engaging" while chasing (not when stopped at stopRange)
        bool isEngaging = distance <= detectionRange && distance > stopRange;

        if (!isEngaging)
            return;

        combatPingTimer -= Time.deltaTime;

        if (combatPingTimer <= 0)
        {
            combatState?.NotifyCombat("EnemyBehaviour");
            combatPingTimer = combatPingInterval;
        }
    }

    private void TryFindPlayer()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        player = p != null ? p.transform : null;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopRange);
    }
#endif
}
