using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour
{
    [SerializeField] private Monster_Stats stats;

    [SerializeField] private Monster_Move move;
    [SerializeField] private Monster_Combat combat;
    [SerializeField] private SpriteRenderer spriteRenderer;


    private void Start()
    {
        Start_Initialize(stats);
        combat.Damaged_knockback_event += move.GetKnockedback;

    }


    private void Start_Initialize(Monster_Stats stats)
    {
        move.Init(stats.MoveSpeed,stats.KnockbackStunTime);
        combat.Init(stats.Power, stats.Hp);
    }

    


}
