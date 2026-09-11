using System.Collections;
using UnityEngine;

//인터페이스 모음

/// <summary>
/// 타격을 받을 수 있는 물체가 상속
/// </summary>
public interface IDamageable
{
    void TakeDamage(float damage);
    void TakeDamage(float damage,float knockbackPower);
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

/// <summary>
/// 오브젝트 풀링을 적용할 오브젝트에 적용하는 인터페이스
/// </summary>
public interface IPoolable
{
    void PoolInit(IObjectPool affiliation);
    GameObject GameObject { get; }
}


/// <summary>
/// 오브젝트 풀링을 관리할 풀에 적용하는 인터페이스
/// </summary>
public interface IObjectPool
{
    /// <summary>
    /// 오브젝트를 다시 풀에 반납할 때 사용
    /// SetActive(false)처리는 각 IPoolable에서 처리 하고 Return하는것으로 함.
    /// </summary>
    void Return(IPoolable obj);
}