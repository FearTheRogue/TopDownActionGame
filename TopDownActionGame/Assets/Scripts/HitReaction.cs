using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class HitReaction : MonoBehaviour
{
    [SerializeField] private float defaultHitStunTime = 0.08f;

    private EnemyMovement movement;
    private Coroutine routine;
    private bool stunned;

    // Track stun intil a time so repeated hit extend properly
    private float stunUntil;

    public bool IsStunned => stunned;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
    }

    public void HitStun()
    {
        HitStun(defaultHitStunTime);
    }

    // Custom stun
    public void HitStun(float duration)
    {
        if (duration <= 0f)
            return;

        stunUntil = Mathf.Max(stunUntil, Time.time + duration);

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(HitStunRoutine());
    }

    private IEnumerator HitStunRoutine()
    {
        stunned = true;
        movement.StopMove();

        while (Time.time < stunUntil)
            yield return null;
        
        stunned = false;
        routine = null;
    }
}
