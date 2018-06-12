using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTarget : MonoBehaviour, IWeaponTarget
{
    public CharacterFaction GetCharacaterFaction()
    {
        return CharacterFaction.Player;
    }

    bool IWeaponTarget.ApplyHit(HitInfo hitInfo)
    {
        return true;
    }

    bool IWeaponTarget.IsActive()
    {
        return true;
    }
}
