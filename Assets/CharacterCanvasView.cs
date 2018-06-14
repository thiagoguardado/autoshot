using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCanvasView : MonoBehaviour {

    public WeaponCanvas weaponCanvas;
    public EnergyCanvas energyCanvas;

    internal void Init()
    {
        weaponCanvas.Init();
        energyCanvas.Init();
    }
}
