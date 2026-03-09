using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponData weapon;
    [SerializeField] private bool autoEquip = true;

    private void Reset()
    {
        // Ensure trigger behaviour
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        var shooting = other.GetComponent<PlayerShooting>();
        if (shooting == null)
            return;

        shooting.AddWeapon(weapon, autoEquip);

        Destroy(gameObject);
    }
}
