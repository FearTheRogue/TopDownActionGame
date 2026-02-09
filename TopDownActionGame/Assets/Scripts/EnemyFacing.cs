using System;
using System.Net.Http.Headers;
using UnityEngine;

public class EnemyFacing : MonoBehaviour
{
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Transform visuals;
    [SerializeField] private Transform armPivot;

    private float currentAngle;
    private SpriteFlipper flipper;

    private void Awake()
    {
        if (visuals == null)
            visuals = transform;

        if (armPivot == null)
            armPivot = visuals;

        flipper = GetComponent<SpriteFlipper>();
    }

    public void FaceDirection(Vector2 direction)
    {
        if (!rotate || direction.sqrMagnitude < 0.001f) return;

        if (flipper != null)
            flipper.FaceDirection(direction);

        if (armPivot != null)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotationSpeed);

            armPivot.localRotation = Quaternion.Euler(0, 0, currentAngle);
        }
    }

    public void FaceTarget(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        FaceDirection(direction);
    }
}
