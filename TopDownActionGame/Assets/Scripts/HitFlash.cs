using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] renderers;

    [Header("Flash Settings")]
    [SerializeField] private int flashes = 2;
    [SerializeField] private float flashOnTime = 0.06f;
    [SerializeField] private float flashOffTime = 0.08f;

    [Header("Tint Settings")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashIntensity = 1f;

    private Coroutine routine;
    private Color[] originalColors;

    private void Awake()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<SpriteRenderer>(true);

        // Cache original colours
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
                originalColors[i] = renderers[i].color;
        }
    }

    public void Play()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        for (int i = 0; i < flashes; i++)
        {
            SetFlash(true);
            yield return new WaitForSeconds(flashOnTime);

            SetFlash(false);
            yield return new WaitForSeconds(flashOffTime);
        }

        SetFlash(false);
        routine = null;
    }

    private void SetFlash(bool active)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] == null) continue;

            if (active)
            {
                renderers[i].color = Color.Lerp(
                    originalColors[i],
                    flashColor,
                    flashIntensity
                );
            }
            else
            {
                renderers[i].color = originalColors[i];
            }
        }
    }
}