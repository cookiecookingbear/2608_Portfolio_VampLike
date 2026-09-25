using System.Collections;
using UnityEngine;

public class Gun : Weapon_Base
{
    protected override IEnumerator Fire()
    {
        while (true)
        {
            Vector2 direction = detector.DetectNearestMonsterDirection();

            GameObject bullet = myPool.UseObject();

            Bullet obj = bullet.GetComponent<Bullet>();
            obj.Init(transform.position, direction, currentStats.ProjectileSpeed, currentStats.ProjectileLifetimeDuration);
            obj.GetPower(currentStats.Power);

            yield return new WaitForSeconds(currentStats.Cooldown);
        }//while
    }

}
