using System.Collections;
using System.Net.Sockets;
using UnityEngine;
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
        GameObject targetGameObject = GameObject.FindGameObjectWithTag(targetTag);

        if (targetGameObject != null)
            target = targetGameObject.transform;

        // Start hidden/dormant
        if (visuals != null)
            visuals.gameObject.SetActive(false);

        if (behaviour != null)
        {
            behaviour.enabled = false;
            movement.Stop();
        }
    }

    private void Update()
    {
        if (triggered || target == null) return;

        float dist = Vector2.Distance(transform.position, target.position);

        if (dist <= triggerRange)
            StartCoroutine(AmbushRoutine());
    }

    private IEnumerator AmbushRoutine()
    {
        triggered = true;

        // reveal
        if (visuals != null)
            visuals.gameObject.SetActive(true);

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
        if (behaviour != null) 
            behaviour.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0.1f);
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
}
