using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] renderers;

    [Header("Flash Settings")]
    [SerializeField] private int flashes = 2;
    [SerializeField] private float flashOnTime = 0.06f;
    [SerializeField] private float flashOffTime = 0.08f;

    [Tooltip("If enabled, toggles renderer visibility instead of tinting")]
    [SerializeField] private bool toggleVisibility = true;

    private Coroutine routine;

    private void Awake()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<SpriteRenderer>(true);
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
            SetVisible(false);
            yield return new WaitForSeconds(flashOnTime);

            SetVisible(true);
            yield return new WaitForSeconds(flashOffTime);
        }

        SetVisible(true);
        routine = null;
    }

    private void SetVisible(bool visible)
    {
        if (renderers == null)
            return;

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] == null)
                continue;

            if (toggleVisibility)
                renderers[i].enabled = visible;
            else
                renderers[i].color = visible ? Color.white : new Color(1f, 1f, 1f, 0.2f);
        }

    }
}
