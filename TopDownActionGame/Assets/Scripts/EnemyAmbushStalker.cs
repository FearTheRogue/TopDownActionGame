using NUnit.Framework.Constraints;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyAmbushStalker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform visuals;
    [SerializeField] private EnemyBehaviour behaviour;
    [SerializeField] private EnemyFacing facing;

    [Header("Target")]
    [SerializeField] private string targetTag = "Player";
    private Transform target;

    [Header("Ambush Trigger")]
    [SerializeField] private float triggerRange = 5f;

    [Header("Reset")]
    [SerializeField] private float disengageRange = 10f;
    [SerializeField] private float resetDelay = 0.5f;
    [SerializeField] private bool pickNewAmbushSpot = false;
    [SerializeField] private float ambushRadius;
    [SerializeField] private float returnSpeed = 4f;
    [SerializeField] private float arriveDistance = 0.2f;

    [Header("Telegraph")]
    [SerializeField] private float revealDelay = .15f;
    [SerializeField] private float telegraphTime = 0.35f;

    [Header("Lunge")]
    [SerializeField] private float lungeSpeed = 10f;
    [SerializeField] private float lungeDuration = 0.22f;

    [Header("After Lunge")]
    [SerializeField] private float postLungePause = 0.1f;

    private EnemyMovement movement;

    private bool triggered;
    private bool ambushing;
    private Coroutine routine;

    private Vector2 startPos;
    private Vector2 ambushSpot;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();

        if (behaviour == null)
            behaviour = GetComponent<EnemyBehaviour>();

        if (facing == null)
            facing = GetComponent<EnemyFacing>();
    }

    private void Start()
    {
        startPos = transform.position;
        ambushSpot = startPos;

        FindTarget();

        // Start hidden/ dormant
        SetHidden(true);
        DisableNormalAI();
    }

    private void Update()
    {
        // If player object changed after respawn, re-find it
        if (target == null)
        {
            FindTarget();
            return;
        }

        if (!triggered && !ambushing)
        {
            float dist = Vector2.Distance(transform.position, target.position);

            if (dist <= triggerRange)
                StartAmbush();
        } 
        else if (triggered && !ambushing)
        {
            // After ambush, normal AI is enabled. If the player escapes far enough, reset.
            float dist = Vector2.Distance(transform.position, target.position);

            if (dist >= disengageRange)
                ResetToAmbush();
        }
    }

    private void FindTarget()
    {
        var go = GameObject.FindGameObjectWithTag(targetTag);

        if (go != null)
            target = go.transform;
    }

    private void StartAmbush()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(AmbushRoutine());
    }

    private IEnumerator AmbushRoutine()
    {
        triggered = true;
        ambushing = true;

        // reveal
        SetHidden(false);

        yield return new WaitForSeconds(revealDelay);

        // Telegraph: face the player, but dont move
        Vector2 dir = ((Vector2)target.position - (Vector2)transform.position);

        if (dir.magnitude > 0.001f && facing != null)
            facing.FaceDirection(dir);

        movement.Stop();

        yield return new WaitForSeconds(telegraphTime);

        // Lunge
        dir = ((Vector2)target.position - (Vector2)transform.position).normalized;
        movement.SetSpeed(lungeSpeed);
        movement.SetMoveDirection(dir);

        yield return new WaitForSeconds(lungeDuration);

        // stop briefly
        yield return new WaitForSeconds(postLungePause);

        // Hand off to normal AI
        EnableNormalAI();

        ambushing = false;
        routine = null;
    }

    private void ResetToAmbush()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        ambushing = true;

        DisableNormalAI();
        movement.Stop();

        yield return new WaitForSeconds(resetDelay);

        // Decide ambush point
        ambushSpot = pickNewAmbushSpot ? PickNewAmbushSpot() : startPos;

        // Return to ambush spot
        while (((Vector2)transform.position - ambushSpot).magnitude > arriveDistance)
        {
            Vector2 dir = (ambushSpot - (Vector2)transform.position);

            if (dir.sqrMagnitude > 0.001f && facing != null)
                facing.FaceDirection(dir);

            movement.SetSpeed(returnSpeed);
            movement.SetMoveDirection(dir);

            yield return null;
        }

        movement.Stop();

        // Hide again and wait
        SetHidden(true);
        triggered = false;
        ambushing = false;
        routine = null;
    }

    private Vector2 PickNewAmbushSpot()
    {
        if (ambushRadius <= 0f)
            return startPos;

        return startPos + Random.insideUnitCircle * ambushRadius;
    }

    private void DisableNormalAI()
    {
        if (behaviour != null)
            behaviour.enabled = false;
    }

    private void EnableNormalAI()
    {
        if (behaviour != null)
            behaviour.enabled = true;
    }

    private void SetHidden(bool isHidden)
    {
        if (visuals != null)
            visuals.gameObject.SetActive(!isHidden);
    }

    public void ForceHideNow()
    {
        if (routine != null)
            StopCoroutine(routine);

        DisableNormalAI();
        movement.Stop();
        transform.position = ambushSpot;
        SetHidden(true);
        triggered = false;
        ambushing = false;
        routine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0.1f);
        Gizmos.DrawWireSphere(transform.position, triggerRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, disengageRange);
    }
}
