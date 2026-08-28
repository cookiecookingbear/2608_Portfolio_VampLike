using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Bullet : MonoBehaviour, IProjectile
{
    private Rigidbody2D rb;
    private float power;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") == false) return;

        Destroy(gameObject);
    }
    public void Init(Vector3 dir, float speed)
    {
        transform.up = dir;
        rb.linearVelocity = dir * speed;
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
