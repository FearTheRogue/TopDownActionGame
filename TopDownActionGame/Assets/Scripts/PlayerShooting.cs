using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;

    [Header("Weapons")]
    public WeaponData pistolWeapon;
    public WeaponData automaticWeapon;
    public WeaponData burstWeapon;

    private WeaponData currentWeapon;

    private bool isShooting;
    private float shotTimer;
    private bool isBursting;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Shoot.performed += OnShootPerformed;
        playerInputActions.Player.Shoot.canceled += OnShootCanceled;

        playerInputActions.Player.Switch1.performed += context => EquipWeapon(pistolWeapon);
        playerInputActions.Player.Switch2.performed += context => EquipWeapon(automaticWeapon);
        playerInputActions.Player.Switch3.performed += context => EquipWeapon(burstWeapon);

        playerInputActions.Enable();
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Shoot.performed -= OnShootPerformed;
        playerInputActions.Player.Shoot.canceled -= OnShootCanceled;

        playerInputActions.Player.Switch1.performed -= context => EquipWeapon(pistolWeapon);
        playerInputActions.Player.Switch2.performed -= context => EquipWeapon(automaticWeapon);
        playerInputActions.Player.Switch3.performed -= context => EquipWeapon(burstWeapon);

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

        Instantiate(currentWeapon.bulletPrefab, firePoint.position, firePoint.rotation);
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

    public void EquipWeapon (WeaponData newWeapon)
    {
        currentWeapon = newWeapon;
        shotTimer = 0f;
        isShooting = false;
        isBursting = false;

        Debug.Log($"Equipped weapon {newWeapon.weaponName}");
    }
}
