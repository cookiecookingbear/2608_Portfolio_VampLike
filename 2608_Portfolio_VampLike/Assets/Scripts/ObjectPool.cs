using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour, IObjectPool
{
    private Stack<GameObject> inactives;
    private GameObject objectPrefab;
    private GameObject pool;
    [SerializeField] private int preWarmCount = 10;

    private bool isProjectilesRotate;

    /// <summary>
    /// 각 무기에 붙은 ObjectPool 컴포넌트를 초기화.
    /// 무기의 Init()최하단에서 호출하도록 작성
    /// </summary>
    /// <param name="objectPrefab">이 무기의 투사체 프리팹</param>
    /// <param name="isProjectilesRotate">투사체가 캐릭터를 중심으로 회전하면 true</param>
    public void InitObjectPool(GameObject objectPrefab, bool isProjectilesRotate = false)
    {
        inactives = new Stack<GameObject>();
        this.objectPrefab = objectPrefab;
        pool = new GameObject($"{objectPrefab.name}_Pool");

        this.isProjectilesRotate = isProjectilesRotate;
        if(isProjectilesRotate == true)
        {
            pool.transform.SetParent(transform);
            pool.transform.localPosition = Vector2.zero;
        }

        for(int i = 0; i < preWarmCount; i++)
        {
            InstantiateObjects();
        }
    }


    private void InstantiateObjects()
    {
        GameObject obj = Instantiate(objectPrefab, pool.transform);
        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable.PoolInit(this);
        
        obj.SetActive(false);
        inactives.Push(obj);
    }

    public GameObject UseObject()
    {
        if (inactives.Count <= 0)
        {
            InstantiateObjects();
        }
        GameObject obj = inactives.Pop();

        if(isProjectilesRotate == false)
        {
            obj.transform.SetParent(null);
        }
        obj.SetActive(true);

        return obj;
    }

    public void Return(IPoolable obj)
    {
        if (obj.GameObject.activeSelf == false) return;

        obj.GameObject.SetActive(false);
        obj.GameObject.transform.SetParent(pool.transform);
        inactives.Push(obj.GameObject);
    }
}
