﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
   
    // holder collider and target
    [HideInInspector] public Collider2D IgnoreCollider = null;
    [HideInInspector] public GameObject IgnoreTarget = null;

    // holder enemies targets
    protected List<GameObject> _Targets = new List<GameObject>();
    protected GameObject _ClosestTarget = null;
    private List<CharacterFaction> friendFactions = new List<CharacterFaction>();
    
    [Header("Configuration")]
    public string Name = "No Name";
    public float Cooldown = 1.0f;
    public float Range = 1.0f;
    public int Ammo = 10;
    public bool InfiniteAmmo = false;
    public bool Shooting = true;
    public Character Holder = null;

    private float _CurrentCooldown = 0;
    private Color _RangeGizmosColor = new Color(1, 1, 1, 0.4f);
    private Color _TargetGizmosColor = new Color(1, 0, 0, 0.4f);
    private Color _ClosestTargetGizmosColor = new Color(0, 1, 0, 0.4f);

   

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



    public void NewHolder(Character holder, Vector3 holderPosition, Collider2D holderCollider, GameObject holderTarget, List<CharacterFaction> holderFriendFactions)
    {
        Holder = holder;
        IgnoreCollider = holderCollider;
        IgnoreTarget = holderTarget;
        transform.position = holderPosition;
        friendFactions.Clear();
        friendFactions = holderFriendFactions;

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
                Shot();
                _CurrentCooldown = Cooldown;
            }
        }
    }

    public abstract void Shot();

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
    bool CheckIsTarget(GameObject targetObject)
    {
        var target = targetObject.GetComponent<IWeaponTarget>();
        if(target != null)
        {
            if(target.IsActive())
            {
                return true;
            }
        }

        return false;
    }

    void FindTargets()
    {
        _Targets.Clear();
       // int targetLayerMask = LayerMask.GetMask("Target");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Range);

        foreach (Collider2D collider in colliders)
        {
            var target = collider.gameObject;

            if (!CheckIsTarget(target))
            {
                continue;
            }

            if (target == IgnoreTarget)
            {
                continue;
            }


            if (friendFactions.Contains(target.GetComponent<IWeaponTarget>().GetCharacaterFaction()))
            {
                continue;
            }
            

            if (CheckOnSight(target))
            {
                _Targets.Add(target);
            }

        }
    }

    bool CheckOnSight(GameObject target)
    {
        int groundLayerMask = LayerMask.GetMask("Ground", "Platform");
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
