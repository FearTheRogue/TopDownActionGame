using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityBurst : MonoBehaviour
{
    [Header("Burst Settings")]
    [SerializeField] private float radius = 2.2f;
    [SerializeField] private float force = 7f;
    [SerializeField] private float cooldown = 7f;
    [SerializeField] private LayerMask enemyLayers;

    private float cooldownTimer;
    private PlayerInputActions playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        playerInput.Player.Enable();
        playerInput.Player.AbilityBurst.performed += OnBurst;
    }

    private void OnDestroy()
    {
        playerInput.Player.AbilityBurst.performed -= OnBurst;
        playerInput.Disable();
    }

    private void Update()
    {
        if (cooldownTimer > 0) 
            cooldownTimer -= Time.deltaTime;
    }

    private void OnBurst(InputAction.CallbackContext context)
    {
        if (cooldownTimer > 0)
            return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayers);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyMovement>(out var move))
            {
                Vector2 dir = (hit.transform.position - transform.position).normalized;
                move.AddKnockback(dir * force);
            }

            if (hit.TryGetComponent<HitReaction>(out var reaction))
            {
                reaction.HitStun();
            }
        }

        cooldownTimer = cooldown;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
