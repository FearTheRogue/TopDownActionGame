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
    public int burstCount;
    public GameObject bulletPrefab;
}