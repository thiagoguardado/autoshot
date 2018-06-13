using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponBox : MonoBehaviour {


    [HideInInspector]
    public WeaponFactionSelector WeaponSelector;

    public List<WeaponFactionSelector> weaponsSelectors;

    public void Start()
    {

        WeaponSelector = weaponsSelectors[Random.Range(0, weaponsSelectors.Count)];

        if (WeaponSelector != null)
        {
            Instantiate(WeaponSelector, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        Debug.LogError("Weaponbox empty");

    }

}



