using System.Collections.Generic;
using Unity.Cinemachine;
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

        TryBindShooting();
    }

    private void OnEnable()
    {
        TrySubscribe();

        Refresh();
    }

    private void OnDisable()
    {
       Unsubscribe();
    }

    private void Update()
    {
        // If shooting wasn't ready on Awake/OnEnable (scene init order), keep trying.
        if (shooting == null)
        {
            TryBindShooting();
            TrySubscribe();

            // Don't spam refresh constantly; once bound, Refresh() will run via event anyway.
            if (shooting != null) 
                Refresh();
        }
    }

    private void TryBindShooting()
    {
        if (shooting != null)
            return;

        shooting = FindFirstObjectByType<PlayerShooting>();
    }

    private void TrySubscribe()
    {
        if (shooting == null)
            return;

        // Avoid double-subscribe
        shooting.OnChargesChanged -= Refresh;
        shooting.OnChargesChanged += Refresh;
    }

    private void Unsubscribe()
    {
        if (shooting != null)
            shooting.OnChargesChanged -= Refresh;
    }

    private void Refresh()
    {
        if (shooting == null)
            return;

        // Hide entirely if the current weapon doesn't use charges
        if (!shooting.UsesCharges || shooting.MaxCharges <= 0)
        {
            EnsurePipCount(0); // clears pips

            // Hide softly
            SetVisible(false);
            return;
        }

        SetVisible(true);

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
        if (pipPrefab == null || pipParent == null)
            return;

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

    private void SetVisible(bool visible)
    {
        if (canvasGroup == null)
            return;

        canvasGroup.alpha = visible ? 1 : 0;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}
