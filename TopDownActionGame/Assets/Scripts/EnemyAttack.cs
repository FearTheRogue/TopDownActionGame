using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] protected string targetTag = "Player";
    protected Transform targetTransform;

    [Header("Attack Timing")]
    [SerializeField] protected float attackCooldown = 1f;
    protected float cooldownTimer;

    [Header("Attack Range")]
    [SerializeField] protected float attackRange = 5f;

    protected virtual void Awake()
    {
        cooldownTimer = Random.Range(0f, attackCooldown);
    }

    protected virtual void Start()
    {
        GameObject target = GameObject.FindGameObjectWithTag(targetTag);
        if (target != null) targetTransform = target.transform;
    }

    protected virtual void Update()
    {
        if (targetTransform == null) return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer > 0f) return;
        if (!IsTargetInRange()) return;

        PerformAttack();
        cooldownTimer = attackCooldown;
    }

    protected bool IsTargetInRange()
    {
        float dist = Vector2.Distance(transform.position, targetTransform.position);
        return dist <= attackRange;
    }

    protected abstract void PerformAttack();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
