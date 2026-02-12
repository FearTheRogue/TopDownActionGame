using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;

    [Header("Weapons")]
    public WeaponData[] weapons;
    public WeaponData pistolWeapon;
    public WeaponData automaticWeapon;
    public WeaponData burstWeapon;

    private WeaponData currentWeapon;
    private int currentWeaponIndex = 0;

    private bool isShooting;
    private float shotTimer;
    private bool isBursting;

    private PlayerInputActions playerInputActions;

    private float scrollCooldown = 0.2f;
    private float lastScrollTime;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Shoot.performed += OnShootPerformed;
        playerInputActions.Player.Shoot.canceled += OnShootCanceled;

        playerInputActions.Player.ScrollWheel.performed += OnScrollWheel;

        playerInputActions.Enable();
    }

    private void Start()
    {
        EquipWeapon(0);
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Shoot.performed -= OnShootPerformed;
        playerInputActions.Player.Shoot.canceled -= OnShootCanceled;

        playerInputActions.Disable();

    }

    private void Update()
    {
        if (currentWeapon == null) return;

        if (currentWeapon.weaponType == WeaponType.Automatic && isShooting)
        {
            shotTimer -= Time.deltaTime;

            if (shotTimer <= 0f)
            {
                FireBullet();
                shotTimer = currentWeapon.fireRate;
            }
        }
    }

    private void OnScrollWheel(InputAction.CallbackContext context)
    {
        // Adds scroll dampening
        if (Time.time < lastScrollTime + scrollCooldown) return;

        lastScrollTime = Time.time;

        // Change weapon
        Vector2 scroll = context.ReadValue<Vector2>();

        if (scroll.y > 0)
            CycleWeapon(+1);
        else if (scroll.y < 0)
            CycleWeapon(-1);
    }

    public void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (currentWeapon == null) return;

        switch (currentWeapon.weaponType)
        {
            case WeaponType.Pistol:
                FireBullet();
                break;
            case WeaponType.Automatic:
                isShooting = true;
                break;
            case WeaponType.Burst:
                if (!isBursting)
                    StartCoroutine(BurstFire());
                break;
        }
    }

    public void OnShootCanceled(InputAction.CallbackContext context)
    {
        if (currentWeapon != null && currentWeapon.weaponType == WeaponType.Automatic)
            isShooting = false;
    }



    private void FireBullet()
    {
        if (currentWeapon == null || currentWeapon.bulletPrefab == null) return;

        GameObject bulletObj = Instantiate(currentWeapon.bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetDamage(currentWeapon.bulletDamage);
        }

        FindFirstObjectByType<CombatCameraZoom>().NotifyCombat();
    }

    private IEnumerator BurstFire()
    {
        if (isBursting || currentWeapon == null) yield break;

        isBursting = true;

        for (int i = 0; i < currentWeapon.burstCount; i++)
        {
            FireBullet();
            yield return new WaitForSeconds(currentWeapon.fireRate);
        }

        isBursting = false;
    }

    public void EquipWeapon (int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        currentWeaponIndex = index;
        currentWeapon = weapons[index];

        shotTimer = 0f;
        isShooting = false;
        isBursting = false;

        Debug.Log($"Equipped weapon {currentWeapon.weaponName}");
    }

    private void CycleWeapon(int direction)
    {
        currentWeaponIndex += direction;

        if (currentWeaponIndex >= weapons.Length)
            currentWeaponIndex = 0;
        if (currentWeaponIndex < 0)
            currentWeaponIndex = weapons.Length - 1;

        EquipWeapon(currentWeaponIndex);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(firePoint.position, 0.05f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.right * 0.5f);
    }
}
