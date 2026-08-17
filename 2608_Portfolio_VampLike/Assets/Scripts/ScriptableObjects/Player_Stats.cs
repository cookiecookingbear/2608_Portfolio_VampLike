using UnityEngine;


[CreateAssetMenu(menuName = "SettingValues/Player")]
public class Player_Stats : ScriptableObject
{
    [SerializeField] float moveSpeed = 2.5f;
    [SerializeField] float hp = 100.0f;
    [SerializeField] float invulnerableTime = 0.5f;
    [SerializeField] float enemyDetectRadius = 10.0f;


    public float MoveSpeed => moveSpeed;
    public float HP => hp;
    public float InvulnerableTime => invulnerableTime;
    public float EnemyDetectRadius => enemyDetectRadius;
}
