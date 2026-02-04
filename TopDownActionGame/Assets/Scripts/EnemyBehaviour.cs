using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Transform player;
    private EnemyMovement movement;
    private EnemyFacing facing;

    [SerializeField] private float detectionRange;
    [SerializeField] private float stopRange;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        facing = GetComponent<EnemyFacing>();
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
            movement.Stop();
            return;
        }

        if (distance < stopRange)
        {
            movement.Stop();
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
