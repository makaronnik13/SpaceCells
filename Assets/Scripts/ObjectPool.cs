using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;
    public static ObjectPool Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<ObjectPool>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private List<Pool> _pools;

    public GameObject GetObject(GameObject go)
    {
        Pool pool = _pools.FirstOrDefault(p => p.PoolObject == go);
        if (pool==null)
        {
            pool = new Pool(go);
            _pools.Add(pool);
        }

        if (pool.Objects.Count == 0)
        {
            return Instantiate(pool.PoolObject);
        }

        return pool.Take();
    }

    private void Start()
    {
        foreach (Pool p in _pools)
        {
            StartCoroutine(Instantiation(p));
        }
    }

    private IEnumerator Instantiation(Pool p)
    {
        while (true)
        {
            if (p.Objects.Count<p.Size)
            {
                GameObject go = Instantiate(p.PoolObject);
                go.SetActive(false);
                p.AddToPool(go);
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
    }
}
