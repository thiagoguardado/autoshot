using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    
    public TargetFinder TargetFinder;
    public GameObject AimObject;

    public string WeaponName = "Weapon";
    public GameObject ShotPrefab;
    public int MaxAmmo = 10;
    
    public float ShotSpeed = 5.0f;
    public float ShotInterval = 0.4f;
    public int Shots = 1;
    public float Spread = 10.0f;

    [HideInInspector]
    public int CurrentAmmo = 10;

    [HideInInspector]
    public Character holder;

    private Character _Target;
    private float _ShotTimeout = 0;
    private Vector2 _ShotDirection = Vector2.right;

    public void Awake()
    {
        // targets = FindObjectsOfType<Character>();
        //  Target = targets[2];
        CurrentAmmo = MaxAmmo;
    }
    public virtual void Shot()
    {
        float spreadAngle = Random.Range(-Spread, Spread);
        float sin = Mathf.Sin(spreadAngle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(spreadAngle * Mathf.Deg2Rad);

        Vector2 spreadedDirection = new Vector2();
        spreadedDirection.x = (cos * _ShotDirection.x) - (sin * _ShotDirection.y);
        spreadedDirection.y = (sin * _ShotDirection.x) + (cos * _ShotDirection.y);

        GameObject bulletObject = Instantiate(ShotPrefab);
        var bullet = bulletObject.GetComponent<Bullet>();
        if(holder != null)
        {
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), holder.GetComponent<Collider2D>());
            bullet.IgnoreCollider = holder.GetComponent<Collider2D>();
            
        }
        
        bulletObject.transform.position = transform.position;
        var rigidBody = bulletObject.GetComponent<Rigidbody2D>();
        rigidBody.velocity = spreadedDirection * ShotSpeed;
        
    }
    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }

    public void Update()
    {
        if(holder == null)
        {
            return;
        }
        if(holder != null)
        {
            transform.position = holder.transform.position;
            TargetFinder.IgnoreTarget = holder;
        }
        _ShotTimeout -= Time.deltaTime;
        if(_ShotTimeout <= 0)
        {
            _ShotTimeout = 0;
            _Target = TargetFinder.ClosestTarget;
        }
        if(_Target != null)
        {
            AimObject.transform.position = _Target.transform.position;

            _ShotDirection = AimObject.transform.position - transform.position;
            _ShotDirection.Normalize();

            if (_ShotTimeout <= 0)
            {
                _ShotTimeout = ShotInterval;
                for(int i = 0; i < Shots; i++)
                {
                    Shot();
                }
                CurrentAmmo--;
                if (CurrentAmmo == 0)
                {
                    DestroyWeapon();
                }
            }
        }
    }
}
