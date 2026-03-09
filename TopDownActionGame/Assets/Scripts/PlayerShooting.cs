using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;

    [Header("Weapons")]
    [SerializeField] private bool startWithNoWeapon = true;
    [SerializeField] private List<WeaponData> ownedWeapons = new();

    [Header("Weapon Switching")]
    [SerializeField] private float scrollCooldown = 0.2f;

    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;
    
    private PlayerInputActions playerInputActions;
    private WeaponData currentWeapon;
    private int currentWeaponIndex = 0;

    // Firing state
    private bool isShooting;
    private bool isBursting;
    private float nextShotTime;

    // Charges state (for relic-style weapons
    private int currentCharges;
    private float rechargeTimer;

    // UI update debouncing (prevents "pip flicker" when regen + firing happen in same frame)
    private bool chargesDirty;
    private int lastNotifiedCharges = int.MinValue;

    private float lastScrollTime;

    public int CurrentCharges => currentCharges;
    public int MaxCharges => currentWeapon != null && currentWeapon.usesCharges ? currentWeapon.maxCharges : 0;
    public bool UsesCharges => currentWeapon != null && currentWeapon.usesCharges;

    public event System.Action OnChargesChanged;

    private bool Blocked => PauseManager.Paused || GameOverManager.GameOverActive;
    private bool HasWeaponEquipped => currentWeapon != null;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Shoot.performed += OnShootPerformed;
        playerInputActions.Player.Shoot.canceled += OnShootCanceled;
        playerInputActions.Player.ScrollWheel.performed += OnScrollWheel;

    }

    private void OnEnable()
    {
        playerInputActions?.Player.Enable();
    }
    private void OnDisable()
    {
        // If we pause/disable mid-auto fire, make sure it doesn't resume unexpectedly.
        isShooting = false;

        playerInputActions?.Player.Disable();
    }

    private void Start()
    {
        if (impulseSource == null)
            impulseSource = GetComponent<CinemachineImpulseSource>();

        if (startWithNoWeapon)
        {
            UnequipWeapon();
        }
        else
        {
            //EquipWeapon(0);
            MarkChargesDirty();
        }
    }

    private void OnDestroy()
    {
        if (playerInputActions == null)
            return;

        playerInputActions.Player.Shoot.performed -= OnShootPerformed;
        playerInputActions.Player.Shoot.canceled -= OnShootCanceled;
        playerInputActions.Player.ScrollWheel.performed -= OnScrollWheel;
    }

    private void Update()
    {
        if (Blocked)
        {
            isShooting = false;
            return;
        }

        if (currentWeapon == null)
            return;

        // Continuous fire only applies to automatic weapons
        if (currentWeapon.weaponType == WeaponType.Automatic && isShooting)
            TryFireOnce();

        UpdateChargeRegen();
    }

    private void LateUpdate()
    {
        // Fire the UI event once per frame at most to avoid flicker
        if (!chargesDirty)
            return;

        chargesDirty = false;

        if (currentCharges == lastNotifiedCharges)
            return;

        lastNotifiedCharges = currentCharges;
        OnChargesChanged?.Invoke();
    }

    // ----------------------------
    // Input handlers
    // ----------------------------

    private void OnScrollWheel(InputAction.CallbackContext context)
    {
        if (Blocked)
            return;

        // Use unscalableTime so the cooldown behaves consistently even if timescale changes.
        if (Time.unscaledTime < lastScrollTime + scrollCooldown)
            return;

        lastScrollTime = Time.unscaledTime;

        Vector2 scroll = context.ReadValue<Vector2>();

        //if (scroll.y > 0)
        //    CycleWeapon(+1);
        //else if (scroll.y < 0)
        //    CycleWeapon(-1);
    }
    public void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (Blocked)
            return;

        if (currentWeapon == null)
            return;

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
        if (Blocked)
            return;

        if (currentWeapon != null && currentWeapon.weaponType == WeaponType.Automatic)
            isShooting = false;
    }

    // ----------------------------
    // Weapon control
    // ----------------------------

    public void AddWeapon(WeaponData weapon, bool equipNow = true)
    {
        if (weapon == null)
            return;

        if (!ownedWeapons.Contains(weapon))
            ownedWeapons.Add(weapon);

        if (equipNow)
            EquipWeaponByReference(weapon);
    }

    public void EquipWeaponByReference(WeaponData weapon)
    {
        if (weapon == null)
            return;

        currentWeapon = weapon;

        isShooting = false;
        isBursting = false;
        nextShotTime = 0;

        currentCharges = currentWeapon.usesCharges ? currentWeapon.maxCharges : 0;
        rechargeTimer = 0f;
        MarkChargesDirty();
    }

    //public void EquipWeapon (int index)
    //{
    //    if (weapons == null || weapons.Length == 0)
    //        return;
    //    if (index < 0 || index >= weapons.Length)
    //        return;

    //    currentWeaponIndex = index;
    //    currentWeapon = weapons[index];

    //    // Reset firing state when swapping
    //    isShooting = false;
    //    isBursting = false;
    //    nextShotTime = 0f;

    //    // Charges initialise per weapon type
    //    currentCharges = currentWeapon.usesCharges ? currentWeapon.maxCharges : 0;
    //    rechargeTimer = 0f;
    //    MarkChargesDirty();
    //}

    public void UnequipWeapon()
    {
        currentWeapon = null;
        currentWeaponIndex = 0;

        isShooting = false;
        isBursting = false;
        nextShotTime = 0f;

        currentCharges = 0;
        rechargeTimer = 0f;

        MarkChargesDirty();
    }

    //private void CycleWeapon(int direction)
    //{
    //    if (weapons == null || weapons.Length == 0)
    //        return; 

    //    currentWeaponIndex += direction;

    //    if (currentWeaponIndex >= weapons.Length)
    //        currentWeaponIndex = 0;
    //    else if (currentWeaponIndex < 0)
    //        currentWeaponIndex = weapons.Length - 1;

    //    EquipWeapon(currentWeaponIndex);
    //}

    // ----------------------------
    // Firing logic
    // ----------------------------

    private void TryFireOnce()
    {
        if (currentWeapon == null) 
            return;

        // Rate limit (prevents click spam)
        if (Time.time < nextShotTime)
            return;

        // Charges gate (relic pacing)
        if (currentWeapon.usesCharges)
        {
            if (currentCharges <= 0)
                return;

            currentCharges--;
            MarkChargesDirty();
        }

        FireBullet();
        nextShotTime = Time.time + currentWeapon.fireRate;
    }

    private void FireBullet()
    {
        if (firePoint == null)
            return;

        if (currentWeapon == null || currentWeapon.bulletPrefab == null)
            return;

        float spread = currentWeapon.spreadAngle;
        float angleOffset = Random.Range(-spread, spread);

        Quaternion rot = firePoint.rotation * Quaternion.Euler(0f, 0f, angleOffset);

        GameObject bulletObj = Instantiate(currentWeapon.bulletPrefab, firePoint.position, rot);

        // Camera shake is subtle feedback; safe to skip if impulse isn't configured.
        impulseSource?.GenerateImpulse();

        if (bulletObj.TryGetComponent<Bullet>(out var bullet))
            bullet.SetDamage(currentWeapon.bulletDamage);
    }

    private IEnumerator BurstFire()
    {
        if (isBursting || currentWeapon == null) 
            yield break;

        isBursting = true;

        for (int i = 0; i < currentWeapon.burstCount; i++)
        {
            if (Blocked) // prevents firing during pause/game over
                break;
            TryFireOnce(); // ensures charges + rate limit are respected
            yield return new WaitForSeconds(currentWeapon.fireRate);
        }

        isBursting = false;
    }

    // ----------------------------
    // Charges
    // ----------------------------

    private void UpdateChargeRegen()
    {
        if (currentWeapon == null || !currentWeapon.usesCharges)
            return;

        if (currentCharges >= currentWeapon.maxCharges)
            return;
            
        rechargeTimer += Time.deltaTime;

        if (rechargeTimer >= currentWeapon.rechargeTime)
        {
            currentCharges++;
            rechargeTimer = 0f;
            MarkChargesDirty();
        }
    }

    private void MarkChargesDirty()
    {
        chargesDirty = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(firePoint.position, 0.05f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.right * 0.5f);
    }
#endif
}
