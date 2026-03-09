using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerGameOverHook : MonoBehaviour
{
    private PlayerHealth health;
    private GameOverManager manager;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
        manager = FindFirstObjectByType<GameOverManager>();
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
        // prevent death triggering twice
        manager?.Show();
    }
}
