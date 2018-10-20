using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pool
{
    public GameObject PoolObject;
    public int Size;

    [NonSerialized]
    public List<GameObject> _objects;
    public List<GameObject> Objects
    {
        get
        {
            if (_objects == null)
            {
                _objects = new List<GameObject>();
            }
            return _objects;
        }
    }

    public Pool(GameObject poolObject)
    {
        PoolObject = poolObject;
    }

    public void AddToPool(GameObject go)
    {
        Objects.Add(go);
    }

    public GameObject Take()
    {
        GameObject go = Objects[0];
        Objects.Remove(go);
        go.SetActive(true);
        return go;
    }
}