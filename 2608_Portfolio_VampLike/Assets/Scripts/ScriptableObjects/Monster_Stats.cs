using UnityEngine;

[CreateAssetMenu(fileName = "Monster_Stats", menuName = "SettingValues/Monster_Stats")]
public class Monster_Stats : ScriptableObject
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float hp;
    [SerializeField] public float power;

}
