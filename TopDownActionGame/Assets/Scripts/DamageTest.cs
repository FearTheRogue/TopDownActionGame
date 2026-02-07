using UnityEngine;
using UnityEngine.InputSystem;

public class DamageTest : MonoBehaviour
{
    private PlayerHealth health;

    private void Start()
    {
        health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            health.TakeDamage(10);
        }
    }
}
