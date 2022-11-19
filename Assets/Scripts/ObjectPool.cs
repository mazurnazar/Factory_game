using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private uint initPoolSize;
    [SerializeField] private Product objectToPool;
    // store the pooled objects in a collection
    private Stack<Product> stack;
    private void Awake()
    {
        SetupPool();
    }
    // creates the pool (invoke when the lag is not noticeable)
    private void SetupPool()
    {
        stack = new Stack<Product>();
        Product instance = null;
        for (int i = 0; i < initPoolSize; i++)
        {
            instance = Instantiate(objectToPool);
            instance.gameObject.name = objectToPool.ProductName;
            instance.transform.parent = transform;
            instance.Pool = this;
            instance.gameObject.SetActive(false);
            stack.Push(instance);
        }
    }
    // getting last object from pool or creating new one if pool is empty
    public Product GetPooledObject()
    {
        // if the pool is not large enough, instantiate a new PooledObjects
        if (stack.Count == 0)
        {
            Product newInstance = Instantiate(objectToPool);
            newInstance.Pool = this;
            return newInstance;
        }
        // otherwise, just grab the next one from the list
        Product nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }
    // return objects to pool
    public void ReturnToPool(Product pooledObject)
    {
        stack.Push(pooledObject);
        pooledObject.transform.parent = transform;
        pooledObject.gameObject.SetActive(false);
    }
}

