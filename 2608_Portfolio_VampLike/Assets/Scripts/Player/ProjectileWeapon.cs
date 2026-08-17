using System.Linq;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject projectilePrefab;

    private float detectRadius;
    private int enemyLayer;

    private void Awake()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    public void Init(float detectRadius)
    {
        this.detectRadius = detectRadius;
    }

    private Vector2 DetectNearestMonster()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,detectRadius, enemyLayer);

        Vector2 nearestMonsterPosition = colliders.OrderBy(c => Vector2.Distance(transform.parent.position, c.transform.position)).FirstOrDefault().transform.position;

        return nearestMonsterPosition;
    }

    public void Fire()
    {

    }

}
