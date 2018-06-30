using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour {
    public ObjectPool parentPool { get; set; }
    public Action<PoolObject> OnActivate;
    public Action<PoolObject> OnDeactivate;

    void Start()
    {
        if(parentPool == null)
        {
            Activate();
        }
    }
    public bool InUse { get; private set; }
    public void Activate()
    {
        if(OnActivate != null)
        {
            OnActivate(this);
        }
        InUse = true;
    }

    public void Deactivate()
    {
        if(OnDeactivate != null)
        {
            OnDeactivate(this);
        }
        InUse = false;
    }
}
