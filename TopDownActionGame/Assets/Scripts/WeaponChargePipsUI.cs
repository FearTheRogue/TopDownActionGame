using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChargePipsUI : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private PlayerShooting shooting;

    [Header("Pip Setup")]
    [SerializeField] private Image pipPrefab;
    [SerializeField] private Transform pipParent;

    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Visuals")]
    [SerializeField] private Color filledColour = Color.white;
    [SerializeField] private Color emptyColour = new Color(1f, 1f, 1f, 0.25f);

    private readonly List<Image> pips = new();

    private void Awake()
    {
        if (pipParent == null)
            pipParent = transform;

        if (shooting == null)
            shooting = FindFirstObjectByType<PlayerShooting>();
    }

    private void OnEnable()
    {
        if (shooting != null)
            shooting.OnChargesChanged += Refresh;

        Refresh();
    }

    private void OnDisable()
    {
        if (shooting != null)
            shooting.OnChargesChanged -= Refresh;
    }

    private void Refresh()
    {
        if (shooting == null)
            return;

        // Hide entirely if the current weapon doesn't use charges
        if (!shooting.UsesCharges)
        {
            EnsurePipCount(0); // clears pips
            return;
        }

        int max = shooting.MaxCharges;
        int current = shooting.CurrentCharges;

        EnsurePipCount(max);

        for (int i = 0; i < pips.Count; i++)
        {
            bool filled = i < current;
            pips[i].color = filled ? filledColour : emptyColour;
        }
    }

    private void EnsurePipCount(int count)
    {
        // Add pips if needed
        while (pips.Count < count)
        {
            Image pip = Instantiate(pipPrefab, pipParent);
            pips.Add(pip);
        }

        // Remove pips if needed
        while (pips.Count > count)
        {
            Image pip = pips[pips.Count - 1];
            pips.RemoveAt(pips.Count - 1);

            if (pip != null)
                Destroy(pip.gameObject);
        }
    }
}
