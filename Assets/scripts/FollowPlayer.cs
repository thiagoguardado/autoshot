using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public GameObject Player;
    public float Speed = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        Vector3 pos = transform.position;
        pos = Vector2.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime);
        pos.z = transform.position.z;
        transform.position = pos;
	}
}
