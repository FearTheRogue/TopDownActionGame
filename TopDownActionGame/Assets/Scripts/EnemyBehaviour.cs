using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Transform player;
    private EnemyMovement movement;
    private EnemyFacing facing;

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
        movement.SetMoveDirection(direction);

        facing.FaceDirection(direction);
    }
}
