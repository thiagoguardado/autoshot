using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInput : MonoBehaviour {
    public Character Character;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Character.walkDirection.x = Input.GetAxis("Horizontal");
        Character.input_jumping = Input.GetButton("Jump");
	}
}
