using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class HitReaction : MonoBehaviour
{
    [SerializeField] private float hitStunTime = 0.08f;

    private EnemyMovement movement;
    private Coroutine routine;
    private bool stunned;

    public bool IsStunned => stunned;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
    }

    public void HitStun()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(HitStunRoutine());
    }

    private IEnumerator HitStunRoutine()
    {
        stunned = true;
        movement.StopMove();
        yield return new WaitForSeconds(hitStunTime);
        stunned = false;
        routine = null;
    }
}
