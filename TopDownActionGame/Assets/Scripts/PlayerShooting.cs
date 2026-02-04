using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System.Transactions;
using System;

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

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Shoot.performed += OnShootPerformed;
        playerInputActions.Player.Shoot.canceled += OnShootCanceled;

        playerInputActions.Player.ScrollWheel.performed += OnScrollWheel;

        playerInputActions.Player.Switch1.performed += context => EquipWeapon(0);
        playerInputActions.Player.Switch2.performed += context => EquipWeapon(1);
        playerInputActions.Player.Switch3.performed += context => EquipWeapon(2);

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

        playerInputActions.Player.Switch1.performed -= context => EquipWeapon(0);
        playerInputActions.Player.Switch2.performed -= context => EquipWeapon(1);
        playerInputActions.Player.Switch3.performed -= context => EquipWeapon(2);

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
        Vector2 scroll = context.ReadValue<Vector2>();

        if (scroll.y > 0)
        {
            CycleWeapon(+1);
        }
        else if (scroll.y < 0)
        {
            CycleWeapon(-1);
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
}
