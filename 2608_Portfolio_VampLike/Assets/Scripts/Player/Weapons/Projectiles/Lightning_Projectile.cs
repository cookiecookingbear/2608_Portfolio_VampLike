using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class Lightning_Projectile : MonoBehaviour, IProjectile, IPoolable
{
    private float power;
    private int enemyLayer;
    [SerializeField] private LayerMask enemyLayerMask;
    private float lifespan;

    private CapsuleCollider2D capsuleCollider;

    [SerializeField] private Transform areaTransform;
    [SerializeField] private SpriteRenderer[] boltRenderers;
    [SerializeField] private Sprite[] boltSprites;

    [SerializeField] private ContactFilter2D filter;
    List<Collider2D> enemies = new List<Collider2D>();

    //IPoolable
    private IObjectPool affiliation;
    public GameObject GameObject => this.gameObject;

    private void Awake()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        enemyLayerMask = 1 << enemyLayer;
        capsuleCollider = GetComponentInChildren<CapsuleCollider2D>();
        if(capsuleCollider == null)
        {
            print("캡슐콜라이더 못찾음");
        }
        filter.useTriggers = true;
        filter.SetLayerMask(enemyLayerMask);
    }

    //충돌체가 서로 접근하여 충돌하는 식이 아니라서 triggerEnter 정상작동하지 않음
    //위의 설명이 버그의 원인인 줄 알았으나, 실상은 자식 오브젝트에 있는 콜라이더가 실제로 충돌은 검출하였으나, 이 코드가 붙은 부모오브젝트로 결과를 보내지 못하는 것이 문제였다.
    //이는 콜라이더를 가장 가까운 부모의 rigidbody에 귀속시킴으로써 해결할 수 있으나, 작동 방식이 의도와는 달라 이 방안은 폐기하고 다른 것으로 대체했다.
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    GiveDamage(other);
    //}

    private void OnDisable()
    {
        areaTransform.localScale = Vector3.one;
        for(int i = 0; i < boltRenderers.Length; i++)
        {
            boltRenderers[i].sprite = null;
        }
    }

    public void Init(Vector2 targetPosition, int boltCount, float areaSize, float lifespan)
    {
        transform.position = targetPosition;
        areaTransform.localScale = new Vector3(areaSize, areaSize,1f);
        this.lifespan = lifespan;
        for(int i = 0;  i < boltCount; i++)
        {
            boltRenderers[i].sprite = boltSprites[i];
        }

    }

    public void GetPower(float power)
    {
        this.power = power;
    }

    public void PoolInit(IObjectPool affiliation)
    {
        this.affiliation = affiliation;
    }

    public void Strike()
    {
        HitEnemyInArea();
    }

    private void GiveDamage(Collider2D other)
    {
        if (other.gameObject.layer != enemyLayer) return;
        IDamageable enemy = other.gameObject.GetComponent<IDamageable>();
        enemy.TakeDamage(power);

    }

    private IEnumerator WaitBeforeReturn(float effectDuration)
    {
        yield return new WaitForSeconds(effectDuration);
        
        affiliation.Return(this);
    }

    private void HitEnemyInArea()
    {
        enemies.Clear();
        int enemyCount = Physics2D.OverlapCapsule(transform.position, capsuleCollider.size * areaTransform.lossyScale, capsuleCollider.direction, areaTransform.eulerAngles.z, filter, enemies);

        for(int i = 0; i < enemyCount; i++)
        {
            GiveDamage(enemies[i]);
        }

        StartCoroutine(WaitBeforeReturn(lifespan));
    }
}
