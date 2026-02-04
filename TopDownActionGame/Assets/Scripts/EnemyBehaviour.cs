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

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        facing = GetComponent<EnemyFacing>();
        wander = GetComponent<EnemyWander>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector2 direction = (player.position - transform.position);
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > detectionRange)
        {
            movement.SetSpeed(wanderSpeed);
            wander.Wander();
            return;
        }

        if (distance < stopRange)
        {
            movement.Stop();

            if (direction.sqrMagnitude > 0.01f)
                facing.FaceDirection(direction);

            return;
        }

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
