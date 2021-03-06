﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class NpcController : MonoBehaviour {
    private Character _Character;
    private Character _PlayerCharacter;
    public bool UseY = false;
    public bool UseJump = true;
    public void Awake()
    {
        PlayerInput pi = FindObjectOfType<PlayerInput>();
        _PlayerCharacter = pi.GetComponent<Character>();
        _Character = GetComponent<Character>();
    }

    public void Update()
    {
        _Character.InputWalkDirection.x = Mathf.Sign(_PlayerCharacter.transform.position.x - _Character.transform.position.x);
        if(UseY)
        {
            _Character.InputWalkDirection.y = Mathf.Sign(_PlayerCharacter.transform.position.y - _Character.transform.position.y);
        }
        if(UseJump)
        {
            if (_PlayerCharacter.transform.position.y > transform.position.y)
            {
                _Character.InputIsJumping = true;
            }
            else
            {
                _Character.InputIsJumping = false;
            }
        }
    }
}
