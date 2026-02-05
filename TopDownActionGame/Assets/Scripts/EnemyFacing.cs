using System.Net.Http.Headers;
using UnityEngine;

public class EnemyFacing : MonoBehaviour
{
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Transform visuals;

    public void FaceDirection(Vector2 direction)
    {
        if (!rotate || direction.sqrMagnitude < 0.001f) return;

        if (visuals == null) visuals = transform;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion target = Quaternion.Euler(0, 0, targetAngle);

        visuals.rotation = Quaternion.Lerp(visuals.rotation, target, Time.deltaTime * rotationSpeed);
    }

    public void FaceTarget(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        FaceDirection(direction);
    }
}
