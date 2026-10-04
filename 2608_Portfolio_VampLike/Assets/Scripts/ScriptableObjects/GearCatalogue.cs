using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GearCatalogue", menuName = "Catalogues/GearCatalogue")]
public class GearCatalogue : ScriptableObject
{
    [SerializeField] private List<GearInfo> gearList;
    public List<GearInfo> GearList => gearList;
}

[System.Serializable]
public struct GearInfo
{
    [SerializeField] private Gear_Base gearPrefab;

    public Gear_Base GearPrefab => gearPrefab;
    public Gears Type => gearPrefab.Type;

}

public enum Gears
{
    BaseAttack,
    BaseHaste,
    BaseSize,
    Defense,
    HpRegen
}

