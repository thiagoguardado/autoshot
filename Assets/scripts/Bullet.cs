using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PoolObject))]
public class Bullet : MonoBehaviour {   
    [HideInInspector]
    public HitInfo HitInfo;
    [HideInInspector]
    public List<CharacterFaction> FriendFactions = new List<CharacterFaction>();

    
    private Rigidbody2D _RigidBody;
    private Collider2D _Collider;
    private Collider2D _IgnoreCollider = null;
    private PoolObject _PoolObject;


   
    void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
        _Collider = GetComponent<Collider2D>();
        _PoolObject = GetComponent<PoolObject>();
        _PoolObject.OnActivate += OnActivate;
        _PoolObject.OnDeactivate += OnDeactivate;
    }
   
    void OnDestroy()
    {
        _PoolObject.OnActivate -= OnActivate;
        _PoolObject.OnDeactivate -= OnDeactivate;
    }

    public Collider2D IgnoreCollider
    {
        get
        {
            return _IgnoreCollider;
        }
        set
        {
            if (_IgnoreCollider != null)
            {
                Physics2D.IgnoreCollision(_Collider, _IgnoreCollider, false);
            }
            _IgnoreCollider = value;
            if (_IgnoreCollider != null)
            {
                Physics2D.IgnoreCollision(_Collider, _IgnoreCollider, true);
            }
        }
    }

    void OnActivate(PoolObject poolObject)
    {
        gameObject.SetActive(true);
    }
    void OnDeactivate(PoolObject poolObject)
    {
        IgnoreCollider = null;
        gameObject.SetActive(false);
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

    void Update()
    {
        
        transform.right = _RigidBody.velocity;
    }
}
