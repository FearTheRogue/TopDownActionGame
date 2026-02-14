using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    [SerializeField] private float knockbackDecay = 14f;
    private Vector2 externalVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void FixedUpdate()
    {
        externalVelocity = Vector2.Lerp(externalVelocity, Vector2.zero, Time.fixedDeltaTime * knockbackDecay);
        rb.linearVelocity = (moveDirection * moveSpeed) + externalVelocity;
    }

    public void AddKnockback(Vector2 impulse)
    {
        externalVelocity += impulse;
    }

    public void Stop()
    {
        rb.linearVelocity = Vector2.zero;
        moveDirection = Vector2.zero;
    }
}
