using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]

public class WeaponData : ScriptableObject
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Weapon_Stats stats;
    [SerializeField] private Weapon_Stats[] upgrades;

    public GameObject ProjectilePrefab => projectilePrefab;
    public Weapon_Stats Stats => stats;
    public Weapon_Stats[] Upgrades => upgrades;
}
