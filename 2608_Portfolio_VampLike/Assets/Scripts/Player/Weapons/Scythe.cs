using System.Collections;
using UnityEngine;

public class Scythe : Weapon_Base
{
    private float orbitRadius = 2.0f;

    protected override void SetupObjectPool()
    {
        myPool.InitObjectPool(weaponData.ProjectilePrefab, true);
    }

    private void Update()
    {
        transform.Rotate(0, 0, -currentStats.ProjectileSpeed * Time.deltaTime);
    }
    
    protected override IEnumerator Fire()
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
}
