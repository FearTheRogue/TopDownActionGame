using UnityEngine;
using UnityEngine.Rendering;

public class EnemyBehaviour : MonoBehaviour
{
    private Transform player;
    private EnemyMovement movement;
    private EnemyFacing facing;
    private EnemyWander wander;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float stopRange;

    [Header("Speed Settings")]
    [SerializeField] private float wanderSpeed;
    [SerializeField] private float chaseSpeed;

    private EnemyPatrol patrol;
    private CombatState combatState;

    [SerializeField] private float combatPingInterval = 0.25f;
    private float combatPingTimer;

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
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null)
            return;

        Vector2 direction = (player.position - transform.position);
        float distance = Vector2.Distance(transform.position, player.position);

        combatPingTimer -= Time.deltaTime;

        bool isEngaging = distance <= detectionRange && distance > stopRange;

        if (isEngaging && combatPingTimer <= 0f)
        {
            combatState?.NotifyCombat("EnemyBahaviour");
            combatPingTimer = combatPingInterval;
        }

        if (distance > detectionRange)
        {
            if (patrol != null)
            {
                patrol.Patrol();
            }
            else if (wander != null)
            {
                movement.SetSpeed(wanderSpeed);
                wander.Wander();
            }
            else
            {
                movement.Stop();
            }

            return;
        }

        if (distance < stopRange)
        {
            movement.Stop();

            if (direction.sqrMagnitude > 0.01f)
                facing.FaceDirection(direction);

            return;
        }

        movement.SetSpeed(chaseSpeed);
        movement.SetMoveDirection(direction);
        facing.FaceDirection(direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopRange);
    }
}
