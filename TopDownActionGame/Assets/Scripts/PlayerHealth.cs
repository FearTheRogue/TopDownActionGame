using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 3.5f;

    private int currentHealth;
   // private Rigidbody2D rb;

    private CombatState combatState;
    private HitFlash hitFlash;

    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged; // current, max

    private bool invulnerable;
    [SerializeField] private float damageCooldown = 0.5f;
    private float damageCooldownTimer;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        hitFlash = GetComponent<HitFlash>();
        //rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        combatState = FindFirstObjectByType<CombatState>();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        if (damageCooldownTimer > 0f)
            damageCooldownTimer -= Time.deltaTime;
    }

    // Interface method (works for anything that only knows IDamageable)
    public void TakeDamage(int damage)
    {
        ApplyDamage(damage);
    }

    public void TakeDamage(int damage, Vector2 hitFromWorldPos)
    {
        ApplyDamage(damage);

        Vector2 dir = (Vector2)transform.position - hitFromWorldPos;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        dir.Normalize();

        var controller = GetComponent<PlayerController>();

        if (controller != null)
            controller.AddKnockback(dir * knockbackForce);

    }

    private void ApplyDamage(int damage)
    {
        if (damage <= 0 || currentHealth <= 0 || invulnerable)
            return;

        if (damageCooldownTimer > 0f)
            return;

        damageCooldownTimer = damageCooldown;

        combatState?.NotifyCombat("PlayerHealth", 3f);

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        //Debug.Log($"Player HP: {currentHealth}/{maxHealth}");

        hitFlash?.Play();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth == 0)
            OnDeath?.Invoke();
    }

    public void SetInvulnerable(float seconds)
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(InvulnRoutine(seconds));
    }

    private IEnumerator InvulnRoutine(float seconds)
    {
        invulnerable = true;
        yield return new WaitForSeconds(seconds);
        invulnerable = false;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log("Player Died");

        var controller = GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.enabled = false;
        }
    }
}
