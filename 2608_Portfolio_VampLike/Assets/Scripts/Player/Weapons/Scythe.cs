using System.Collections;
using UnityEngine;

public class Scythe : MonoBehaviour,IWeapon
{
    [SerializeField] private WeaponData weaponData;

    [SerializeField] private Player_EnemyDetector detector;
    [SerializeField] private ObjectPool myPool;

    private Coroutine fireCoroutine;

    private float orbitRadius = 2.0f;

    private int weaponLevel = 0;
    private Weapon_Stats[] stats;
    private Weapon_Stats currentStats;

    private bool isUpgradeAvailable = true;

    private void Update()
    {
        transform.Rotate(0, 0, -currentStats.ProjectileSpeed * Time.deltaTime);
    }
    public void Init(Player_EnemyDetector detector, WeaponData weaponData)
    {
        this.detector = detector;
        this.weaponData = weaponData;
        stats = weaponData.Stats;
        currentStats = stats[weaponLevel];
        weaponLevel++;

        myPool.InitObjectPool(weaponData.ProjectilePrefab,true);
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
            float maxProjectileCount = currentStats.MaxProjectileCount;
            float projectileSpeed = currentStats.ProjectileSpeed;
            float instantiateTimeInterval = (360f / maxProjectileCount) / projectileSpeed;

            for (int i = 0; i < maxProjectileCount; i++)
            {
                //각 투사체 instantiate에 필요한 정보
                float instantiatePositionX = Mathf.Cos(Mathf.Deg2Rad*(360f/ maxProjectileCount * i));
                float instantiatePositionY = Mathf.Sin(Mathf.Deg2Rad * (360f / maxProjectileCount * i));
                Vector2 instantitatePosition = new Vector2(instantiatePositionX, instantiatePositionY) * orbitRadius;

                //GameObject scytheProjectile = Instantiate(weaponData.ProjectilePrefab, transform,true);
                GameObject scytheProjectile = myPool.UseObject();

                Scythe_Projectile obj = scytheProjectile.GetComponent<Scythe_Projectile>();

                obj.Init(-projectileSpeed, instantitatePosition, currentStats.ProjectileLifetimeDuration);
                obj.GetPower(currentStats.Power);

                yield return new WaitForSeconds(instantiateTimeInterval);

            }
            yield return new WaitForSeconds(currentStats.Cooldown);
        }
    }
    public void Upgrade()
    {
        if (isUpgradeAvailable == false)
        {
            print("이미 무기가 최고단계임");
            return;
        }

        currentStats = stats[weaponLevel];
        StopCoroutine(fireCoroutine);
        fireCoroutine = StartCoroutine("Fire");
        weaponLevel++;
        print($"현재 무기 레벨: {weaponLevel}");

        if (weaponLevel == 6)
        {
            isUpgradeAvailable = false;
            print("무기가 최고단계에 도달함");
        }
    }
}
