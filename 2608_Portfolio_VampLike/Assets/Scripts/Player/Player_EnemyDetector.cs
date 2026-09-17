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

    public Vector2[] DetectAndReturnRandomEnemyPositions(int enemyCount)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectRadius, enemyLayerMask);

        Vector2[] enemyPositions = new Vector2[enemyCount];

        //감지 0 처리
        if (colliders.Length == 0)
        {
            for (int i  = 0;  i < enemyCount; i++)
            {
                float randomX = Random.Range(-9f, 9f);
                float randomY = Random.Range(-5f, 5f);
                enemyPositions[i] = new Vector2(randomX, randomY);
            }
            return enemyPositions;
        }

        //Linq

        //enemyCount>감지수 처리
        
        return enemyPositions;
    }
}
