using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffects : MonoBehaviour {

    private static VisualEffects _Instance;
    public static VisualEffects Instance
    {
        get
        {
            if (_Instance == null)
            {
                CreateInstance();
            }
            return _Instance;
        }
    }

    public const string _ResourceName = "VisualEffects";


    private static void CreateInstance()
    {
        var prefab = Resources.Load<GameObject>(_ResourceName);
        var instance = Instantiate(prefab);
        instance.name = "_" + _ResourceName;
        instance.transform.SetAsFirstSibling();
        _Instance = instance.GetComponent<VisualEffects>();
    }


    public GameObject spawnEffect;
    public GameObject weaponSpawnEffect;
    public GameObject hurtEffect;
    public GameObject dieEffect;



    public void PlayHurtEffect(Transform parent)
    {
        InstantiateEffect(hurtEffect, parent);
    }

    public void PlayDieEffect(Transform parent)
    {
        InstantiateEffect(dieEffect, parent);
    }

    public void PlaySpawnEffect(Transform parent)
    {
        InstantiateEffect(spawnEffect, parent);
    }

    public void PlayWeaponSpawnEffect(Transform parent)
    {
        InstantiateEffect(weaponSpawnEffect, parent);
    }

    private void InstantiateEffect(GameObject effect, Transform parent)
    {

        Instantiate(effect, parent.position, effect.transform.rotation);

    }

}
