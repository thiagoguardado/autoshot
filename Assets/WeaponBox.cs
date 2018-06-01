using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour {
    public GameObject[] WeaponPrefabs;
    [HideInInspector]
    public GameObject WeaponPrefab;

    public void Awake()
    {
      
        WeaponPrefab = WeaponPrefabs[Random.Range(0, WeaponPrefabs.Length)];
    }
    public void DestroyBox()
    {
        Destroy(gameObject);
    }
}
