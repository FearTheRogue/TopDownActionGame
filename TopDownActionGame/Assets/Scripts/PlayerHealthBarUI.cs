using Unity.Cinemachine;
using Unity.Collections;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image mainFill;
    [SerializeField] private Image chipFill;

    [Header("Animation")]
    [SerializeField] private float chipDelay = 0.25f;
    [SerializeField] private float chipSpeed = 3f;
    [SerializeField] private float colourFlashSpeed = 8f;

    [Header("Low Health Pulse")]
    [SerializeField] private bool isPulsing = true;
    [SerializeField] private float lowHealthThreshold = 0.3f; // 30%
    [SerializeField] private float pulseSpeed = 3.0f;
    [SerializeField] private float pulseColourStrength = 0.35f;
    [SerializeField] private float pulseScaleStrength = 0.04f;
    [SerializeField] private RectTransform pulseTarget;

    private float targetFill = 1f;
    private float chipTargetFill = 1f;
    private float chipDelayTimer;

    private Color baseColour;
    private Color flashColour = new Color(1f, 0.3f, 0.3f);

    private Vector3 baseScale;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();

        baseColour = mainFill.color;

        if (pulseTarget == null && mainFill != null)
            pulseTarget = mainFill.rectTransform;

        if (pulseTarget != null)
            baseScale = pulseTarget.localScale;
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

            if (mainFill != null)
                mainFill.fillAmount = targetFill;
        }
    }

    private void Update()
    {
        // MAIN BAR - instant
        mainFill.fillAmount = targetFill;

        // CHIP BAR - delayed ease
        if (chipFill.fillAmount > chipTargetFill)
        {
            if (chipDelayTimer > 0f)
            {
                chipDelayTimer -= Time.deltaTime;
            }
            else
            {
                chipFill.fillAmount = Mathf.Lerp(chipFill.fillAmount, chipTargetFill, Time.deltaTime * chipSpeed);
            }
        }
        else
        {
            chipFill.fillAmount = chipTargetFill;
        }

        if (isPulsing)
        {
            // Smooth colour return
            mainFill.color = Color.Lerp(mainFill.color, baseColour, Time.deltaTime * colourFlashSpeed);

            bool low = targetFill <= lowHealthThreshold;

            if (low)
            {
                float t = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) * 0.5f;

                // Colour pulse (towards a slightly brighter red
                Color pulseCol = Color.Lerp(baseColour, new Color(1f, 0.2f, 0.2f), t * pulseColourStrength);
                mainFill.color = pulseCol;

                // Scale pulse
                if (pulseTarget != null)
                {
                    float s = 1f + (t - 0.5f) * 2f * pulseColourStrength;
                    pulseTarget.localScale = baseScale * s;
                }
            }
            else
            {
                // return to normal
                if (pulseTarget != null)
                    pulseTarget.localScale = Vector3.Lerp(pulseTarget.localScale, baseScale, Time.unscaledDeltaTime * 10f);
            }
        }
    }

    private void HandleHealthChanged(int current, int max)
    {
        float newFill = Mathf.Clamp01((float)current / max);

        if (newFill < targetFill)
        {
            // Took Damage
            chipTargetFill = newFill;
            chipDelayTimer = chipDelay;

            // Flash colour
            mainFill.color = flashColour;
        }
        else
        {
            // Healed
            chipTargetFill= newFill;
            chipFill.fillAmount = newFill;
        }

        targetFill = newFill;
    }
}