using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

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

    private int currentCharges;
    private float rechargeTimer;

    private float nextShotTime;

    private bool chargesDirty;
    private int lastNotifiedCharges = int.MinValue;

    [SerializeField] private CinemachineImpulseSource impulseSource;

    public int CurrentCharges => currentCharges;
    public int MaxCharges => currentWeapon != null && currentWeapon.usesCharges ? currentWeapon.maxCharges : 0;
    public bool UsesCharges => currentWeapon != null && currentWeapon.usesCharges;
    public event System.Action OnChargesChanged;
    //private void NotifyChargesChanged() => OnChargesChanged?.Invoke();

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
        NotifyChargesChanged();

        if (impulseSource == null)
            impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Shoot.performed -= OnShootPerformed;
        playerInputActions.Player.Shoot.canceled -= OnShootCanceled;

        playerInputActions.Disable();

    }

    private void Update()
    {
        if (PauseManager.Paused)
        {
            isShooting = false;
            return;
        }

        if (currentWeapon == null) return;

        if (currentWeapon.weaponType == WeaponType.Automatic && isShooting)
        {
            //shotTimer -= Time.deltaTime;

            //if (shotTimer <= 0f)
            //{
            //    FireBullet();
            //    shotTimer = currentWeapon.fireRate;
            //}
            TryFireOnce();
        }

        if (currentWeapon != null && currentWeapon.usesCharges)
        {
            if (currentCharges < currentWeapon.maxCharges)
            {
                rechargeTimer += Time.deltaTime;

                if (rechargeTimer >= currentWeapon.rechargeTime)
                {
                    currentCharges++;
                    rechargeTimer = 0f;
                    NotifyChargesChanged();
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (!chargesDirty)
            return;

        chargesDirty = false;

        if (currentCharges == lastNotifiedCharges)
            return;

        lastNotifiedCharges = currentCharges;
        OnChargesChanged?.Invoke();
    }

    private void OnScrollWheel(InputAction.CallbackContext context)
    {
        if (PauseManager.Paused)
            return;

        // Adds scroll dampening
        if (Time.unscaledTime < lastScrollTime + scrollCooldown) return;

        lastScrollTime = Time.unscaledTime;

        // Change weapon
        Vector2 scroll = context.ReadValue<Vector2>();

        if (scroll.y > 0)
            CycleWeapon(+1);
        else if (scroll.y < 0)
            CycleWeapon(-1);
    }

    public void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (PauseManager.Paused)
            return;

        if (currentWeapon == null) return;

        switch (currentWeapon.weaponType)
        {
            case WeaponType.Pistol:
                TryFireOnce();
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
        if (PauseManager.Paused)
            return;

        if (currentWeapon != null && currentWeapon.weaponType == WeaponType.Automatic)
            isShooting = false;
    }

    private void FireBullet()
    {
        if (currentWeapon == null || currentWeapon.bulletPrefab == null)
            return;

        float spread = currentWeapon.spreadAngle;
        float angleOffset = UnityEngine.Random.Range(-spread, spread);

        Quaternion rot = firePoint.rotation * Quaternion.Euler(0f, 0f, angleOffset);

        GameObject bulletObj = Instantiate(currentWeapon.bulletPrefab, firePoint.position, rot);

        impulseSource?.GenerateImpulse();

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetDamage(currentWeapon.bulletDamage);
        }
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

        currentCharges = currentWeapon.usesCharges ? currentWeapon.maxCharges : 0;
        NotifyChargesChanged();

        rechargeTimer = 0f;

        nextShotTime = 0f;

        //Debug.Log($"Equipped weapon {currentWeapon.weaponName}");
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

    private void TryFireOnce()
    {
        if (Time.time < nextShotTime)
            return;

        if (currentWeapon.usesCharges)
        {
            if (currentCharges <= 0)
                return;

            currentCharges--;
            NotifyChargesChanged();
        }

        FireBullet();
        nextShotTime = Time.time + currentWeapon.fireRate;
    }

    private void NotifyChargesChanged()
    {
        chargesDirty = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(firePoint.position, 0.05f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.right * 0.5f);
    }
}
