using UnityEngine;

public enum WeaponType
{
    Pistol,
    Automatic,
    Burst
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class WeaponData : ScriptableObject 
{
    public string weaponName;
    public WeaponType weaponType;
    public float fireRate;
    public int bulletDamage;
    public int burstCount;
    public GameObject bulletPrefab;

    public float spreadAngle = 0;

    [Header("Charges")]
    public int maxCharges = 6;
    public float rechargeTime = 1.2f;
    public bool usesCharges = true;
}