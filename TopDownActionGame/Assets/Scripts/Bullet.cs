using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeToLive;
    [SerializeField] private int damage;

    private Rigidbody2D rb;

    [Header("Effects")]
    public GameObject impactEffect;

    [Header("Knockback Force")]
    [SerializeField] private float knockbackForce = 2.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.right * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            enemyHealth.TakeDamage(damage);

            // Knockback enemy away from the bullet
            if (other.TryGetComponent<EnemyMovement>(out var enemyMove))
            {
                Vector2 dir = (Vector2)(other.transform.position - transform.position);

                if (dir.sqrMagnitude > 0.0001f)
                {
                    dir.Normalize();
                    enemyMove.AddKnockback(dir * knockbackForce);
                }
            }
        }

        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void SetDamage(int amount)
    {
        damage = amount;
    }
}
