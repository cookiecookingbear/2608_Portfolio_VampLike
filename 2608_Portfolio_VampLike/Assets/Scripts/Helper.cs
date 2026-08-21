using System.Linq;
using UnityEngine;

public static class Helper
{
    /// <summary>
    /// 감지 반경 내 감지 대상 레이어에 있는 가장 가까운 대상의 Vector2좌표를 반환
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="detectRadius"> 감지 반경</param>
    /// <param name="layer"> 감지 대상 레이어</param>
    /// <returns></returns>
    public static Vector2 DetectNearestMonsterDirection(this Transform transform, float detectRadius, int layer)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectRadius, layer);

        if (colliders.Length == 0)
        {
            return Vector2.left;
        }

        Vector2 nearestMonsterPosition = colliders.OrderBy(c => Vector2.Distance(transform.position, c.transform.position)).FirstOrDefault().transform.position;

        Vector2 direction = nearestMonsterPosition - (Vector2)transform.position;

        return direction;
    }
}
