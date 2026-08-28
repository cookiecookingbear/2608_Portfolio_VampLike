using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Scythe_Projectile : MonoBehaviour,IProjectile
{
    private float angularSpeed;
    private float duration;
    private float power;

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") == false) return;


    }

    public void Init(float zAngularSpeed, Vector2 instantiatePosition,float duration)
    {
        angularSpeed = zAngularSpeed;
        transform.localPosition = instantiatePosition;
        this.duration = duration;
    }
    
    public void GetPower(float power)
    {
        this.power = power;
    }

    public float GiveDamage()
    {
        return power;
    }

}
