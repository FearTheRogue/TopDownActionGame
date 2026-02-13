using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private string targetName = "Player";

    [Header("Attack")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float cooldown = 1.5f;

    [Header("Timing")]
    [SerializeField] private float windupTime = 0.25f;
    [SerializeField] private float recoveryTime = 0.15f;

    [Header("Hitbox")]
    [SerializeField] private Transform hitPoint;
    [SerializeField] private float hitRadius = 0.5f;
    [SerializeField] private LayerMask hitLayers;

    private Transform target;
    private float cooldownTimer;
    private bool attacking;

    private void Start()
    {
        GameObject targetGameObject = GameObject.FindGameObjectWithTag(targetName);
        
        if(targetGameObject != null)
            target = targetGameObject.transform;
    }

    private void Update()
    {
        if (target == null)
            return;

        cooldownTimer -= Time.deltaTime;

        if (attacking || cooldownTimer > 0f)
            return;

        float dist = Vector2.Distance(transform.position, target.position);

        if (dist > attackRange)
            return;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        attacking = true; ;
        cooldownTimer = cooldown;

        yield return new WaitForSeconds(windupTime);

        // hit
        Vector2 point = hitPoint != null ? (Vector2)hitPoint.position : (Vector2)transform.position;
        Collider2D hit = Physics2D.OverlapCircle(point, hitRadius, hitLayers);

        if (hit != null)
        {
            if (hit.TryGetComponent<PlayerHealth>(out var playerHealth))
            {
                playerHealth.TakeDamage(damage, transform.position); // knockback away from enemy
            }
            else
            {
                IDamageable dmg = hit.GetComponent<IDamageable>();
                dmg?.TakeDamage(damage);
            }
        }

        // Recovery
        yield return new WaitForSeconds(recoveryTime);

        attacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 point = hitPoint != null ? (Vector2)hitPoint.position : (Vector2)transform.position;
        Gizmos.DrawWireSphere(point, hitRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
