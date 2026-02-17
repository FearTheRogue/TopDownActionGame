using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class LowHealthVignetteUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image vignetteImage;

    [Header("Low Health")]
    [SerializeField] private float lowHealthThreshold = 0.3f;
    [SerializeField] private float maxAlpha = 0.45f;
    [SerializeField] private float fadeSpeed = 6f;
    [SerializeField] private float pulseSpeed = 3f;

    private float targetAlpha;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (vignetteImage == null)
            vignetteImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (playerHealth == null || vignetteImage == null)
            return;

        float hp = Mathf.Clamp01((float)playerHealth.CurrentHealth/playerHealth.MaxHealth);
        bool low = hp <= lowHealthThreshold;

        if (low)
        {
            // Stronger vignette the lower your health is
            float intensity = Mathf.InverseLerp(lowHealthThreshold, 0f, hp);
            float pulse = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) * 0.5f;

            targetAlpha = Mathf.Lerp(0f, maxAlpha, intensity) * Mathf.Lerp(0.85f, 1.0f, pulse);
        }
        else
        {
            targetAlpha = 0f;
        }

        Color c = vignetteImage.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.unscaledDeltaTime * fadeSpeed);
        vignetteImage.color = c;
    }
}
