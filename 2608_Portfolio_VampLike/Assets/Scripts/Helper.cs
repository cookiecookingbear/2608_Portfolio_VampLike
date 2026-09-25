using System.Linq;
using System.Collections.Generic;
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

    /// <summary>
    /// 배열의 원소 순서를 섞습니다. 0번부터 quantity-1번 원소까지 섞입니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">원소의 순서를 섞을 배열</param>
    /// <param name="quantity">필요한 섞인 원소의 수</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void ShuffleArray<T>(this T[] array, int quantity)
    {
        //exception
        if(quantity < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(quantity), quantity, "quantity는 0이상이어야 합니다.");
        }
        quantity = Mathf.Min(quantity, array.Length);

        //main logic
        for(int i = 0; i < quantity; i++)
        {
            int randomIndex = Random.Range(i, array.Length);

            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
