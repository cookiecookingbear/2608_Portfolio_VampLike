using UnityEngine;
using System.Collections;

//포크_v1과 다르게 다수의 투사체가 관통하며 일정 시간동안 날아간다.
public class Fork_v2 : MonoBehaviour,IWeapon
{
    [SerializeField] private WeaponData weaponData;

    [SerializeField] private Player_EnemyDetector detector;
    [SerializeField] private ObjectPool myPool;

    private Coroutine fireCoroutine;

    private int weaponLevel = 0;
    private Weapon_Stats[] stats;
    private Weapon_Stats currentStats;

    private bool isUpgradeAvailable = true;

    [SerializeField, Tooltip("projectile spread angle as degree")] private float spreadAngleDeg = 50f;

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

            for (int i = 0; i < currentStats.MaxProjectileCount; i++)
            {
                GameObject projectile = myPool.UseObject();
                Fork_v2_Projectile obj = projectile.GetComponent<Fork_v2_Projectile>();

                float angle = 0;
                if(currentStats.MaxProjectileCount > 1)
                {
                    angle = 0 - spreadAngleDeg * 0.5f + spreadAngleDeg / (currentStats.MaxProjectileCount - 1) * i;
                }
                Vector2 modifiedDirection = Quaternion.Euler(0f, 0f, angle) * direction;

                obj.Init(
                    transform.position,
                    modifiedDirection,
                    currentStats.ProjectileSpeed,
                    currentStats.ProjectileLifetimeDuration,
                    currentStats.KnockbackPower);
                obj.GetPower(currentStats.Power);
            }

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
