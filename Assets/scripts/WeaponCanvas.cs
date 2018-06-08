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
        if (Character.CurrentWeapon == null)
        {
            WeaponNameGui.text = "";
            WeaponAmmoGui.text = "";
        }
        else
        {
            WeaponNameGui.text = Character.CurrentWeapon.Name;
            WeaponAmmoGui.text = Character.CurrentWeapon.Ammo.ToString() + "/" + Character.CurrentWeapon.MaxAmmo.ToString();
        }
    }
}
