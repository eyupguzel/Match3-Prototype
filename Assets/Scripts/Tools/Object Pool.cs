using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : Singleton<ObjectPool<T>> where T : MonoBehaviour  
{
    [SerializeField] protected T prefab;

    private List<T> pooledObject;
    private int amount;
    private bool isReady;

    public void PoolObject(int amount = 0)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException("Amount to pool must be non-negative.");

        this.amount = amount;
        pooledObject = new List<T>(amount);

        GameObject newObject;

        for(int i = 0;i != amount; i++)
        {
            newObject = Instantiate(prefab.gameObject, transform);
            newObject.SetActive(false);
            pooledObject.Add(newObject.GetComponent<T>());
        }
        isReady = true;
    }

    public T GetPooledObject()
    {
        if (!isReady)
            PoolObject(1);

        for(int i = 0; i != amount; i++)
            if (!pooledObject[i].isActiveAndEnabled)
                return pooledObject[i];

        GameObject newobject = Instantiate(prefab.gameObject, transform);
        newobject.SetActive(true);
        pooledObject.Add(newobject.GetComponent<T>());
        ++amount;
        return newobject.GetComponent<T>();
    }
    public void ReturnObjectToPool(T toBeReturned)
    {
        if (toBeReturned == null)
            return;

        if (!isReady)
        {
            PoolObject();
            pooledObject.Add(toBeReturned);
        }

        toBeReturned.gameObject.SetActive(false);
    }
}
