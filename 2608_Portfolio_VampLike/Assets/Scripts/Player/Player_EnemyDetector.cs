using System.Linq;
using UnityEngine;

public class Player_EnemyDetector : MonoBehaviour
{
    private float detectRadius;
    [SerializeField] private LayerMask enemyLayerMask;

    public void Init(float detectRadius)
    {
        this.detectRadius = detectRadius;
    }

    public Vector2 DetectNearestMonsterDirection()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectRadius, enemyLayerMask);
        
        if (colliders.Length == 0)
        {
            return Vector2.left;
        }

        Vector2 nearestMonsterPosition = colliders.OrderBy(c => Vector2.Distance(transform.position, c.transform.position)).FirstOrDefault().transform.position;

        Vector2 direction = nearestMonsterPosition - (Vector2)transform.position;

        return direction.normalized;
    }
}
