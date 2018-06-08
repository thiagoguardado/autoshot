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
   
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider == IgnoreCollider)
        {
            return;
        }

        var target = col.gameObject.GetComponent<IWeaponTarget>();
        if(target != null)
        {
            HitInfo hit = new HitInfo();
            hit.StunDirection = _RigidBody.velocity;
            hit.StunForce = _Force;
            hit.StunTime = _Time;
            target.ApplyHit(hit);
        }

        Destroy(gameObject);
    }
}
