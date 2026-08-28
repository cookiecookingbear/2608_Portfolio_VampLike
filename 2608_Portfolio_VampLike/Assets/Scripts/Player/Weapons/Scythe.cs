using System.Collections;
using UnityEngine;

public class Scythe : MonoBehaviour,IWeapon
{
    [SerializeField] private WeaponData weaponData;

    [SerializeField] private Player_EnemyDetector detector;

    private Coroutine fireCoroutine;

    private float orbitRadius = 2.0f;

    private void Update()
    {
        transform.Rotate(0, 0, -weaponData.Stats.ProjectileSpeed * Time.deltaTime);
    }
    public void Init(Player_EnemyDetector detector, WeaponData weaponData)
    {
        this.detector = detector;
        this.weaponData = weaponData;
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
            for (int i = 0; i < weaponData.Stats.MaxProjectileCount; i++)
            {
                //각 투사체 instantiate에 필요한 정보
                float instantiatePositionX = Mathf.Cos(Mathf.Deg2Rad*(360f/ weaponData.Stats.MaxProjectileCount * i));
                float instantiatePositionY = Mathf.Sin(Mathf.Deg2Rad * (360f / weaponData.Stats.MaxProjectileCount * i));
                Vector2 instantitatePosition = new Vector2(instantiatePositionX, instantiatePositionY) * orbitRadius;
                float instantiateTimeInterval = (360f / weaponData.Stats.MaxProjectileCount) / weaponData.Stats.ProjectileSpeed;

                GameObject scytheProjectile = Instantiate(weaponData.ProjectilePrefab, transform,true);
                Scythe_Projectile obj = scytheProjectile.GetComponent<Scythe_Projectile>();

                obj.Init(-weaponData.Stats.ProjectileSpeed, instantitatePosition, weaponData.Stats.ProjectileLifetimeDuration);
                obj.GetPower(weaponData.Stats.Power);

                yield return new WaitForSeconds(instantiateTimeInterval);

            }
            yield return new WaitForSeconds(weaponData.Stats.Cooldown);
        }
    }
    public void Upgrade()
    {

    }
}
