using System.Collections;
using UnityEngine;

public abstract class Weapon_Base : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponData weaponData;

    [SerializeField] protected Player_EnemyDetector detector;
    [SerializeField] protected ObjectPool myPool;


    protected Coroutine fireCoroutine;

    protected int weaponLevel = 0;
    protected Weapon_Stats[] stats;
    protected Weapon_Stats currentStats;

    protected bool isUpgradeAvailable = true;

    protected abstract IEnumerator Fire();

    #region IWeapon
    public virtual void Init(Player_EnemyDetector detector, WeaponData weaponData)
    {
        this.detector = detector;
        this.weaponData = weaponData;
        stats = weaponData.Stats;
        currentStats = stats[weaponLevel];
        ApplyStats();
        weaponLevel++;

        SetupObjectPool();
    }

    protected virtual void SetupObjectPool()
    {
        myPool.InitObjectPool(weaponData.ProjectilePrefab);
    }

    public virtual void StartFireCoroutine()
    {
        fireCoroutine = StartCoroutine("Fire");
    }

    public virtual void StopFireCoroutine()
    {
        StopCoroutine(fireCoroutine);
    }
    #endregion

    #region IUpgradeable
    public virtual void Upgrade()
    {
        if (isUpgradeAvailable == false)
        {
            print("이미 무기가 최고단계임");
            return;
        }

        currentStats = stats[weaponLevel];

        ApplyStats();
        StopFireCoroutine();
        StartFireCoroutine();

        weaponLevel++;
        print($"현재 무기 레벨: {weaponLevel}");

        if (weaponLevel == 6)
        {
            isUpgradeAvailable = false;
            print("무기가 최고단계에 도달함");
        }
    }

    protected virtual void ApplyStats()
    {

    }
    #endregion
}
