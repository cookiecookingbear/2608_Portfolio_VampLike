using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Player_Stats stats;

    [SerializeField] private Player_Move playerMove;
    [SerializeField] private Player_Combat playerCombat;
    [SerializeField] private Player_WeaponModule weaponModule;
    [SerializeField] private Player_GearModule gearModule;

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public event Action GameOverEvent;

    private void Start()
    {
        Init_Stat();

        playerCombat.GameOver += GameOver;
        playerCombat.InvulnerableEvent += ShowInvernerableState;

        //플레이어 애니메이션 설정
        playerMove.Moving += moveInputX => anim.SetFloat("moveSpeed", Mathf.Abs(moveInputX));

        //플레이어 스프라이트 보는 방향 설정
        playerMove.Moving += moveInputX =>
        {
            if (Mathf.Abs(moveInputX) < 0.01) return;

            if (moveInputX < 0)
            {
                spriteRenderer.flipX = true;
                return;
            }
            spriteRenderer.flipX = false;
        };
    }

    private void Init_Stat()
    {
        playerMove.GetMoveSpeed(stats.MoveSpeed);
        playerCombat.GetInitialStats(stats.HP, stats.InvulnerableTime);
        weaponModule.Init(stats.EnemyDetectRadius);
    }

    private void GameOver()
    {
        GameOverEvent?.Invoke();
    }
    
    private void ShowInvernerableState(bool damagable)
    {
        if(damagable == false)
        {
            spriteRenderer.color = Color.red;
            return;
        }

        spriteRenderer.color = Color.white;
    }
}
