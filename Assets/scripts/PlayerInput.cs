using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInput : MonoBehaviour {
    public Character Character;
    public bool touch_jump = false;
    public bool touch_left = false;
    public bool touch_right = false;

    public void pressRight()
    {
        touch_right = true;
    }
    public void releaseRight()
    {
        touch_right = false;
    }
    public void presLeft()
    {
        touch_left = true;
    }
    public void releaseLeft()
    {
        touch_left = false;
    }
    public void pressJump()
    {
        touch_jump = true;
    }
    public void releaseJump()
    {
        touch_jump = false;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Character.walkDirection.x = Input.GetAxis("Horizontal");
        if(touch_left || touch_right)
        {
            if(touch_left)
            {
                Character.walkDirection.x = -1;
            }
            else if(touch_right)
            {
                Character.walkDirection.x = 1;
            }        }
        Character.input_jumping = Input.GetButton("Jump");
        if(touch_jump == true)
        {
            Character.input_jumping = true;
        }
	}
}
