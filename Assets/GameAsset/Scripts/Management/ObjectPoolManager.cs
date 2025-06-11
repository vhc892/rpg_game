using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new();

    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
        }
        Queue<GameObject> objectPool = poolDictionary[prefab];
        GameObject obj;

        if (objectPool.Count > 0 && objectPool.Peek() != null)
        {
            obj = objectPool.Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, position, rotation);
        }

        if (obj.TryGetComponent<IPoolable>(out var poolable))
        {
            poolable.OnTakenFromPool();
        }
        return obj;
    }

    public void ReturnToPool(GameObject prefab, GameObject obj)
    {
        if (obj.TryGetComponent<IPoolable>(out var poolable))
        {
            poolable.OnReturnedToPool();
        }

        obj.SetActive(false);

        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
        }
        poolDictionary[prefab].Enqueue(obj);
    }

}
