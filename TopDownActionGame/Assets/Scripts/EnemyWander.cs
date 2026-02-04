using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyFacing))]
public class EnemyWander : MonoBehaviour
{
    [Header("Wander Settings")]
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderDelay;

    private EnemyMovement movement;
    private EnemyFacing facing;

    private Vector2 startPos;
    private Vector2 wanderTarget;
    private float timer;

    private bool waiting = false;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        facing = GetComponent<EnemyFacing>();
    }

    private void Start()
    {
        startPos = transform.position;
        PickNewTarget();
    }

    public void Wander()
    {
        if (waiting) return;

        // Move toward wander target
        Vector2 direction = wanderTarget - (Vector2)transform.position;
        movement.SetMoveDirection(direction);
        facing.FaceDirection(direction);

        // If close enough to target, pick a new target
        if (direction.magnitude < 0.2f)
        {
            StartCoroutine(WanderWait());
        }
    }

    private IEnumerator WanderWait()
    {
        waiting = true;
        movement.Stop();

        float waitTime = UnityEngine.Random.Range(1f, 3f);
        yield return new WaitForSeconds(waitTime);

        PickNewTarget();
        waiting = false;
    }

    private void PickNewTarget()
    {
        // Pick a random point within a circle radius from the start position
        wanderTarget = startPos + UnityEngine.Random.insideUnitCircle * wanderRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawCube(wanderTarget, new Vector2(0.5f, 0.5f));
    }
}
