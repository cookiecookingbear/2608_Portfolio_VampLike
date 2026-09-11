using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour, IObjectPool
{
    private Stack<GameObject> inactives;
    private GameObject objectPrefab;
    private GameObject pool;
    [SerializeField] private int preWarmCount = 10;

    public void InitObjectPool(GameObject objectPrefab)
    {
        inactives = new Stack<GameObject>();
        this.objectPrefab = objectPrefab;
        pool = new GameObject($"{objectPrefab.name}_Pool");

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
        obj.transform.SetParent(null);
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
