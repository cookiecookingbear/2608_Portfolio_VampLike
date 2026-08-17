using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        
    }

    public void Init(Vector3 dir, float speed) 
    {
        transform.up = dir;
        rb.linearVelocity = dir * speed;
    }

    private void FixedUpdate_Flying()
    {

    }

}
