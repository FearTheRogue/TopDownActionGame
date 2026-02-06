using UnityEngine;

public class CinemachineFollowProxy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform followTarget;

    private Vector2 previousFixedPos;
    private Vector2 currentFixedPos;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (followTarget == null) followTarget = transform;

        previousFixedPos = rb.position;
        currentFixedPos = rb.position;
    }

    private void FixedUpdate()
    {
        previousFixedPos = currentFixedPos;
        currentFixedPos = rb.position;
    }

    private void LateUpdate()
    {
        float alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        alpha = Mathf.Clamp01(alpha);

        Vector2 smoothPos = Vector2.Lerp(previousFixedPos, currentFixedPos, alpha);
        followTarget.position = smoothPos;
    }
}
