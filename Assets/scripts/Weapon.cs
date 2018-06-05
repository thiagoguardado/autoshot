using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   

    [HideInInspector]
    public Collider2D IgnoreCollider = null;

    [HideInInspector]
    public WeaponTarget IgnoreTarget = null;

    private List<WeaponTarget> _Targets = new List<WeaponTarget>();
    private WeaponTarget _ClosestTarget = null;
    
    [Header("Configuration")]
    public string Name = "No Name";
    public GameObject ShotPrefab = null;
    public float Range = 1.0f;
    public float Cooldown = 1.0f;
    public float Spread = 1.0f;
    public int Shots = 1;
    public float ShotSpeed = 1.0f;
    public int MaxAmmo = 10;

    [Header("Debug")]
    public int Ammo = 10;
    public bool Shooting = true;
    public bool InfiniteAmmo = false;

    private float _CurrentCooldown = 0;
    private Color _RangeGizmosColor = new Color(1, 1, 1, 0.4f);
    private Color _TargetGizmosColor = new Color(1, 0, 0, 0.4f);
    private Color _ClosestTargetGizmosColor = new Color(0, 1, 0, 0.4f);
    public Character Holder = null;

    public virtual void Shot()
    {
        Vector2 shotDirection = _ClosestTarget.transform.position - transform.position;
        float spreadAngle = Random.Range(-Spread, Spread);
        float sin = Mathf.Sin(spreadAngle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(spreadAngle * Mathf.Deg2Rad);

        Vector2 spreadedDirection = new Vector2();
        spreadedDirection.x = (cos * shotDirection.x) - (sin * shotDirection.y);
        spreadedDirection.y = (sin * shotDirection.x) + (cos * shotDirection.y);

        if (ShotPrefab != null)
        {
            GameObject bulletObject = Instantiate(ShotPrefab);
            var bullet = bulletObject.GetComponent<Bullet>();

            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), IgnoreCollider);
            bullet.IgnoreCollider = IgnoreCollider;


            bulletObject.transform.position = transform.position;
            var rigidBody = bulletObject.GetComponent<Rigidbody2D>();
            rigidBody.velocity = spreadedDirection * ShotSpeed;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Holder != null)
        {
            transform.position = Holder.transform.position;
        }
        FindClosestTarget();
        ShootingRoutine();
    }

    void ShootingRoutine()
    {
        if (_CurrentCooldown >= 0)
        {
            _CurrentCooldown -= Time.deltaTime;
        }
        else if (Shooting && (Ammo > 0 || InfiniteAmmo))
        {
            if (_ClosestTarget != null)
            {
                for (int i = 0; i < Shots; i++)
                {
                    Shot();
                    _CurrentCooldown = Cooldown;
                    Ammo--;
                }
            }
        }
    }

    void FindClosestTarget()
    {
        FindTargets();
        _ClosestTarget = null;
        float closest = float.MaxValue;
        foreach (var target in _Targets)
        {
            var sqrDistance = Vector2.SqrMagnitude(target.transform.position - transform.position);
            if (sqrDistance < closest)
            {
                closest = sqrDistance;
                _ClosestTarget = target;
            }
        }
    }
    void FindTargets()
    {
        _Targets.Clear();
       // int targetLayerMask = LayerMask.GetMask("Target");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Range);

        foreach (Collider2D collider in colliders)
        {

            WeaponTarget target = collider.GetComponent<WeaponTarget>();
            if (target == null)
            {
                continue;
            }

            if (target == IgnoreTarget)
            {
                continue;
            }

            if (CheckOnSight(target))
            {
                _Targets.Add(target);
            }

        }
    }

    bool CheckOnSight(WeaponTarget target)
    {
        int groundLayerMask = LayerMask.GetMask("Ground");
        Vector2 distanceVector = target.transform.position - transform.position;
        var hit = Physics2D.Raycast(transform.position, distanceVector, distanceVector.magnitude, groundLayerMask);

        if (hit.collider == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = _RangeGizmosColor;
        Gizmos.DrawSphere(transform.position, Range);

        foreach (var target in _Targets)
        {
            if (target == _ClosestTarget)
            {
                Gizmos.color = _ClosestTargetGizmosColor;
            }
            else
            {
                Gizmos.color = _TargetGizmosColor;
            }
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }

    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }
}
