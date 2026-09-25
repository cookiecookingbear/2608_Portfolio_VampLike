using System.Collections;
using UnityEngine;

public class Lightning : Weapon_Base
{
    protected override IEnumerator Fire()
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
}
