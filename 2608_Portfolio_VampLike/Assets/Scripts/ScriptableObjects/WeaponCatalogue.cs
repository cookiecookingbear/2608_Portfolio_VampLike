using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeaponCatalogue", menuName = "Catalogues/WeaponCatalogue")]
public class WeaponCatalogue : ScriptableObject
{
    [SerializeField] private List<WeaponInfo> weaponInfoList;

    public List<WeaponInfo> WeaponInfoList => weaponInfoList;
}

[System.Serializable]
    public struct WeaponInfo
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private GameObject weaponPrefab;

    public WeaponData WeaponData => weaponData;
    public GameObject WeaponPrefab => weaponPrefab;
}

