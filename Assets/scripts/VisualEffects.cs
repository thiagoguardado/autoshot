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
    public GameObject slimeHurtEffect;
    public GameObject ghostHurtEffect;
    
    public GameObject dieEffect;
    public GameObject ghostDieEffect;
    public GameObject slimeDieEffect;

    public void PlayHurtEffect(Transform parent, CharacterTypes characterType)
    {
        switch (characterType)
        {
            case CharacterTypes.Player:
                InstantiateEffect(hurtEffect, parent);
                break;
            case CharacterTypes.Slime:
                InstantiateEffect(slimeHurtEffect, parent);
                break;
            case CharacterTypes.Ghost:
                InstantiateEffect(ghostHurtEffect, parent);
                break;
            default:
                InstantiateEffect(hurtEffect, parent);
                break;
        }
    }

    public void PlayDieEffect(Transform parent, CharacterTypes characterType)
    {
        switch (characterType)
        {
            case CharacterTypes.Player:
                InstantiateEffect(dieEffect, parent);
                break;
            case CharacterTypes.Slime:
                InstantiateEffect(slimeDieEffect, parent);
                break;
            case CharacterTypes.Ghost:
                InstantiateEffect(ghostDieEffect, parent);
                break;
            default:
                InstantiateEffect(dieEffect, parent);
                break;
        }
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
