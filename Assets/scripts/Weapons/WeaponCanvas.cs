using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponCanvas : MonoBehaviour {
    private Character _Character;

    public Text WeaponNameGui;
    public Text WeaponAmmoGui;
    
    void Awake()
    {
        _Character = transform.parent.GetComponent<Character>();
    }

	void Update () {
        UpdateWeaponGui();
	}

    void UpdateWeaponGui()
    {
        if (_Character.CurrentWeaponSelector == null)
        {
            WeaponNameGui.text = "";
            WeaponAmmoGui.text = "";
        }
        else
        {
            WeaponNameGui.text = _Character.CurrentWeaponSelector.currentInstantiatedWeapon.Name;
            if (_Character.CurrentWeaponSelector.currentInstantiatedWeapon is Weapon_Gun)
            {
                WeaponAmmoGui.text = _Character.CurrentWeaponSelector.currentInstantiatedWeapon.Ammo.ToString() + "/" + ((Weapon_Gun)_Character.CurrentWeaponSelector.currentInstantiatedWeapon).MaxAmmo.ToString();
            }
            else
            {
                WeaponAmmoGui.text = "";
            }
            
        }
    }
}
