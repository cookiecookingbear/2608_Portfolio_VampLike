using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Stats", menuName = "SettingValues/Weapon_Stats")]
public class Weapon_Stats : ScriptableObject
{
    [SerializeField] private float power;
    [SerializeField] private float cooldown;
    [SerializeField] private float projectileSpeed;

    public float Power => power;
    public float Cooldown => cooldown;
    public float ProjectileSpeed => projectileSpeed;
}
