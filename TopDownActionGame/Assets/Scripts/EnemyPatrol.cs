using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyFacing))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform[] patrolPoints;

    [Header("Settings")]
    [SerializeField] private float arriveDistance = 0.2f;
    [SerializeField] private float waitTime = 1.0f;
    [SerializeField] private float patrolSpeed = 1.5f;

    private EnemyMovement movement;
    private EnemyFacing facing;

    private int currentIndex = 0;
    private int direction = 1;
    private float waitTimer = 0;
    private bool waiting = false;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        facing = GetComponent<EnemyFacing>();
    }

    public void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        // Waiting at point
        if (waiting)
        {
            waitTimer -= Time.deltaTime;
            movement.Stop();

            if (waitTimer <= 0)
            {
                waiting = false;

                // Move to the next index in current direction
                currentIndex += direction;

                // Reverse direction
                if (currentIndex >= patrolPoints.Length)
                {
                    currentIndex = patrolPoints.Length - 2;
                    direction = -1;
                }
                else if (currentIndex < 0)
                {
                    currentIndex = 1;
                    direction = 1;
                }
            }

            return;
        }

        Transform target = patrolPoints[currentIndex];

        Vector2 dir = target.position - transform.position;

        movement.SetSpeed(patrolSpeed);
        movement.SetMoveDirection(dir);
        facing.FaceDirection(dir);

        // Arrived at point
        if (dir.magnitude <= arriveDistance)
        {
            waiting = true;
            waitTimer = waitTime;
            movement.Stop();
        }
    }
}