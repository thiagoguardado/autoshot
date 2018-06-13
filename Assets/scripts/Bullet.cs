using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    [HideInInspector]
    public Collider2D IgnoreCollider;
    [HideInInspector]
    public HitInfo HitInfo;
    [HideInInspector]
    public List<CharacterFaction> FriendFactions = new List<CharacterFaction>();

    Rigidbody2D _RigidBody;
   
    void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
    }
   
    void OnHit(GameObject hitTarget)
    {
        var target = hitTarget.GetComponent<IWeaponTarget>();
        if (target != null)
        {
            if(!FriendFactions.Contains(target.GetCharacaterFaction()))
            {
                HitInfo.StunDirection = _RigidBody.velocity.normalized;
                bool hitSuccessful = target.ApplyHit(HitInfo);
                if (hitSuccessful)
                {
                    Destroy(gameObject);
                }
            }
            
        }
        else
        {
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
