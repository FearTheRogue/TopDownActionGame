using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldownUI : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private Transform sourceRoot;
    [SerializeField] private string sourceId = "Dash";
    [SerializeField] private AbilityCooldownSource source;

    [Header("UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownFill;
    [SerializeField] private TextMeshProUGUI cooldownText;

    [Header("Colours")]
    [SerializeField] private Color readyColour = Color.white;
    [SerializeField] private Color cooldownColour = new Color(0.6f, 0.6f, 0.6f, 1f);

    private void Awake()
    {
        if (source == null)
            source = FindSource();

        if (source == null)
            Debug.LogError($"{name}: could not find AbilityCooldownSource with id '{sourceId}'.", this);
    }

    private AbilityCooldownSource FindSource()
    {
        if (sourceRoot == null)
        {
            // Fallback: try player
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) 
                sourceRoot = player.transform;
        }

        if (sourceRoot == null)
            return null;

        var sources = sourceRoot.GetComponents<AbilityCooldownSource>();

        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i] != null && sources[i].Id == sourceId)
                return sources[i];
        }

        return null;
    }

    private void Update()
    {
        if (source == null)
            return;

        bool ready = source.IsReady;

        if (iconImage != null)
            iconImage.color = ready ? readyColour : cooldownColour;

        if (cooldownFill != null)
        {
            cooldownFill.gameObject.SetActive(!ready);
            float t = source.CooldownRemaining / Mathf.Max(0.0001f, source.CooldownDuration);
            cooldownFill.fillAmount = t;
        }

        if (cooldownText != null)
        {
            cooldownText.text = ready ? "" : Mathf.CeilToInt(source.CooldownRemaining).ToString();
        }
    }
}
