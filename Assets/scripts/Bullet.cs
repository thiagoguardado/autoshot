using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    [HideInInspector]
    public Collider2D IgnoreCollider;
    Rigidbody2D _RigidBody;
    private float _Force = 10;
    private float _Time = 0.1f;
    void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnCollisionEnter2D(Collision2D col)
    {
        
        if(col.collider == IgnoreCollider)
        {
            return;
        }

        if(col.collider.tag == "Character")
        {
            var character = col.collider.GetComponent<Character>();
            if(character != null)
            {
                character.Stun(_RigidBody.velocity.normalized, _Force, _Time);
            }
        }

        Destroy(gameObject);
    }
}
