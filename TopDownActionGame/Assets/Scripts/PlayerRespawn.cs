using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 1.2f;

    private PlayerHealth health;
    private PlayerController controller;
    private Rigidbody2D rb;
    private bool isDead;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        if (respawnPoint == null)
            respawnPoint = transform;
    }

    private void OnEnable()
    {
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        if (isDead)
            return;

        isDead = true;

        // Disable control & stop motion
        if (controller != null)
            controller.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        controller?.ClearExternalVelocity();

        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        // teleport to respawn
        rb.position = respawnPoint.position;

        // reset health
        health.ResetHealth();
        controller?.ClearExternalVelocity();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (controller != null)
            controller.enabled = true;

        isDead = false;
    }

    public void SetRespawnPoint(Transform newPoint)
    {
        if (newPoint != null)
            respawnPoint = newPoint;
    }
}
