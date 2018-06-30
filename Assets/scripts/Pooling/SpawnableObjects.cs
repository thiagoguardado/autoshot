using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObjects : MonoBehaviour {
    private static SpawnableObjects _Instance = null;
    public static SpawnableObjects Instance
    {
        get
        {
            if(_Instance == null)
            {
                CreateInstance();
            }

            return _Instance;
        }
    }
    
    public ObjectPool SlimePool;
    public ObjectPool SlimeWithGunPool;
    public ObjectPool SlimeWithMeleePool;
    public ObjectPool GhostPool;
    public ObjectPool GhostWithGunPool;
    public ObjectPool GhostWithMeleePool;
    public ObjectPool MeleeWeaponSelectorPool;
    public ObjectPool GunWeaponSelectorPool;
    public ObjectPool GunBulletPool;

    ObjectPool[] _AllPools;
    
    private static void CreateInstance()
    {
        var prefab = Resources.Load<GameObject>("SpawnableObjects").GetComponent<SpawnableObjects>();
        _Instance = Instantiate(prefab);
        _Instance.name = "_SpawnableObjects";
        _Instance.transform.SetAsFirstSibling();

    }

    void Awake()
    {
        if(_Instance != null)
        {
            Destroy(gameObject);
        }
        _Instance = this;

       
        Init();
    }
    void OnDestroy()
    {
        _Instance = null;
    }

    void Init()
    {
        GameObject poolParent = new GameObject();
        poolParent.name = "_PooledObjects";
        poolParent.transform.SetAsFirstSibling();
        poolParent.transform.parent = transform;

        _AllPools = new ObjectPool[]
       {
            SlimePool,
            SlimeWithGunPool,
            SlimeWithMeleePool,
            GhostPool,
            GhostWithGunPool,
            GhostWithMeleePool,
            MeleeWeaponSelectorPool,
            GunWeaponSelectorPool,
            GunBulletPool
       };

        foreach (var pool in _AllPools)
        {
            pool.parent = poolParent.transform;
            pool.init();
        }
    }
}
