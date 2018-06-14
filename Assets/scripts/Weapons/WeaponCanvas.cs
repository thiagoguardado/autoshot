using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCanvas : MonoBehaviour {

    private Character _Character;

    public Image WeaponBackground;
    public Image WeaponIconImage;
    public Slider AmmoSlider;
    
  
    public void Init()
    {
        _Character = GetComponentInParent<Character>();
    }


	void Update () {
        UpdateWeaponGui();
	}

    void UpdateWeaponGui()
    {

        if (_Character.CurrentWeaponSelector == null)
        {
            WeaponBackground.gameObject.SetActive(false);
            AmmoSlider.gameObject.SetActive(false);
        }
        else
        {
            WeaponBackground.gameObject.SetActive(true);
            AmmoSlider.gameObject.SetActive(true);
            WeaponIconImage.sprite = _Character.CurrentWeaponSelector.currentInstantiatedWeapon.IconSprite;
            AmmoSlider.maxValue = _Character.CurrentWeaponSelector.currentInstantiatedWeapon.MaxAmmo;
            AmmoSlider.value = _Character.CurrentWeaponSelector.currentInstantiatedWeapon.Ammo;
        }
    }
}
