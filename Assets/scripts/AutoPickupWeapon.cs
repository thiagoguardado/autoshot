using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPickupWeapon : MonoBehaviour {

    private Character characterScript;
    public WeaponFactionSelector weaponSelectorPrefab;

    void Start()
    {
        characterScript = GetComponent<Character>();

        WeaponFactionSelector weapon = Instantiate(weaponSelectorPrefab, transform.position, Quaternion.identity);
        characterScript.PickupWeapon(weapon);

    }
	

}
