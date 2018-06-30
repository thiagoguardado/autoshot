using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class GhostController : MonoBehaviour {
    private Character _Character;
    private Character _PlayerCharacter;
    public void Awake()
    {
        PlayerInput pi = FindObjectOfType<PlayerInput>();
        if(pi != null)
        {
            _PlayerCharacter = pi.GetComponent<Character>();
        }
        
        _Character = GetComponent<Character>();
    }

    public void Update()
    {
        if(_PlayerCharacter == null)
        {
            return;
        }
        _Character.InputWalkDirection.x = Mathf.Sign(_PlayerCharacter.transform.position.x - _Character.transform.position.x);
        _Character.InputWalkDirection.y = Mathf.Sign(_PlayerCharacter.transform.position.y - _Character.transform.position.y);
    }
}
