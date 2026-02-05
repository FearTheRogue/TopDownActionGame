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
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
            enemyHealth.TakeDamage(damage);

        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void SetDamage(int amount)
    {
        damage = amount;
    }
}
