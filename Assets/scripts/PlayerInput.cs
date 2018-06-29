using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerInput : MonoBehaviour {
    private Character _Character;
    bool _TouchInputJump = false;
    bool _TouchInputLeft = false;
    bool _TouchInputRight = false;

    public void PressRight()
    {
        _TouchInputRight = true;
    }
    public void ReleaseRight()
    {
        _TouchInputRight = false;
    }
    public void PressLeft()
    {
        _TouchInputLeft = true;
    }
    public void ReleaseLeft()
    {
        _TouchInputLeft = false;
    }
    public void PressJump()
    {
        _TouchInputJump = true;
    }

    public void ReleaseJump()
    {
        _TouchInputJump = false;
    }

    void Awake()
    {
        _Character = GetComponent<Character>();
    }

    void Update () {

        if (LevelManager.Instance.inGame)
        {
            _Character.InputWalkDirection.x = Input.GetAxis("Horizontal");
            _Character.InputIsJumping = Input.GetButton("Jump");


            if (_TouchInputLeft)
            {
                _Character.InputWalkDirection.x = -1;
            }
            else if (_TouchInputRight)
            {
                _Character.InputWalkDirection.x = 1;
            }

            if (_TouchInputJump == true)
            {
                _Character.InputIsJumping = true;
            }
        }
	}
}
