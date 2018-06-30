using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnPoint: SpawnPoint {
    public WeaponTypes WeaponType = WeaponTypes.Any;
    public float SpawnTime = 3;
    private float _Timeout = 0;
    private WeaponFactionSelector _SpawnedSelector;

    public enum WeaponTypes
    {
        /*
            GetAnyWeaponType() uses this order
        */
        Melee,
        Gun,
        Any
    }


    void OnWeaponPickedUp()
    {
        _SpawnedSelector = null;
        _Timeout = SpawnTime;
    }
    void Update()
    {
        if(_SpawnedSelector == null)
        {
            _Timeout -= Time.deltaTime;
            if(_Timeout <= 0)
            {
                SpawnSelector();
            }
        }
    }

    void SpawnSelector()
    {
        var pool = GetPool(WeaponType);
        _SpawnedSelector = Spawn(pool).GetComponent<WeaponFactionSelector>();
        _SpawnedSelector.UseGravity = false;
        _SpawnedSelector.OnWeaponPickedUp += OnWeaponPickedUp;
        _Timeout = SpawnTime;
    }

    void OnValidate()
    {
        SetColor(Color.red);
        SetLabel(WeaponType.ToString());
    }

    ObjectPool GetPool(WeaponTypes weaponType)
    {
        if (weaponType == WeaponTypes.Any)
        {
            weaponType = GetAnyWeaponType();
        }

        switch (weaponType)
        {
            case WeaponTypes.Melee:
                return SpawnableObjects.Instance.MeleeWeaponSelectorPool;
            case WeaponTypes.Gun:
                return SpawnableObjects.Instance.GunWeaponSelectorPool;
            default:
                break;
        }

        return null;
    }

    WeaponTypes GetAnyWeaponType()
    {
        int i = Random.Range(0, 2);
        return (WeaponTypes) i;
    }
}
