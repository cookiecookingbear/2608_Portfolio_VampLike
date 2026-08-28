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
/// 무기들이 공통적으로 가지는 인터페이스
/// Init : 무기가 가져야 할 디텍터와 자신의 데이터를 주입받음
/// Start/StopFireCoroutine : 플레이어의 웨폰모듈에서 무기를 통제할 수 있게끔 함
/// Upgrade : 웨폰모듈에서 업그레이드 명령을 받아 업그레이드를 처리할 수 있게 함
/// </summary>
public interface IWeapon
{
    void StartFireCoroutine();
    void StopFireCoroutine();
    void Init(Player_EnemyDetector detector,WeaponData weaponData);
    void Upgrade();
}

/// <summary>
/// 무기의 투사체들이 공통적으로 가지는 인터페이스
/// 각 투사체들이 자신의 공격력을 가질 수 있게 하고
/// 투사체가 몬스터에 닿았을 때 몬스터에서 데미지 정보를 받을 수 있게 함
/// </summary>
public interface IProjectile
{
    void GetPower(float power);
    //float GiveDamage();
}