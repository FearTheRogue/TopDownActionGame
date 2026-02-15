using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image fillImage;

    [Header("Animation")]
    [SerializeField] private float fillSpeed = 8f;

    private float targetFill = 1f;

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
        {
            targetFill = Mathf.Clamp01((float)playerHealth.CurrentHealth / playerHealth.MaxHealth);
            
            if (fillImage != null)
                fillImage.fillAmount = targetFill;
        }
    }

    private void Update()
    {
        if (fillImage == null)
            return;

        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * fillSpeed);

        if (Mathf.Abs(fillImage.fillAmount - targetFill) < 0.001f)
            fillImage.fillAmount = targetFill;
    }

    private void HandleHealthChanged(int current, int max)
    {
        if (max <= 0)
            return;

        targetFill = Mathf.Clamp01((float)current / max);
    }
}
