using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Monster_Combat : MonoBehaviour, IDamageable
{
    private CapsuleCollider2D hitBox;
    private CircleCollider2D collisionBox;

    private float power;
    private float hp;
    private int playerLayer;
    private int projecitleLayer;

    public Action Damaged_event;
    public Action<float> Damaged_knockback_event;
    public Action Dead_event;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        projecitleLayer = LayerMask.NameToLayer("Player_Projectiles");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GiveDamageToPlayer(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GiveDamageToPlayer(collision);
    }

    public void Init(float power, float hp)
    {
        this.power = power;
        this.hp = hp;
        print($"최대 체력: {hp}");
    }

    public void TakeDamage(float power)
    {
        hp -= power;

        InvokeAnimationEvent(hp);
        print($"남은 체력: {hp}");
    }

    public void TakeDamage(float power, float knockbackPower)
    {
        TakeDamage(power);

        Damaged_knockback_event?.Invoke(knockbackPower);
    }

    private void InvokeAnimationEvent(float hp)
    {
        if(hp <= 0)
        {
            Dead_event?.Invoke();
            return;
        }

        Damaged_event?.Invoke();
    }

    private void GiveDamageToPlayer(Collider2D collision)
    {
        if (collision.gameObject.layer != playerLayer) return;

        Player_Combat playerCombat = collision.gameObject.GetComponent<Player_Combat>();

        playerCombat.TakeDamage(power);
    }

}
