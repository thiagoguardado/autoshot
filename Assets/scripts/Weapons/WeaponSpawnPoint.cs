using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnPoint: SpawnPoint {
    public List<WeaponClass> SpawnableWeponTypes;

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
