using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Fork_Projectile : MonoBehaviour,IProjectile, IPoolable
{
    private Rigidbody2D rb;
    private float power;
    private int enemyLayer;
    private float duration;
    private float knockbackPower;

    //IPoolable
    private IObjectPool affiliation;
    public GameObject GameObject => this.gameObject;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    private void Update()
    {
        Update_CountDuration();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GiveDamage(other);
    }

    private void OnDisable()
    {
        transform.up = Vector2.up;
        rb.linearVelocity = Vector2.zero;
        knockbackPower = 0;
    }

    public void Init(Vector2 startingPosition, Vector3 dir, float speed, float duration,float knockbackPower)
    {
        transform.position = startingPosition;
        transform.up = dir;
        rb.linearVelocity = dir * speed;
        this.duration = duration;
        this.knockbackPower = knockbackPower;
    }

    public void GetPower(float power)
    {
        this.power = power;
    }

    public void PoolInit(IObjectPool affiliation)
    {
        this.affiliation = affiliation;
    }

    private void GiveDamage(Collider2D other)
    {
        if (other.gameObject.layer != enemyLayer) return;

        IDamageable enemy = other.gameObject.GetComponent<IDamageable>();
        enemy.TakeDamage(power, knockbackPower);


        //Destroy(gameObject);

        affiliation.Return(this);
    }
    private void Update_CountDuration()
    {
        if (duration <= 0)
        {
            duration = 0;
            //Destroy(gameObject);
            affiliation.Return(this);
        }

        duration -= Time.deltaTime;
    }
}
