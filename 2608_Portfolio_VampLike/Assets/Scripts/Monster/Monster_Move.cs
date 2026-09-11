using System;
using System.Collections;
using UnityEngine;


public class Monster_Move : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;

    private float moveSpeed;
    private float knockbackStunTime;
    private Vector2 playerPosition => player.transform.position;
    private Vector2 position => transform.parent.position;
    private bool isKnocked = false;

    public event Action<bool> facingRight;
    private Coroutine knockbackCoroutine;

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

    public void Init(float moveSpeed, float stunTime)
    {
        this.moveSpeed = moveSpeed;
        knockbackStunTime = stunTime;
    }

    private void FixedUpdate_ChasePlayer()
    {
        if (isKnocked == true) return;

        Vector2 moveDirection = GetMoveDirection();

        bool isFacingRight = moveDirection.x >= 0 ? true : false;
        facingRight?.Invoke(isFacingRight);

        rb.linearVelocity = moveDirection * moveSpeed* Time.fixedDeltaTime;
    }


    public void GetKnockedback(float knockbackPower)
    {
        if(knockbackCoroutine != null) StopCoroutine(knockbackCoroutine);

        knockbackCoroutine = StartCoroutine(Knockbacked(knockbackPower));
    }

    private IEnumerator Knockbacked(float knockbackPower)
    {
        isKnocked = true;

        rb.AddForce(-knockbackPower * GetMoveDirection(), ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackStunTime);

        isKnocked = false;
    }

    private Vector2 GetMoveDirection()
    {
        Vector2 moveDirection = (playerPosition - position).normalized;
        return moveDirection;
    }
}
