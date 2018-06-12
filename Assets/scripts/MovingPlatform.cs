using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour {
    public float Speed = 1;
    public float Interval = 1;
    private float _Timeout;
    private Rigidbody2D _Rigidbody2D;

    void Awake()
    {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _Timeout = Interval;
    }
    
	void Update () {
        _Timeout -= Time.deltaTime;
        if(_Timeout <= 0)
        {
            Speed = -Speed;
            _Timeout = Interval;
        }

        transform.position += (Vector3) Vector2.up * Speed * Time.deltaTime;
    }
}
