using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour {
    public Character Character;
    Character PlayerCharacter;
    public void Awake()
    {
        PlayerInput pi = FindObjectOfType<PlayerInput>();
        PlayerCharacter = pi.Character;
    }
    public void Update()
    {
        Character.walkDirection.x = Mathf.Sign(PlayerCharacter.transform.position.x - Character.transform.position.x);
        if(PlayerCharacter.transform.position.y > transform.position.y)
        {
            Character.input_jumping = true;
        }
        else
        {
            Character.input_jumping = false;
        }
    }
}
