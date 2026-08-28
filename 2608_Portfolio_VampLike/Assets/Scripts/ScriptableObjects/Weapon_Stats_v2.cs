using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Stats_v2", menuName = "Scriptable Objects/Weapon_Stats_v2")]
public class Weapon_Stats_v2 : ScriptableObject
{
    [SerializeField] private WeaponStats stats;
    public WeaponStats Stats => stats;
}

[System.Serializable]
public struct WeaponStats
{
    [SerializeField] private float power;
    [SerializeField] private float cooldown;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float maxProjectileCount;
    [SerializeField] private float projectileLifetimeDuration;

    public float Power => power;
    public float Cooldown => cooldown;
    public float ProjectileSpeed => projectileSpeed;
    public float MaxProjectileCount => maxProjectileCount;
    public float ProjectileLifetimeDuration => projectileLifetimeDuration;
}
