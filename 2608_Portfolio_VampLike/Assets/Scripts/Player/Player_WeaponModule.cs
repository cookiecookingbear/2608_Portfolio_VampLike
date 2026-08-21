using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 최대 3개의 무기를 장착하는 모듈
/// 레벨업시 선택한 선택지의 정보를 받아 무기를 추가/업그레이드 
/// 각 무기의 공격력, 공격 속도, 공격 범위에 따라 공격하고
/// 최종적으로 캐릭터의 base ability를 곱해 몬스터에 적용.
/// </summary>
public class Player_WeaponModule : MonoBehaviour
{
    private List<IWeapon> weapons = new List<IWeapon>();

    private float detectRadius;
    private int weaponCount = 0;


    private void Awake()
    {
        
    }
    private void Start()
    {
        
    }

    public void Init(float detectRadius)
    {
        this.detectRadius = detectRadius;
        RegisterInitialWeapon();
    }

    /// <summary>
    /// 최초 게임 시작시 기본무기 등록
    /// </summary>
    private void RegisterInitialWeapon()
    {
        IWeapon weapon = GetComponentInChildren<IWeapon>();
        if(weapon == null)
        {
            Debug.LogError($"cannot find IWeapon", this);
            weapon = GetComponentInChildren<Gun>();
        }
        weapons.Add(weapon);
        weapon.Init(detectRadius);
        weapon.StartFireCoroutine();
        weaponCount++;
        //RegisterInFireCoroutines();
    }

    /// <summary>
    /// 무기 추가시 호출
    /// </summary>
    private void AddWeapon()
    {
        //컴포넌트 불러오는 방식 수정해야됨
        IWeapon weapon = GetComponentInChildren<IWeapon>();
        weapons.Add(weapon);
        weapon.Init(detectRadius);
        weaponCount++;
        //RegisterInFireCoroutines();
    }

}
