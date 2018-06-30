using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool {

    [SerializeField]
    public PoolObject Prefab;
    [SerializeField]
    public int poolInitialSize = 10;

    private Queue<PoolObject> _Pool;
    
    //Used for debug only
    private int currentPoolSize = 0;

    [HideInInspector]
    public Transform parent = null;

    public void init()
    {
        _Pool = new Queue<PoolObject>();
        for(int i = 0; i < poolInitialSize; i++)
        {
            instantiate();
        }
        currentPoolSize = _Pool.Count;
    }

    public PoolObject create()
    {
        var instance = getNext();
        currentPoolSize = _Pool.Count;
        instance.Activate();
        return instance;
    }
   

    void OnDeactivate(PoolObject poolObject)
    {
        poolObject.transform.parent = parent;
        if (!_Pool.Contains(poolObject))
        {
            _Pool.Enqueue(poolObject);
            currentPoolSize = _Pool.Count;
        }
    }

    void OnActivate(PoolObject poolObject)
    {
        poolObject.transform.parent = null;
    }

    void instantiate()
    {
        var instance = GameObject.Instantiate(Prefab.gameObject).GetComponent<PoolObject>();
        instance.parentPool = this;
        
        instance.OnDeactivate += OnDeactivate;
        instance.OnActivate += OnActivate;

        instance.Deactivate();
    }

    PoolObject getNext()
    {
        if(_Pool.Count > 0)
        {
            instantiate();
        }

        return _Pool.Dequeue();
    }

    
}
