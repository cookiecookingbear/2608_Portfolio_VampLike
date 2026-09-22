using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//햇빛은 별도의 투사체가 없으므로, 투사체의 역할까지 무기가 분담해야 한다.
[RequireComponent(typeof(CapsuleCollider2D))]
public class Sunshine : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponData weaponData;

    [SerializeField] private Player_EnemyDetector detector;

    private Coroutine fireCoroutine;

    private int weaponLevel = 0;
    private Weapon_Stats[] stats;
    private Weapon_Stats currentStats;

    private bool isUpgradeAvailable = true;

    [SerializeField] private Transform areaTransform;
    [SerializeField] private Transform beamTransform;


    [SerializeField] private ContactFilter2D filter;
    [SerializeField] private LayerMask enemyLayerMask;
    private CapsuleCollider2D capsuleCollider;
    List<Collider2D> enemies = new List<Collider2D>();

    private void Start()
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");
        filter.useTriggers = true;

        filter.SetLayerMask(enemyLayerMask);
    }

    public void Init(Player_EnemyDetector detector, WeaponData weaponData)
    {
        this.detector = detector;
        this.weaponData = weaponData;
        stats = weaponData.Stats;
        currentStats = stats[weaponLevel];
        weaponLevel++;
    }
    private void Awake()
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (capsuleCollider == null)
        {
            print("캡슐콜라이더 못찾음");
        }

        filter.useTriggers = true;
        filter.SetLayerMask(enemyLayerMask);
    }
    private IEnumerator Fire()
    {
        while (true)
        {
            enemies.Clear();
            int enemyCount = Physics2D.OverlapCapsule(transform.position, capsuleCollider.size * transform.lossyScale, capsuleCollider.direction, transform.eulerAngles.z, filter, enemies);

            for (int i = 0; i < enemyCount; i++)
            {
                GiveDamage(enemies[i]);
            }

            yield return new WaitForSeconds(currentStats.Cooldown);
        }
    }

    private void GiveDamage(Collider2D other)
    {
        IDamageable enemy = other.gameObject.GetComponent<IDamageable>();
        enemy.TakeDamage(currentStats.Power);

    }
    #region IWeapon
    public void StartFireCoroutine()
    {
        fireCoroutine = StartCoroutine("Fire");
    }

    public void StopFireCoroutine()
    {
        StopCoroutine(fireCoroutine);
    }

    public void Upgrade()
    {
        if (isUpgradeAvailable == false)
        {
            print("이미 무기가 최고단계임");
            return;
        }

        currentStats = stats[weaponLevel];
        applyAreaSize();
        weaponLevel++;
        print($"현재 무기 레벨: {weaponLevel}");

        if (weaponLevel == 6)
        {
            isUpgradeAvailable = false;
            print("무기가 최고단계에 도달함");
        }
    }

    private void applyAreaSize()
    {
        Vector3 scale = new Vector3(currentStats.AreaMagnification, currentStats.AreaMagnification, 1f);
        transform.localScale = scale;
        areaTransform.localScale = scale;
        beamTransform.localScale = scale;
    }

#endregion
}
