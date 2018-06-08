using UnityEngine;
using FiniteStateMachines;
using System;

[RequireComponent(typeof(Collider2D))]
public class Character : MonoBehaviour, IWeaponTarget {

    public Animator Animator;
    public float WalkingMaxSpeed = 3;
    public float WalkingAcceleration = 3;
    public float ReactivityPercent = 0.5f;
    public float JumpForceMax = 6;
    public float JumpForceMin = 4;

    public GameObject _Ground = null;
    public GameObject _RightWall = null;
    public GameObject _LeftWall = null;
    public GameObject _TopWall = null;

    public int HealthPoints = 10;
    public Vector2 _MovementDirection;

    public bool IgnoreBullets = false;
    int _GroundLayerMask;
    int _PlatformLayerMask;
    

    [HideInInspector]
    public Weapon CurrentWeapon { get; private set; }

    [HideInInspector]
    public Vector2 InputWalkDirection;

    [HideInInspector]
    public bool InputIsJumping;

    [HideInInspector]
    public HitInfo HitInfo { get; private set; }

    
    private Collider2D _Collider;

    FiniteStateMachine<Character> _StateMachine;
    CharacterJumping _JumpingState = new CharacterJumping();
    CharacterIdle _IdleState = new CharacterIdle();
    CharacterStunned _StunnedState = new CharacterStunned();
    CharacterDeadState _DeadState = new CharacterDeadState();

    public enum EventTriggers
    {
        EndState,
        Stunned
    }
    
    void Awake()
    {

        _GroundLayerMask = LayerMask.GetMask("Ground");
        _PlatformLayerMask = LayerMask.GetMask("Platform", "Ground");

        _Collider = GetComponent<Collider2D>();

        _IdleState.AddCondition(CheckIsDead, _DeadState);
        _IdleState.AddCondition(() => InputIsJumping, _JumpingState);
        _IdleState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);

        _JumpingState.AddCondition(CheckIsDead, _DeadState);
        _JumpingState.AddCondition(CheckIsGrounded, _IdleState);
        _JumpingState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);
        

        _StunnedState.AddTrigger((int)EventTriggers.EndState, _IdleState);
        _StunnedState.AddTransition((int)EventTriggers.EndState, CheckIsDead, _DeadState);
        _StunnedState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);
       

        

        _StateMachine = new FiniteStateMachine<Character>(this);
        _StateMachine.SetState(_IdleState);
    }
    
    void Start()
    {
        

    }
    public void StateEnded()
    {
        _StateMachine.TriggerEvent((int)EventTriggers.EndState);
    }

    void PickupWeapon(GameObject prefab)
    {
        if(CurrentWeapon != null)
        {
            CurrentWeapon.DestroyWeapon();
        }
        var currentWeaponObject = Instantiate(prefab);
        CurrentWeapon = currentWeaponObject.GetComponent<Weapon>();
        
        CurrentWeapon.transform.position = transform.position;
        CurrentWeapon.IgnoreCollider = _Collider;
        CurrentWeapon.IgnoreTarget = gameObject;
        CurrentWeapon.Holder = this;
    }

    void Update()
    {
        _StateMachine.Update();
        if(CheckIsGrounded())
        {
            transform.parent = _Ground.transform;
        }
        else
        {
            transform.parent = null;
        }
    }
    
    public void Move()
    {
        _MovementDirection = InputWalkDirection;
    }

    public Vector2 velocity = new Vector2();

    void ApplySmoothMovement()
    {
        Vector2 v = velocity;
        bool movingInSameDirection =
            (_MovementDirection.x > 0 && v.x > 0) ||
            (_MovementDirection.x < 0 && v.x < 0);

        if (movingInSameDirection)
        {

            if (Mathf.Abs(v.x) > WalkingMaxSpeed)
            {
                return;
            }

        }

        float acceleration = WalkingAcceleration;

        bool movingInOppositeDirection =
            (_MovementDirection.x > 0 && v.x < 0) ||
            (_MovementDirection.x < 0 && v.x > 0);

        //if is moveing in opposite direction, Add reaction to acceleration
        if (movingInOppositeDirection)
        {
            acceleration += acceleration * ReactivityPercent;
        }

        v.x = Mathf.MoveTowards(v.x, _MovementDirection.x * WalkingMaxSpeed, acceleration * Time.fixedDeltaTime);

        v.x = Mathf.Sign(v.x) * Mathf.Min(Mathf.Abs(v.x), WalkingMaxSpeed);
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

        if(_Ground)
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
        ApplySmoothMovement();
        ApplyGravity();
        CheckCollisions();
        transform.position += (Vector3) velocity * Time.fixedDeltaTime;
    }

    GameObject CheckWallCollision(Vector2 direction, int layerMask)
    {
        float rayDistance = 0.1f;
        Vector2 origin = transform.position + ((Vector3) direction * _Collider.bounds.extents.y);

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
		if(velocity.y > 0)
		{
			return false;
		}

        return _Ground != null;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        WeaponBox box = other.GetComponent<WeaponBox>();
        if(box != null)
        {
            var weaponBox = other.GetComponent<WeaponBox>();
            PickupWeapon(weaponBox.WeaponPrefab);
            weaponBox.DestroyBox();
        }
    }

    public bool CheckIsDead()
    {
        return HealthPoints <= 0;
    }

    bool IWeaponTarget.ApplyHit(HitInfo hitInfo)
    {
        if(IgnoreBullets)
        {
            return false;
        }

        HitInfo = hitInfo;
        HealthPoints -= 1;
        _StateMachine.TriggerEvent((int)EventTriggers.Stunned);
        return true;
    }

    bool IWeaponTarget.IsActive()
    {
        return !CheckIsDead();
    }
}
