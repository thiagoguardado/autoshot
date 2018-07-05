using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveHudDisplay : MonoBehaviour {

    private Animator Animator;
    public Text waveText;
    public AudioClip newWaveSFX;

    private const string AnimatorStateBlink = "WaveTextBlink";
    private const string AnimatorBlinkTrigger = "blink";

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnNotifyWaveChanged += StartWave;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnNotifyWaveChanged -= StartWave;
    }


    private void StartWave(int waveCount, int waveTotal)
    {
        if (waveCount != waveTotal)
        {
            waveText.text = "WAVE " + waveCount.ToString();
        }
        else {
            waveText.text = "FINAL WAVE";
        }

        Animator.SetTrigger(AnimatorBlinkTrigger);

        AudioManager.Instance.PlaySFX(newWaveSFX);

    }
}
