using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBoxFactory : MonoBehaviour
{
    public static WeaponBoxFactory Instance { get; private set; }
    public WeaponBox WeaponBoxPrefab;

    public void Awake()
    {
        Instance = this;
    }
    
    public WeaponBox CreateWeaponBox()
    {
        GameObject weaponBoxObject = Instantiate(WeaponBoxPrefab.gameObject);
        WeaponBox weaponBox = weaponBoxObject.GetComponent<WeaponBox>();

        return weaponBox;
    }
}