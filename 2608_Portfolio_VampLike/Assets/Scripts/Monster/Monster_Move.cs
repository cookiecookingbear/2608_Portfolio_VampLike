using System;
using UnityEngine;


public class Monster_Move : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;

    private float moveSpeed;
    private Vector2 playerPosition => player.transform.position;
    private Vector2 position => transform.parent.position;

    public event Action<bool> facingRight;
    private void Start()
    {
        //find Player, null check
        player = GameObject.FindAnyObjectByType<Player>();
        if (player == null)
        {
            Debug.LogError("Cannot find Player", this);
            enabled = false;
        }

        //find parent's Rigidbody2D, null check
        rb = GetComponentInParent<Rigidbody2D>();
        if(rb == null)
        {
            Debug.LogError("Cannot find parent's Rigidbody2D", this);
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        FixedUpdate_ChasePlayer();
    }

    public void Init(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void FixedUpdate_ChasePlayer()
    {
        Vector2 moveDirection = (playerPosition - position).normalized;

        bool isFacingRight = moveDirection.x >= 0 ? true : false;
        facingRight?.Invoke(isFacingRight);

        rb.linearVelocity = moveDirection * moveSpeed* Time.fixedDeltaTime;
    }
}
