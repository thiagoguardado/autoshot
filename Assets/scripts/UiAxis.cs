using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UiAxis : MonoBehaviour {

    public Image LeftImage;
    public Image RightImage;

    private RectTransform _RectTransform;

    private int _TouchCount;
    private Rect _Rect;
    private Vector2 _InputPos;
    private Vector2 _InputRelativePos;
    private bool _Pressed;
    private bool _PressedRight;
    private bool _PressedLeft;

    public Color InactiveColor = new Color(1, 1, 1, 0.6f);
    public Color ActiveColor = new Color(1, 1, 1, 0.6f);
    
    public Vector2 direction
    {
        get
        {
            if(_PressedLeft && !_PressedRight)
            {
                return Vector2.left;
            }
            if(_PressedRight && !_PressedLeft)
            {
                return Vector2.right;
            }

            return Vector2.zero;
        }
    }

	void Awake () {
        _RectTransform = GetComponent<RectTransform>();
	}

    void ApplyTouch(Vector2 position)
    {
        _InputPos = (Vector2) Camera.main.ScreenToWorldPoint(position);
        _InputRelativePos = transform.InverseTransformPoint(_InputPos);
        _Rect = _RectTransform.rect;
        if (_Rect.Contains(_InputRelativePos))
        {
            _Pressed = true;
            if(_InputRelativePos.x > 0)
            {
                _PressedRight = true;
            }
            else
            {
                _PressedLeft = true;
            }
        }

    }

	void Update () {
        _Pressed = false;
        _PressedRight = false;
        _PressedLeft = false;

        _TouchCount = Input.touchCount;
        for (int i = 0; i < _TouchCount; i++)
        {
            Vector2 position = Input.GetTouch(i).position;
            ApplyTouch(position);
        }

        if(Input.GetMouseButton(0))
        {
            Vector2 position = Input.mousePosition;
            ApplyTouch(position);
        }

        LeftImage.color = _PressedLeft ? ActiveColor : InactiveColor;
        RightImage.color = _PressedRight ? ActiveColor : InactiveColor;
    }
}
