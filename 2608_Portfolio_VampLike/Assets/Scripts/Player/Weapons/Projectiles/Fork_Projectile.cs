using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Fork_Projectile : MonoBehaviour,IProjectile
{
    private Rigidbody2D rb;
    private float power;
    private int enemyLayer;
    private float duration;
    private float knockbackPower;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    private void Update()
    {
        if (duration <= 0)
        {
            duration = 0;
            Destroy(gameObject);
        }

        duration -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GiveDamage(other);
    }

    public void Init(Vector3 dir, float speed, float duration,float knockbackPower)
    {
        transform.up = dir;
        rb.linearVelocity = dir * speed;
        this.duration = duration;
        this.knockbackPower = knockbackPower;
    }

    public void GetPower(float power)
    {
        this.power = power;
    }

    private void GiveDamage(Collider2D other)
    {
        if (other.gameObject.layer != enemyLayer) return;

        IDamageable enemy = other.gameObject.GetComponent<IDamageable>();
        enemy.TakeDamage(power, knockbackPower);


        Destroy(gameObject);
    }
}
