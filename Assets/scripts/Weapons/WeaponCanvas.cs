using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponCanvas : MonoBehaviour {
    Character _Character;

    public Image WeaponIconGui;
    public Text WeaponAmmoGui;
    public Image Background;

    
    void Awake()
    {
        _Character = transform.parent.GetComponent<Character>();
    }
	void Update () {
        UpdateWeaponGui();
	}

    void UpdateWeaponGui()
    {
        if (_Character.CurrentWeapon == null)
        {
            Background.gameObject.SetActive(false);
        }
        else
        {
            Background.gameObject.SetActive(true);
            WeaponIconGui.sprite = _Character.CurrentWeapon.IconSprite;
            //if (_Character.CurrentWeapon is Weapon_Gun)
         //   {
                WeaponAmmoGui.text = _Character.CurrentWeapon.Ammo.ToString();
          //  }
           // else
           // {
            //    WeaponAmmoGui.text = "";
           // }
            
        }
    }
}
