using System.Diagnostics.Contracts;
using System.Transactions;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;   
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);

        Debug.Log($"Player HP: {currentHealth}/{maxHealth}");

        if (currentHealth == 0)
        {
            Die();
        }
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
