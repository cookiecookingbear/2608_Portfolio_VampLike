using System;
using UnityEngine;

public class Gear_Base : MonoBehaviour, IGear
{
    [SerializeField] protected Gears myType;
    [SerializeField] protected Gear_Stats stats;

    protected int gearLevel = 0;
    protected bool isUpgradeAvailable = true;
    protected float currentStat;

    public Action<float> GearUpdate;
    public Gears Type => myType;
    public float Stat => currentStat;

    #region IUpgradeable
    public virtual void Upgrade()
    {
        if(isUpgradeAvailable == false)
        {
            print("이미 기어가 최고단계임");
            return;
        }

        currentStat = stats.Stats[gearLevel];
        UpdateGearOption();
        gearLevel++;
        print($"현재 {myType}기어 레벨: {gearLevel}");

        if(gearLevel == 6)
        {
            isUpgradeAvailable = false;
            print("기어가 최고단계에 도달함");
        }
    }
    #endregion

    #region IGear
    public void Init()
    {
        currentStat = stats.Stats[gearLevel];
        UpdateGearOption();
        gearLevel++;
    }

    public void UpdateGearOption()
    {
        GearUpdate?.Invoke(currentStat);
    }
    #endregion
}

