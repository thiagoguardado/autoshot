using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponClass
{
    Melee,
    Gun
}


public abstract class Weapon : MonoBehaviour
{
   
    // holder collider and target
    [HideInInspector] public Collider2D IgnoreCollider = null;
    [HideInInspector] public GameObject IgnoreTarget = null;
    public Sprite IconSprite = null;

    // holder enemies targets
    protected List<GameObject> _Targets = new List<GameObject>();
    protected GameObject _ClosestTarget = null;
    protected List<CharacterFaction> friendFactions = new List<CharacterFaction>();

    [Header("Configuration")]
    public WeaponClass weaponClass;
    public string Name = "No Name";
    public float Cooldown = 1.0f;
    public float Range = 1.0f;
    public int Ammo = 10;
    public int MaxAmmo { get; private set; }
    public bool InfiniteAmmo = false;
    public bool Shooting = true;
    public Character Holder = null;
    protected bool checkSight = true;

    private float _CurrentCooldown = 0;
    private Color _RangeGizmosColor = new Color(1, 1, 1, 0.4f);
    private Color _TargetGizmosColor = new Color(1, 0, 0, 0.4f);
    private Color _ClosestTargetGizmosColor = new Color(0, 1, 0, 0.4f);

    //created for safety. weapon_gun implemented awake
    protected virtual void Awake()
    {
        MaxAmmo = Ammo;
    }

    // Update is called once per frame
    protected virtual void Update()
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

            if (!friendFactions.Contains(target.GetCharacaterFaction()))
            {
                if (target.IsActive())
                {
                    return true;
                }
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

            if (checkSight)
            {
                if (CheckOnSight(target))
                {
                    _Targets.Add(target);
                }
            }
            else
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
