using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;

    public void Shoot(Vector2 dir, float speed, float lifetime)
    {
        direction = dir.normalized;
        this.speed = speed;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
}
