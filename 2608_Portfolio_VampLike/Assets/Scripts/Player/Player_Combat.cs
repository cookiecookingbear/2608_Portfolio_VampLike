using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CapsuleCollider2D))]
/// <summary>
/// 무기 사용
/// 받은 피해 계산
/// </summary>
public class Player_Combat : MonoBehaviour, IDamageable
{
    private float hp;
    private float invulnerableTime;
    private float invulnerableTimer = 0f;
    private bool damagable = true;

    public event Action GameOver;
    public event Action<bool> InvulnerableEvent;


    private void Update()
    {
        Update_CountInvulnerableTimer();
    }

    public void GetInitialStats(float hp, float invulnerableTime)
    {
        this.hp = hp;
        this.invulnerableTime = invulnerableTime;
    }
    public void TakeDamage(float damage)
    {
        if (damagable == false) return;

        hp -= damage;

        if(hp <= 0)
        {
            hp = 0;
            GameOver?.Invoke();
        }

        damagable = false;
        InvulnerableEvent?.Invoke(damagable);
        invulnerableTimer = invulnerableTime;

        print($"{damage}의 피해, hp:{hp}");
    }

    public void TakeDamage(float damage, float knockbackPower)
    {
        //미구현
    }

    private void Update_CountInvulnerableTimer()
    {
        if(invulnerableTimer <= 0
            && damagable == false)
        {
            invulnerableTimer = 0;
            damagable = true;
            InvulnerableEvent?.Invoke(damagable);
            return;
        }

        invulnerableTimer -= Time.deltaTime;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GUIStyle style = GUIStyle.none;
        style.normal.textColor = Color.black;
        style.alignment = TextAnchor.MiddleCenter;

        string str = $"HP : {hp}";

        Handles.Label((Vector2)transform.position + new Vector2(0, 0.5f), str, style);
    }
#endif
}
