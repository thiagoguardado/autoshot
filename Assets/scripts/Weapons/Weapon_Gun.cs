using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Gun : Weapon
{
    [Header("Gun Components")]
    public float GunDistance = 1.0f;
    public SpriteRenderer GunSprite;
    public GameObject GunShotEffectPrefab;
    public GameObject ShotPrefab = null;

    [Header("Gun Shot Configuration")]
    public float Spread = 1.0f;
    public int Shots = 1;
    public float ShotSpeed = 1.0f;
    public int MaxAmmo = 10;
    public HitInfo HitInfo;

    private Vector2 _ShotDirection;
    private Animator _GunAnimator;
    private int _ShotAnimation = Animator.StringToHash("shot");

    protected override void Awake()
    {
        _GunAnimator = GunSprite.GetComponent<Animator>();
    }

    public override void Shot()
    {
        for (int i = 0; i < Shots; i++)
        {
            ShootProjectile();
            _GunAnimator.Play(_ShotAnimation);
            Ammo--;
        }
    }

    protected override void Update()
    {
        base.Update();
        UpdateWeaponPosition();

    }

    void UpdateWeaponPosition()
    {
        if(_ClosestTarget != null)
        {
            GunSprite.enabled = true;
            _ShotDirection = _ClosestTarget.transform.position - transform.position;
            _ShotDirection.Normalize();
            GunSprite.transform.localPosition = _ShotDirection * GunDistance;
            GunSprite.transform.right = _ShotDirection;
            GunSprite.flipY = _ShotDirection.x < 0;
        }
        else
        {
            GunSprite.enabled = false;
        }
        
       
    }

    private void ShootProjectile()
    {
        _ShotDirection = _ClosestTarget.transform.position - transform.position;
        _ShotDirection.Normalize();

        float spreadAngle = Random.Range(-Spread, Spread);
        float sin = Mathf.Sin(spreadAngle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(spreadAngle * Mathf.Deg2Rad);

        Vector2 spreadedDirection = new Vector2();
        spreadedDirection.x = (cos * _ShotDirection.x) - (sin * _ShotDirection.y);
        spreadedDirection.y = (sin * _ShotDirection.x) + (cos * _ShotDirection.y);

        if (ShotPrefab != null)
        {
            GameObject bulletObject = Instantiate(ShotPrefab, transform.position, Quaternion.identity);
            var bullet = bulletObject.GetComponent<Bullet>();
            bullet.FriendFactions = friendFactions;
            bullet.HitInfo = HitInfo;
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), IgnoreCollider);
            bullet.IgnoreCollider = IgnoreCollider;


            bulletObject.transform.position = transform.position;
            var rigidBody = bulletObject.GetComponent<Rigidbody2D>();
            rigidBody.velocity = spreadedDirection * ShotSpeed;
        }

    }

}
