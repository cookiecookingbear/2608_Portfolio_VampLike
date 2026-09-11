using UnityEngine;

[CreateAssetMenu(fileName = "Monster_Stats", menuName = "SettingValues/Monster_Stats")]
public class Monster_Stats : ScriptableObject
{
    [SerializeField] float moveSpeed;
    [SerializeField] float hp;
    [SerializeField] float power;
    [SerializeField] float knockbackStunTime;

    public float MoveSpeed => moveSpeed;
    public float Hp => hp;
    public float Power => power;
    public float KnockbackStunTime => knockbackStunTime;

}
