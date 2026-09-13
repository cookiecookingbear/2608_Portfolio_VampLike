using UnityEngine;
using System.Collections;

public class Fork : MonoBehaviour,IWeapon
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

    public void StartFireCoroutine()
    {
        fireCoroutine = StartCoroutine("Fire");
    }

    public void StopFireCoroutine()
    {
        StopCoroutine(fireCoroutine);
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            Vector2 direction = detector.DetectNearestMonsterDirection();

            //GameObject projectile = Instantiate(weaponData.ProjectilePrefab, transform.position, Quaternion.Euler(direction));
            GameObject projectile = myPool.UseObject();

            Fork_Projectile obj = projectile.GetComponent<Fork_Projectile>();
            obj.Init(transform.position, direction, currentStats.ProjectileSpeed, currentStats.ProjectileLifetimeDuration, currentStats.KnockbackPower);
            obj.GetPower(currentStats.Power);

            yield return new WaitForSeconds(currentStats.Cooldown);
        }//while
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
}
