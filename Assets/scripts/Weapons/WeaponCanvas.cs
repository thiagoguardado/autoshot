using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponCanvas : MonoBehaviour {
    private Character _Character;

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

        if (_Character.CurrentWeaponSelector == null)
        {
            Background.gameObject.SetActive(false);
        }
        else
        {
            Background.gameObject.SetActive(true);
            WeaponIconGui.sprite = _Character.CurrentWeaponSelector.currentInstantiatedWeapon.IconSprite;
            WeaponAmmoGui.text = _Character.CurrentWeaponSelector.currentInstantiatedWeapon.Ammo.ToString();
        }
    }
}
