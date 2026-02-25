using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldownUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerDash dash;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownFill;
    [SerializeField] private TextMeshProUGUI cooldownText;

    [Header("Colours")]
    [SerializeField] private Color readyColour = Color.white;
    [SerializeField] private Color cooldownColour = new Color(0.6f, 0.6f, 0.6f, 1f);

    private void Awake()
    {
        if (dash == null)
            dash = FindFirstObjectByType<PlayerDash>();
    }

    private void Update()
    {
        if (dash == null)
            return;

        bool ready = dash.IsReady;

        if (iconImage != null)
            iconImage.color = ready ? readyColour : cooldownColour;

        if (cooldownFill != null)
        {
            cooldownFill.gameObject.SetActive(!ready);

            // Fill shows remaining cooldown
            float t = dash.CooldownRemaining / Mathf.Max(0.0001f, dash.CooldownDuration);
            cooldownFill.fillAmount = t;
        }

        if (cooldownText != null)
        {
            if (ready)
            {
                cooldownText.text = "";
            }
            else
            {
                cooldownText.text = Mathf.CeilToInt(dash.CooldownRemaining).ToString();
            }
        }
    }
}
