using System.Collections;
using UnityEngine;

//인터페이스 모음

/// <summary>
/// 타격을 받을 수 있는 물체가 상속
/// </summary>
public interface IDamageable
{
    void TakeDamage(float damage);
}


/// <summary>
/// 아이템을 드롭할 수 있는 물체가 상속
/// 잠정폐기
/// </summary>
//public interface IDrop
//{

//}


public interface IWeapon
{
    void StartFireCoroutine();
    void StopFireCoroutine();
    void Init(Player_EnemyDetector detector,WeaponData weaponData);
}