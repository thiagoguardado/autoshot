using UnityEngine;
using FiniteStateMachines;
using System;
using System.Collections.Generic;

public enum CharacterFaction
{
    Player,
    Enemies
}

[RequireComponent(typeof(Collider2D))]
public class Character : MonoBehaviour, IWeaponTarget
{

    [SerializeField] private CharacterFaction characterFaction;
    public List<CharacterFaction> friendFactions;
    public Animator Animator;
    public SpriteRenderer SpriteRenderer;

    [HideInInspector]
    public HurtTrigger HurtTrigger;

    [HideInInspector]
    public WeaponCanvas WeaponCanvas;

    public float WalkingMaxSpeed = 3;



    public float WalkingAcceleration = 3;
    public float SpringedAcceleration = 3;
    public float ReactivityPercent = 0.5f;
    public float JumpForceMax = 6;
    public float JumpForceMin = 4;
    public bool UseGravity = true;
    public bool LockYMovement = true;
    public bool ShouldCheckCollisions = true;
    public bool CanPickupWeapon = true;

    public AudioClip deathAudio;
    public AudioClip hurtAudio;

    public GameObject _Ground = null;
    public GameObject _RightWall = null;
    public GameObject _LeftWall = null;
    public GameObject _TopWall = null;

    public int MaxHealthPoints = 10;
    public int HealthPoints { get; private set; }
    public Vector2 _MovementDirection;

    public bool IgnoreBullets = false;
    int _GroundLayerMask;
    int _PlatformLayerMask;
    bool _Moving = false;

    [HideInInspector] public WeaponFactionSelector CurrentWeaponSelector { get; private set; }

    [HideInInspector]
    public Vector2 InputWalkDirection;

    [HideInInspector]
    public bool InputIsJumping;

    [HideInInspector]
    public HitInfo HitInfo { get; private set; }


    public Collider2D _Collider { get; private set; }

    public FiniteStateMachine<Character> _StateMachine { get; private set; }
    CharacterJumping _JumpingState = new CharacterJumping();
    CharacterIdle _IdleState = new CharacterIdle();
    CharacterStunned _StunnedState = new CharacterStunned();
    CharacterDeadState _DeadState = new CharacterDeadState();
    CharacterSpringed _SpringedState = new CharacterSpringed();

    public enum EventTriggers
    {
        EndState,
        Stunned,
        Springed
    }


    void Awake()
    {

        _GroundLayerMask = LayerMask.GetMask("Ground");
        _PlatformLayerMask = LayerMask.GetMask("Platform", "Ground");

        _Collider = GetComponent<Collider2D>();

        _IdleState.AddCondition(CheckIsDead, _DeadState);
        _IdleState.AddCondition(() => InputIsJumping, _JumpingState);
        _IdleState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);
        _IdleState.AddTrigger((int)EventTriggers.Springed, _SpringedState);

        _JumpingState.AddCondition(CheckIsDead, _DeadState);
        _JumpingState.AddCondition(CheckIsGrounded, _IdleState);
        _JumpingState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);
        _JumpingState.AddTrigger((int)EventTriggers.Springed, _SpringedState);

        _SpringedState.AddCondition(CheckIsDead, _DeadState);
        _SpringedState.AddCondition(CheckIsGrounded, _IdleState);
        _SpringedState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);
        _SpringedState.AddTrigger((int)EventTriggers.Springed, _SpringedState);

        _StunnedState.AddTrigger((int)EventTriggers.EndState, _IdleState);
        _StunnedState.AddTransition((int)EventTriggers.EndState, CheckIsDead, _DeadState);
        _StunnedState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);


        _StateMachine = new FiniteStateMachine<Character>(this);
        _StateMachine.SetState(_IdleState);

        WeaponCanvas = GetComponentInChildren<WeaponCanvas>();
        HurtTrigger = GetComponentInChildren<HurtTrigger>();
    }

    void Start()
    {
        HealthPoints = MaxHealthPoints;
    }

    public void StateEnded()
    {
        _StateMachine.TriggerEvent((int)EventTriggers.EndState);
    }

    public void PickupWeapon(WeaponFactionSelector _weaponSelector)
    {
        if(!CanPickupWeapon)
        {
            return;
        }
        FactionWeapon weaponToInstantiate = null;

        for (int i = 0; i < _weaponSelector.weapons.Count; i++)
        {
            if (_weaponSelector.weapons[i].faction == characterFaction)
            {
                weaponToInstantiate = _weaponSelector.weapons[i];
                break;
            }
        }

        if (weaponToInstantiate != null)
        {

            // drop current selector and pick new
            if (CurrentWeaponSelector != null)
            {
                //force destroy
                CurrentWeaponSelector.currentInstantiatedWeapon.Ammo = 0;
                CurrentWeaponSelector.CharacterDropSelector(transform.position);
            }
            CurrentWeaponSelector = _weaponSelector;
            CurrentWeaponSelector.CharacterPickSelector(this, weaponToInstantiate);           
        }
    }

    public void DropWeapon()

    {
        if (CurrentWeaponSelector != null)
        {
            CurrentWeaponSelector.CharacterDropSelector(transform.position);
            CurrentWeaponSelector = null;

        }
    }


    void Update()
    {
        _Moving = false;
        _MovementDirection = Vector2.zero;
        _StateMachine.Update();
        if (CheckIsGrounded())
        {
            transform.parent = _Ground.transform;
        }
        else
        {
            transform.parent = null;
        }
        if (velocity.x > 0)
        {
            SpriteRenderer.flipX = false;
        }
        else if (velocity.x < 0)
        {
            SpriteRenderer.flipX = true;
        }
    }

    public void Move()
    {
        _Moving = true;
        _MovementDirection = InputWalkDirection;
    }
    public void Move2()
    {
        _Moving = true;
        _MovementDirection = velocity.normalized + InputWalkDirection;
    }
    public Vector2 velocity = new Vector2();

    float GetSmoothMovementAxis(float currentVelocity, float inputVelocity, float acceleration, float maxVelocity)
    {
        float newVelocity = currentVelocity;
        bool movingInSameDirection =
            (inputVelocity > 0 && currentVelocity > 0) ||
            (inputVelocity < 0 && currentVelocity < 0);

        if (movingInSameDirection)
        {
            if (Mathf.Abs(currentVelocity) > maxVelocity)
            {
                return currentVelocity;
            }
        }

        bool movingInOppositeDirection =
            (inputVelocity > 0 && currentVelocity < 0) ||
            (inputVelocity < 0 && currentVelocity > 0);

        //if is moveing in opposite direction, Add reaction to acceleration
        if (movingInOppositeDirection)
        {
            acceleration += acceleration * ReactivityPercent;
        }

        newVelocity = Mathf.MoveTowards(currentVelocity, inputVelocity * maxVelocity, acceleration * Time.fixedDeltaTime);

        newVelocity = Mathf.Sign(newVelocity) * Mathf.Min(Mathf.Abs(newVelocity), WalkingMaxSpeed);
        return newVelocity;

    }
    void ApplySmoothMovement()
    {
        Vector2 v = velocity;
        v.x = GetSmoothMovementAxis(v.x, _MovementDirection.x, WalkingAcceleration, WalkingMaxSpeed);
        if (!LockYMovement)
        {
            v.y = GetSmoothMovementAxis(v.y, _MovementDirection.y, WalkingAcceleration, WalkingMaxSpeed);
        }

        velocity = v;
        // Rigidbody.velocity = v;
    }


    public void ApplyGravity()
    {
        velocity += Physics2D.gravity * Time.deltaTime;
    }

    public void CheckCollisions()
    {
        _Ground = CheckWallCollision(Vector2.down, _PlatformLayerMask);
        _LeftWall = CheckWallCollision(Vector2.left, _GroundLayerMask);
        _RightWall = CheckWallCollision(Vector2.right, _GroundLayerMask);
        _TopWall = CheckWallCollision(Vector2.up, _GroundLayerMask);

        if (_Ground)
        {
            velocity.y = Mathf.Max(0, velocity.y);
        }

        if (_TopWall)
        {
            velocity.y = Mathf.Min(0, velocity.y);
        }

        if (_LeftWall)
        {
            velocity.x = Mathf.Max(0, velocity.x);
        }

        if (_RightWall)
        {
            velocity.x = Mathf.Min(0, velocity.x);
        }

    }

    public void FixedUpdate()
    {
        if (_Moving)
        {
            ApplySmoothMovement();
        }
        if (UseGravity)
        {
            ApplyGravity();
        }
        if (ShouldCheckCollisions)
        {
            CheckCollisions();
        }

        transform.position += (Vector3)velocity * Time.fixedDeltaTime;
    }

    GameObject CheckWallCollision(Vector2 direction, int layerMask)
    {
        float rayDistance = 0.1f;
        Vector2 origin = transform.position + ((Vector3)direction * _Collider.bounds.extents.y);

        Debug.DrawLine(origin, origin + (direction * rayDistance));
        var hit = Physics2D.Raycast(origin, direction, rayDistance, layerMask);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    bool CheckIsGrounded()
    {
        if (velocity.y > 0)
        {
            return false;
        }

        return _Ground != null;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        WeaponFactionSelector box = other.GetComponentInParent<WeaponFactionSelector>();
        if (box != null)
        {

            if(CanPickupWeapon)
            {
                PickupWeapon(box);
            }
        }
    }

    public bool CheckIsDead()
    {
        return HealthPoints <= 0;
    }

    bool IWeaponTarget.ApplyHit(HitInfo hitInfo)
    {
        if (IgnoreBullets)
        {
            return false;
        }

        HitInfo = hitInfo;
        HealthPoints -= hitInfo.HitDamage;
        _StateMachine.TriggerEvent((int)EventTriggers.Stunned);

        // play audio
        AudioManager.Instance.PlaySFX(hurtAudio);

        return true;
    }

    bool IWeaponTarget.IsActive()
    {
        return !CheckIsDead();
    }

    CharacterFaction IWeaponTarget.GetCharacaterFaction()
    {
        return characterFaction;
    }

    public void Spring(Vector2 force)
    {

        // remove velocity component in force direction and add force
        Vector2 reducingComponent = Vector2.Dot(force.normalized, velocity) * force.normalized;
        velocity -= reducingComponent;
        velocity += force;
        _StateMachine.TriggerEvent((int)EventTriggers.Springed);
    }

}
