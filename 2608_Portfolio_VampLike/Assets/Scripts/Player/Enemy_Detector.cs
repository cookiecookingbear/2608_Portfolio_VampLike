using System.Linq;
using UnityEngine;


/// <summary>
/// 최단거리의 적 감지
/// Linq공부해야함
/// </summary>
public class Enemy_Detector : MonoBehaviour
{
    [SerializeField] private float radius;

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        Transform nearest = colliders.OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).FirstOrDefault().transform;


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
