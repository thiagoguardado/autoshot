using UnityEngine;
using FiniteStateMachines;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Character : MonoBehaviour, IWeaponTarget {

    public Animator Animator;
    public float WalkingMaxSpeed = 3;
    public float WalkingAcceleration = 3;
    public float ReactivityPercent = 0.5f;
    public float JumpForceMax = 6;
    public float JumpForceMin = 4;

    public Vector2 _MovementDirection;

    [HideInInspector]
    public Weapon CurrentWeapon { get; private set; }

    [HideInInspector]
    public Vector2 InputWalkDirection;

    [HideInInspector]
    public bool InputIsJumping;

    [HideInInspector]
    public HitInfo HitInfo { get; private set; }

    [HideInInspector]
    public Rigidbody2D Rigidbody;

    private Collider2D _Collider;

    FiniteStateMachine<Character> _StateMachine;
    CharacterJumping _JumpingState = new CharacterJumping();
    CharacterGrounded _GroundedState = new CharacterGrounded();
    CharacterStunned _StunnedState = new CharacterStunned();

    public enum EventTriggers
    {
        EndState,
        Stunned
    }
    
    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _Collider = GetComponent<Collider2D>();

        _GroundedState.AddCondition(() => InputIsJumping, _JumpingState);
        _GroundedState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);

        _JumpingState.AddCondition(CheckIsGrounded, _GroundedState);
        _JumpingState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);

        _StunnedState.AddTrigger((int)EventTriggers.EndState, _GroundedState);
        _StunnedState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);

        _StateMachine = new FiniteStateMachine<Character>(this);
        _StateMachine.SetState(_GroundedState);
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
    }
    
    public void Move()
    {
        _MovementDirection = InputWalkDirection;
    }

    public void FixedUpdate()
    {
      
        Vector2 v = Rigidbody.velocity;

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
        Rigidbody.velocity = v;
    }
    
    bool CheckIsGrounded()
    {
		if(Rigidbody.velocity.y > 0)
		{
			return false;
		}

        int groundCollisionMask = LayerMask.GetMask("Ground");
        float rayDistance = _Collider.bounds.extents.y + 0.1f;

        var hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundCollisionMask);
        return hit.collider != null;  
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

    void IWeaponTarget.ApplyHit(HitInfo hitInfo)
    {
        HitInfo = hitInfo;
        _StateMachine.TriggerEvent((int)EventTriggers.Stunned);
    }
}
