using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponCanvas : MonoBehaviour {
    public Character Character;

    public Text WeaponNameGui;
    public Text WeaponAmmoGui;
    

	void Update () {
        UpdateWeaponGui();
	}

    void UpdateWeaponGui()
    {
        if (Character.CurrentWeaponSelector == null)
        {
            WeaponNameGui.text = "";
            WeaponAmmoGui.text = "";
        }
        else
        {
            WeaponNameGui.text = Character.CurrentWeaponSelector.currentInstantiatedWeapon.Name;
            if (Character.CurrentWeaponSelector.currentInstantiatedWeapon is Weapon_Gun)
            {
                WeaponAmmoGui.text = Character.CurrentWeaponSelector.currentInstantiatedWeapon.Ammo.ToString() + "/" + ((Weapon_Gun)Character.CurrentWeaponSelector.currentInstantiatedWeapon).MaxAmmo.ToString();
            }
            else
            {
                WeaponAmmoGui.text = "";
            }
            
        }
    }
}
