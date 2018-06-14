using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyCanvas : MonoBehaviour {

    private Character _Character;

    public Slider EnergySlider;

    public void Init()
    {
        _Character = GetComponentInParent<Character>();
    }

    void Update()
    {
        UpdateEnergyGui();
    }

    void UpdateEnergyGui()
    {

        if(_Character._StateMachine._CurrentState is CharacterDeadState)
        {
            EnergySlider.gameObject.SetActive(false);
        }
        else
        {
            EnergySlider.gameObject.SetActive(true);
            EnergySlider.maxValue = _Character.MaxHealthPoints;
            EnergySlider.value = _Character.HealthPoints;
        }
    }
}
