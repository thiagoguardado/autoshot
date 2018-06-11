using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

    public Collider2D IgnoreCollider;
    Rigidbody2D _RigidBody;
    private float _Force = 16;
    private float _Time = 0.2f;

    void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
    }
   
    void OnHit(GameObject hitTarget)
    {
        var target = hitTarget.GetComponent<IWeaponTarget>();
        if (target != null)
        {
            HitInfo hit = new HitInfo(_RigidBody.velocity.normalized, _Force,_Time);
            if(target.ApplyHit(hit))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("hitting " + hitTarget.name);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider == IgnoreCollider)
        {
            return;
        }

        OnHit(col.collider.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col == IgnoreCollider)
        {
            return;
        }

        OnHit(col.gameObject);
    }
}
