using System.Collections;
using UnityEngine;

public class Lightning : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponData weaponData;

    [SerializeField] private Player_EnemyDetector detector;
    [SerializeField] private ObjectPool myPool;

    private Coroutine fireCoroutine;

    private int weaponLevel = 0;
    private Weapon_Stats[] stats;
    private Weapon_Stats currentStats;

    private bool isUpgradeAvailable = true;

    public void Init(Player_EnemyDetector detector, WeaponData weaponData)
    {
        this.detector = detector;
        this.weaponData = weaponData;
        stats = weaponData.Stats;
        currentStats = stats[weaponLevel];
        weaponLevel++;

        myPool.InitObjectPool(weaponData.ProjectilePrefab);
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            Vector2[] enemyPositions = detector.DetectAndReturnRandomEnemyPositions(currentStats.MaxProjectileCount);

            for(int i = 0; i < currentStats.MaxProjectileCount; i++)
            {
                GameObject projectile = myPool.UseObject();

                Lightning_Projectile obj = projectile.GetComponent<Lightning_Projectile>();

                obj.GetPower(currentStats.Power);
                obj.Init(enemyPositions[i],ReturnBoltCount(), currentStats.AreaMagnification, currentStats.ProjectileLifetimeDuration);
                obj.Strike();
            }

            yield return new WaitForSeconds(currentStats.Cooldown);
        }
    }

    private int ReturnBoltCount()
    {
        switch (weaponLevel)
        {
            case 1:case 2:
            {
                return 1;
            }
            case 3:case 4: case 5:
            {
                return 2;
            }
            case 6:
            {
                return 3;
            }
            default: return 0;
        } 

    }
    #region IWeapon
    public void StartFireCoroutine()
    {
        fireCoroutine = StartCoroutine("Fire");
    }

    public void StopFireCoroutine()
    {
        StopCoroutine(fireCoroutine);
    }

    public void Upgrade()
    {
        if (isUpgradeAvailable == false)
        {
            print("이미 무기가 최고단계임");
            return;
        }

        currentStats = stats[weaponLevel];
        weaponLevel++;
        print($"현재 무기 레벨: {weaponLevel}");

        if (weaponLevel == 6)
        {
            isUpgradeAvailable = false;
            print("무기가 최고단계에 도달함");
        }
    }

    #endregion

}
