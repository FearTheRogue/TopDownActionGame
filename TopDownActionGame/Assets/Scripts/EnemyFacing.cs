using System.Net.Http.Headers;
using UnityEngine;

public class EnemyFacing : MonoBehaviour
{
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Transform visuals;
    [SerializeField] private Transform armPivot;

    private Quaternion currentRotation;

    private void Awake()
    {
        if (visuals == null)
            visuals = transform;

        if (armPivot == null)
            armPivot = visuals;

        currentRotation = visuals.rotation;
    }

    public void FaceDirection(Vector2 direction)
    {
        if (!rotate || direction.sqrMagnitude < 0.001f) return;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion target = Quaternion.Euler(0, 0, targetAngle);

        currentRotation = Quaternion.Lerp(visuals.rotation, target, Time.deltaTime * rotationSpeed);

        if (visuals != null)
            visuals.rotation = currentRotation;

        if (armPivot != null)
            armPivot.rotation = currentRotation;
    }

    public void FaceTarget(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        FaceDirection(direction);
    }
}
