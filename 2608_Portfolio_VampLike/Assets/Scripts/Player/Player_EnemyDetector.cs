using System.Linq;
using UnityEngine;
using System.Collections.Generic;

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
        //적 콜라이더 감지
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
        colliders.ShuffleArray<Collider2D>(enemyCount);
        Collider2D[] enemyColliders = colliders.Take(enemyCount).ToArray();
        
        
        
        if(enemyColliders.Length >= enemyCount)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                enemyPositions[i] = enemyColliders[i].transform.position;
            }
        }
        else //enemyCollidres.Length < enemyCount
        {
            for(int i = 0; i < enemyColliders.Length; i++)
            {
                enemyPositions[i] = enemyColliders[i].transform.position;
            }
            for(int i = enemyColliders.Length; i< enemyCount; i++)
            {
                float randomX = Random.Range(-9f, 9f);
                float randomY = Random.Range(-5f, 5f);
                enemyPositions[i] = new Vector2(randomX, randomY);
            }
        }

            return enemyPositions;
    }

/* Test
    #region Testing

    private void Update()
    {
        Testing();
    }
    private void Testing()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Vector2[] test = DetectAndReturnRandomEnemyPositions(1);
            foreach(Vector2 position in test)
            {
                print(position);
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Vector2[] test = DetectAndReturnRandomEnemyPositions(2);
            foreach (Vector2 position in test)
            {
                print(position);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Vector2[] test = DetectAndReturnRandomEnemyPositions(3);
            foreach (Vector2 position in test)
            {
                print(position);
            }
        }
    }
    #endregion*/
}
