using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPickupWeapon : MonoBehaviour {
    public WeaponClass weaponType;
    private Character _Character;
    public WeaponFactionSelector weaponSelectorPrefab;

    void Awake()
    {
        _Character = GetComponent<Character>();
    }

    void Start()
    {
        PickupWeapon();
    }

    void PickupWeapon()
    {
        ObjectPool pool = null;
        switch (weaponType)
        {
            case WeaponClass.Gun:
                pool = SpawnableObjects.Instance.GunWeaponSelectorPool;
                break;
            case WeaponClass.Melee:
                pool = SpawnableObjects.Instance.MeleeWeaponSelectorPool;
                break;
        }

        if(pool == null)
        {
            return;
        }

        WeaponFactionSelector selector = pool.create().GetComponent<WeaponFactionSelector>();
        selector.transform.position = transform.position;
        selector.transform.parent = transform;
        _Character.PickupWeapon(selector);
    }
	
}
