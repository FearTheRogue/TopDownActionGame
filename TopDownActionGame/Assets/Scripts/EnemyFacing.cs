using UnityEngine;

public class EnemyFacing : MonoBehaviour
{
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotationSpeed;

    public void FaceDirection(Vector2 direction)
    {
        if (!rotate) return;
        if (direction.sqrMagnitude < 0.001f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, angle), Time.deltaTime * rotationSpeed);
    }

    public void FaceTarget(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - targetPosition;
        FaceDirection(direction);
    }
}
