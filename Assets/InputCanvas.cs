using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class InputCanvas : MonoBehaviour {
    private static InputCanvas _Instance = null;
    public static InputCanvas Instance
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
    public static void CreateInstance()
    {
        var prefab = Resources.Load<GameObject>("InputCanvas");
        _Instance = Instantiate(prefab).GetComponent<InputCanvas>();
    }

    public Action OnPause;
    public bool jumping { get; set; }
    public Vector2 direction = new Vector2();
    public UiAxis Axis;
    private Canvas _Canvas;

    void Awake()
    {
        if(_Instance != null)
        {
            Destroy(gameObject);
        }
        _Canvas = GetComponent<Canvas>();
        _Canvas.worldCamera = Camera.main;
        Axis = GetComponentInChildren<UiAxis>();
        _Instance = this;
    }
    
    void OnDestroy()
    {
        if(_Instance == this)
        {
            _Instance = null;
        }
    }
    public void Pause()
    {
        GameManager.Instance.Pause();
    }
}
