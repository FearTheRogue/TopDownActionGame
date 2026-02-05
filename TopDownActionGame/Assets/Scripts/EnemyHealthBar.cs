using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyHealth health;
    [SerializeField] private Image fillImage;

    private void Start()
    {
        if (health == null)
            health = GetComponentInParent<EnemyHealth>();

        // Subscribe to the damage event
        health.OnDamage += UpdateBar;
        health.OnDeath += HideBar;

        UpdateBar();
    }

    private void UpdateBar()
    {
        fillImage.fillAmount = health.GetHealthPercent();
    }

    private void LateUpdate()
    {
        transform.localRotation = Quaternion.identity;
    }

    private void HideBar()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (health == null) return;

        health.OnDamage -= UpdateBar;
        health.OnDeath -= HideBar;
    }
}
