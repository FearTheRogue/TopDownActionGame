using Unity.Cinemachine;
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

    private float targetFill = 1f;
    private float chipTargetFill = 1f;
    private float chipDelayTimer;

    private Color baseColour;
    private Color flashColour = new Color(1f, 0.3f, 0.3f);

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();

        baseColour = mainFill.color;
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

        // Smooth colour return
        mainFill.color = Color.Lerp(mainFill.color, baseColour, Time.deltaTime * colourFlashSpeed);
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