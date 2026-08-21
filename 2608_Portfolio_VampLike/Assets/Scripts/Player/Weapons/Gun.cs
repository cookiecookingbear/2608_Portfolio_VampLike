using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Weapon_Stats stats;

    [SerializeField] private Player_EnemyDetector detector;

    private Coroutine fireCoroutine;

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

            GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(direction));

            Bullet obj = bullet.GetComponent<Bullet>();
            obj.Init(direction, stats.ProjectileSpeed);

            yield return new WaitForSeconds(stats.Cooldown);
        }//while
    }

    
}
