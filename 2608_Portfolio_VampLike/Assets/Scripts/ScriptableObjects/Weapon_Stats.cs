using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Stats", menuName = "SettingValues/Weapon_Stats")]
public class Weapon_Stats : ScriptableObject
{
    [SerializeField] private float power;
    [SerializeField] private float cooldown;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int maxProjectileCount;
    [SerializeField] private float projectileLifetimeDuration;
    [SerializeField] private float knockbackPower;
    [SerializeField] private float areaMagnification;

    public float Power => power;
    public float Cooldown => cooldown;
    public float ProjectileSpeed => projectileSpeed;
    public int MaxProjectileCount => maxProjectileCount;
    public float ProjectileLifetimeDuration => projectileLifetimeDuration;
    public float KnockbackPower => knockbackPower;
    public float AreaMagnification => areaMagnification;
}
