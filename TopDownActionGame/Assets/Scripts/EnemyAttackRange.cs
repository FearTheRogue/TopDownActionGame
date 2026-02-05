using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyAttackRange : EnemyAttack
{
    [Header("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Projectile")]
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float projectileLifetime = 4f;

    protected override void PerformAttack()
    {
        if (firePoint == null || projectilePrefab == null || targetTransform == null) return;

        Vector2 dir = (targetTransform.position - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Rotate projectile visuals to face travel direction
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Drive the projectile
        EnemyProjectile p = proj.GetComponent<EnemyProjectile>();

        if (p != null)
        {
            p.Shoot(dir, projectileSpeed, projectileLifetime);
        }
        else
        {
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();

            if (rb != null) rb.linearVelocity = dir * projectileSpeed;

            Destroy(proj, projectileLifetime);
        }

    }
}
