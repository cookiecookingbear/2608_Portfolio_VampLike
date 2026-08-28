using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Bullet : MonoBehaviour, IProjectile
{
    private Rigidbody2D rb;
    private float power;
    private int enemyLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        GiveDamage(other);
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

    private void GiveDamage(Collider2D other)
    {
        if (other.gameObject.layer != enemyLayer) return;

        IDamageable enemy = other.gameObject.GetComponent<IDamageable>();
        enemy.TakeDamage(power);

        Destroy(gameObject);
    }

}
