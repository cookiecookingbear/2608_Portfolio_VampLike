using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Stats", menuName = "SettingValues/Weapon_Stats")]
public class Weapon_Stats : ScriptableObject
{
    [SerializeField] private float power;
    [SerializeField] private float cooldown;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float maxProjectileCount;
    [SerializeField] private float projectileLifetimeDuration;
    [SerializeField] private float knockbackPower;

    public float Power => power;
    public float Cooldown => cooldown;
    public float ProjectileSpeed => projectileSpeed;
    public float MaxProjectileCount => maxProjectileCount;
    public float ProjectileLifetimeDuration => projectileLifetimeDuration;
    public float KnockbackPower => knockbackPower;
}
