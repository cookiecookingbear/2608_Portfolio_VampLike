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
    [SerializeField] private Player_EnemyDetector detector;
    [SerializeField] private WeaponCatalogue catalogue;

    //private List<IWeapon> weapons = new List<IWeapon>();
    private Dictionary<WeaponData, IWeapon> weapons = new Dictionary<WeaponData, IWeapon>();

    //private int weaponCount = 0;

    #region Testing
    private void Update()
    {
        CheatKey();
    }
    private void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon(catalogue.WeaponInfoList[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectWeapon(catalogue.WeaponInfoList[1]);
        }

    }
    #endregion

    public void Init(float detectRadius)
    {
        detector.Init(detectRadius);
    }
    
    //추후에 레벨업UI에서 호출 가능성 있음
    private void SelectWeapon(WeaponInfo weaponInfo)
    {
        //if(weapons에 weaponInfo.WeaponData(키)가 없음) => 새 무기
        if(weapons.ContainsKey(weaponInfo.WeaponData) == false)
        {
            AddWeapon(weaponInfo);
            return;
        }
        //이미 있는 무기
        UpgrageWeapon(weaponInfo);
    }
        
    private void AddWeapon(WeaponInfo weaponInfo)
    {
        GameObject newWeaponObject = Instantiate(weaponInfo.WeaponPrefab, gameObject.transform);
        IWeapon newWeapon = newWeaponObject.GetComponent<IWeapon>();

        newWeapon.Init(detector, weaponInfo.WeaponData);
        newWeapon.StartFireCoroutine();
        weapons.Add(weaponInfo.WeaponData, newWeapon);
    }

    private void UpgrageWeapon(WeaponInfo weaponInfo)
    {
        weapons[weaponInfo.WeaponData].Upgrade();
    }

}
