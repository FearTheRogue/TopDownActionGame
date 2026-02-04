using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;

    public GameObject impactEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.linearVelocity = transform.right * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(impactEffect, transform.position, Quaternion.identity);
    }
}
