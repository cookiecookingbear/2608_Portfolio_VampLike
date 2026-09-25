using UnityEngine;
using System.Collections;

//포크_v1과 다르게 다수의 투사체가 관통하며 일정 시간동안 날아간다.
public class Fork_v2 : Weapon_Base
{
    [SerializeField, Tooltip("projectile spread angle as degree")] private float spreadAngleDeg = 50f;
    protected override IEnumerator Fire()
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

}
