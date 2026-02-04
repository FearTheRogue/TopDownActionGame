using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Transform player;
    private EnemyMovement movement;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector2 direction = (player.position - transform.position);
        movement.SetMoveDirection(direction);
    }
}
