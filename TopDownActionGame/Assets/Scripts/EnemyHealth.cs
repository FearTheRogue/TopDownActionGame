using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Apply damage to this enemy game object
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (amount < 0) return;

        currentHealth -= amount;
        //OnDamage?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Heal the enemy by a certain amount
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void Die()
    {
        //OnDeath?.Invoke();

        Destroy(gameObject);
    }


}
