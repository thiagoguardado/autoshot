using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    bool _jumping = false;
    private Color EnabledColor = new Color(.5f, .5f, .5f, 0.6f);
    private Color DisabledColor = new Color(1, 1, 1, 0.6f);
    public bool jumping
    {
        get
        {
            return _jumping;
        }
        set
        {
            _jumping = value;
            if(JumpButtonImage != null)
            {
                JumpButtonImage.color = _jumping ? EnabledColor : DisabledColor;
            }
        }
    }
    public Image JumpButtonImage;
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
