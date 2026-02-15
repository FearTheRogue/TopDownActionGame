using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image fillImage;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= HandleHealthChanged;
    }

    private void Start()
    {
        // Force an initial update
        if (playerHealth != null)
            HandleHealthChanged(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }

    private void HandleHealthChanged(int current, int max)
    {
        if (fillImage == null || max <= 0)
            return;

        fillImage.fillAmount = Mathf.Clamp01((float)current / max);
    }
}
