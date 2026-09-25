using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//햇빛은 별도의 투사체가 없으므로, 투사체의 역할까지 무기가 분담해야 한다.
[RequireComponent(typeof(CapsuleCollider2D))]
public class Sunshine : Weapon_Base
{
    [SerializeField] private Transform areaTransform;
    [SerializeField] private Transform beamTransform;

    [SerializeField] private ContactFilter2D filter;
    [SerializeField] private LayerMask enemyLayerMask;
    private CapsuleCollider2D capsuleCollider;
    List<Collider2D> enemies = new List<Collider2D>();

    //햇빛은 오브젝트 풀이 없으니 풀링 호출을 하면 안된다.
    protected override void SetupObjectPool()
    {
        return;
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

    protected override IEnumerator Fire()
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
    
    protected override void ApplyStats()
    {
        base.ApplyStats();
        applyAreaSize();
    }

    private void applyAreaSize()
    {
        Vector3 scale = new Vector3(currentStats.AreaMagnification, currentStats.AreaMagnification, 1f);
        transform.localScale = scale;
    }

}
