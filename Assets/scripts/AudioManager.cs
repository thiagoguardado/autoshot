using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    private static AudioManager _Instance;
    public static AudioManager Instance
    {
        get
        {
            if(_Instance == null)
            {
                CreateInstance();
            }
            return _Instance;
        }
    }

    public const string _ResourceName = "AudioManager";

    [Header("AudioMixer")]
    public AudioMixer audioMixer;
    public AudioMixerSnapshot noBGMSnapshot;
    public AudioMixerSnapshot reducedBGMVolumeSNapshot;
    public AudioMixerSnapshot normalVolumeSnapshot;
    public AudioMixerSnapshot highPitchSnapshot;
    public AudioMixerSnapshot lowPitchSnapshot;
    private bool isPlayingHighPitch = false;

    [Header("AudioSources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip gameClip;
    public AudioClip menuClip;

    [Header("Common sounds")]
    public AudioClip buttonPressAudio;

    private const float snapshotsTransitionDuration = 0.5f;

    private static void CreateInstance()
    {
        var prefab = Resources.Load<GameObject>(_ResourceName);
        var instance = Instantiate(prefab);
        instance.name = "_" + _ResourceName;
        instance.transform.SetAsFirstSibling();
        _Instance = instance.GetComponent<AudioManager>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnStartSceneChange += DisableBGM;
        GameManager.Instance.OnFinishSceneChange += StartBGM;
        GameManager.Instance.OnScenePaused += ReduceBGM;
        GameManager.Instance.OnSceneUnPaused += UnPauseBGM;
        GameManager.Instance.OnNotifyWaveChanged += ChangePitch;
        GameManager.Instance.OnNotifyLevelFinished += LevelFinished;
    }



    private void OnDisable()
    {
        GameManager.Instance.OnStartSceneChange -= DisableBGM;
        GameManager.Instance.OnFinishSceneChange -= StartBGM;
        GameManager.Instance.OnScenePaused -= ReduceBGM;
        GameManager.Instance.OnSceneUnPaused -= UnPauseBGM;
        GameManager.Instance.OnNotifyWaveChanged -= ChangePitch;
        GameManager.Instance.OnNotifyLevelFinished -= LevelFinished;
    }


    private void Awake()
    {
        if (_Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        else
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        StartBGM(SceneManager.GetActiveScene().name);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButtonPressAudio()
    {
        PlaySFX(buttonPressAudio);
    }

    public void ChangeBGMAudio(AudioClip newBGM)
    {
        bgmSource.clip = newBGM;
        bgmSource.loop = true;
        bgmSource.Play();
        isPlayingHighPitch = false;
    }


    public void DisableBGM()
    {
        noBGMSnapshot.TransitionTo(snapshotsTransitionDuration);
    }

    private void RenableBGM()
    {
        normalVolumeSnapshot.TransitionTo(snapshotsTransitionDuration);
    }

    private void ReduceBGM()
    {
        reducedBGMVolumeSNapshot.TransitionTo(snapshotsTransitionDuration);
    }

    private void UnPauseBGM()
    {
        if (isPlayingHighPitch)
        {
            highPitchSnapshot.TransitionTo(snapshotsTransitionDuration);
        }
        else {
            RenableBGM();
        }

    }

    private void ChangePitch(int currentWave, int totalWaves)
    {
        if (currentWave == totalWaves)
        {
            isPlayingHighPitch = true;
            highPitchSnapshot.TransitionTo(snapshotsTransitionDuration);
        }
    }

    public void StartBGM(string sceneName)
    {
        if (sceneName == "Menu")
        {
            ChangeBGMAudio(menuClip);
        }
        else {
            ChangeBGMAudio(gameClip);
        }

        RenableBGM();
    }

    private void LevelFinished(bool successful)
    {
        if (successful)
        {
            normalVolumeSnapshot.TransitionTo(snapshotsTransitionDuration);
        }
        else {
            lowPitchSnapshot.TransitionTo(snapshotsTransitionDuration);
        }
    }


}
