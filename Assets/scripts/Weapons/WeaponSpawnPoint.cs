using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnPoint: SpawnPoint {
    public List<WeaponClass> SpawnableWeponTypes;

    public WeaponClass ChooseWeaponClass()
    {
        if (SpawnableWeponTypes.Count <= 0)
        {
            return WeaponClass.None;
        }

        int randomId = Random.Range(0, SpawnableWeponTypes.Count);
        var weaponClass = SpawnableWeponTypes[randomId];
        return weaponClass;
    }
    void OnValidate()
    {
        SetColor(Color.red);

        if (SpawnableWeponTypes.Contains(WeaponClass.Gun) &&
            SpawnableWeponTypes.Contains(WeaponClass.Melee))
        {
            SetLabel("Any");
        }
        else if (SpawnableWeponTypes.Contains(WeaponClass.Gun))
        {
            SetLabel("Gun");
        }
        else if (SpawnableWeponTypes.Contains(WeaponClass.Melee))
        {
            SetLabel("Sword");
        }
        else
        {
            SetLabel("None");
        }
    }
}
