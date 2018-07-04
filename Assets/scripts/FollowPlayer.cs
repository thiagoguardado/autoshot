using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public GameObject Player;
    public float Speed = 1;

    public BoxCollider2D CameraBounds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        Vector3 pos = transform.position;
        pos = Vector2.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime);

        pos.x = Mathf.Clamp(pos.x, CameraBounds.bounds.min.x, CameraBounds.bounds.max.x);
        pos.y = Mathf.Clamp(pos.y, CameraBounds.bounds.min.y, CameraBounds.bounds.max.y);
        pos.z = transform.position.z;
        transform.position = pos;
	}
}
