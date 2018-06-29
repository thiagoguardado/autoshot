using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("AudioSources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Common sounds")]
    public AudioClip buttonPressAudio;

    private static void CreateInstance()
    {
        var prefab = Resources.Load<GameObject>(_ResourceName);
        var instance = Instantiate(prefab);
        instance.name = "_" + _ResourceName;
        instance.transform.SetAsFirstSibling();
        _Instance = instance.GetComponent<AudioManager>();
    }

    private void Awake()
    {
        if (_Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButtonPressAudio()
    {
        PlaySFX(buttonPressAudio);
    }

}
