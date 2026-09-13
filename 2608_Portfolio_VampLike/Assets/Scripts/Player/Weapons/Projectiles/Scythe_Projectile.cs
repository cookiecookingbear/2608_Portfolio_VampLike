using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Scythe_Projectile : MonoBehaviour,IProjectile, IPoolable
{
    private float angularSpeed;
    private float duration;
    private float power;
    private int enemyLayer;

    //IPoolable
    private IObjectPool affiliation;
    public GameObject GameObject => this.gameObject;

    private void Awake()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    private void Update()
    {
        Update_RotateThisProjectile();
        Update_CountDuration();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GiveDamage(collision);
    }

    private void OnDisable()
    {
        angularSpeed = 0;
        transform.localPosition = Vector3.zero;
        duration = 0;
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

    public void PoolInit(IObjectPool affiliation)
    {
        this.affiliation = affiliation;
    }

    private void GiveDamage(Collider2D other)
    {
        if (other.gameObject.layer != enemyLayer) return;

        IDamageable enemy = other.gameObject.GetComponent<IDamageable>();
        enemy.TakeDamage(power);
    }

    private void Update_RotateThisProjectile()
    {
        transform.Rotate(0, 0, angularSpeed * Time.deltaTime);
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
