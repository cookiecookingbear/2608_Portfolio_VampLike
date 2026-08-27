using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponData weaponData;

    [SerializeField] private Player_EnemyDetector detector;

    private Coroutine fireCoroutine;

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
            Vector2 direction = detector.DetectNearestMonsterDirection();

            GameObject bullet = Instantiate(weaponData.ProjectilePrefab, transform.position, Quaternion.Euler(direction));

            Bullet obj = bullet.GetComponent<Bullet>();
            obj.Init(direction, weaponData.Stats.ProjectileSpeed);

            yield return new WaitForSeconds(weaponData.Stats.Cooldown);
        }//while
    }

    
}
