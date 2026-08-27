using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Scythe_Projectile : MonoBehaviour
{
    private float angularSpeed;
    private float duration;

    private void Update()
    {
        transform.Rotate(0, 0, angularSpeed * Time.deltaTime);

        if (duration <= 0)
        {
            Destroy(gameObject);
            return;
        }
        
        duration -= Time.deltaTime;
    }
    /// <summary>
    /// 투사체 자체의 회전(궤도회전 아님)속도 받아서 자체적으로 회전하게끔
    /// </summary>
    /// <param name="zAngularSpeed"></param>
    /// <param name="orbitRadius"></param>
    public void Init(float zAngularSpeed, Vector2 instantiatePosition,float duration)
    {
        angularSpeed = zAngularSpeed;
        transform.localPosition = instantiatePosition;
        this.duration = duration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") == false) return;

        
    }
}
