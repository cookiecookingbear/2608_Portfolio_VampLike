using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 최대 3개의 기어를 장착하는 모듈
/// 기어: 무기의 가장 기초 능력치를 향상시켜주는 장비
/// 레벨업시 선택한 선택지의 정보를 받아 기어를 추가/업그레이드
/// 기본 공격력, 기본 쿨다운, 기본 공격범위, 체력재생, 방어력
/// 다섯 가지 기어
/// </summary>
public class Player_GearModule : MonoBehaviour
{
    private GearOption totalGearOptions = GearOption.Identity;

    [SerializeField] private GearCatalogue catalogue;

    private Dictionary<Gears, Gear_Base> gears = new Dictionary<Gears, Gear_Base>();

    #region Testing

    private void Update()
    {
        CheatKey();
    }

    private void CheatKey()
    {
        //att
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SelectGear(catalogue.GearList[0]);
        }

        //def
        if (Input.GetKeyDown(KeyCode.X))
        {
            SelectGear(catalogue.GearList[1]);
        }

        //haste
        if (Input.GetKeyDown(KeyCode.C))
        {
            SelectGear(catalogue.GearList[2]);
        }

        //hp
        if (Input.GetKeyDown(KeyCode.V))
        {
            SelectGear(catalogue.GearList[3]);
        }

        //size
        if (Input.GetKeyDown(KeyCode.B))
        {
            SelectGear(catalogue.GearList[4]);
        }

    }

    #endregion

    private void SelectGear(GearInfo gearInfo)
    {
        if(gears.ContainsKey(gearInfo.Type) == false)
        {
            AddGear(gearInfo);
            return;
        }

        UpgradeGear(gearInfo);
    }

    private void AddGear(GearInfo gearInfo)
    {
        Gear_Base newGear= Instantiate(gearInfo.GearPrefab, transform);

        newGear.Init();
        totalGearOptions.Apply(gearInfo.Type, newGear.Stat);

        gears.Add(gearInfo.Type, newGear);
    }

    private void UpgradeGear(GearInfo gearInfo)
    {
        gears[gearInfo.Type].Upgrade();
        totalGearOptions.Apply(gearInfo.Type, gears[gearInfo.Type].Stat);
    }

}
