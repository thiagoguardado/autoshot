using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachines;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Character : MonoBehaviour {
    public Animator Animator;
    public Character target;
    public Weapon currentWeapon;
    public Vector2 walkDirection;
    public float speed = 3;
    public bool input_jumping = false;
    public float StunTime = 1.0f;
    public Vector2 StunDirection = new Vector2(1, 0);
    public float StunForce = 3.0f;
    public Rigidbody2D Rigidbody;
    private Collider2D _collider;
    public Text WeaponNameGui;
    public Text WeaponAmmoGui;
    public WeaponTarget WeaponTarget;
   
    FiniteStateMachine<Character> _stateMachine;
    CharacterJumping _JumpingState = new CharacterJumping();
    CharacterGrounded _GroundedState = new CharacterGrounded();
    CharacterStunned _StunnedState = new CharacterStunned();

    public enum EventTriggers
    {
        EndState,
        Stunned
    }

    
    public float JumpForce = 1.0f;
    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _GroundedState.AddCondition(() => input_jumping, _JumpingState);
        _GroundedState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);

        _JumpingState.AddCondition(CheckIsGrounded, _GroundedState);
        _JumpingState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);

        _StunnedState.AddTrigger((int)EventTriggers.EndState, _GroundedState);
        _StunnedState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);

        _stateMachine = new FiniteStateMachine<Character>(this);
        _stateMachine.SetState(_GroundedState);
    }
    
    public void Stun(Vector2 direction, float force, float time)
    {
        StunDirection = direction;
        StunForce = force;
        StunTime = time;
        _stateMachine.TriggerEvent((int)EventTriggers.Stunned);
        
    }
    public void StateEnded()
    {
        _stateMachine.TriggerEvent((int)EventTriggers.EndState);
        
    }

    void PickupWeapon(GameObject prefab)
    {
        if(currentWeapon != null)
        {
            currentWeapon.DestroyWeapon();
        }
        var currentWeaponObject = Instantiate(prefab);
        currentWeapon = currentWeaponObject.GetComponent<Weapon>();
        
        currentWeapon.transform.position = transform.position;
        currentWeapon.IgnoreCollider = _collider;
        currentWeapon.IgnoreTarget = WeaponTarget;
        currentWeapon.Holder = this;

    }
    void Update()
    {
        _stateMachine.Update();
        UpdateWeaponGui();
    }

    void UpdateWeaponGui()
    {
        if(currentWeapon == null)
        {
            WeaponNameGui.text = "";
            WeaponAmmoGui.text = "";
        }
        else
        {
            WeaponNameGui.text = currentWeapon.Name;
            WeaponAmmoGui.text = currentWeapon.Ammo.ToString() + "/" + currentWeapon.MaxAmmo.ToString();
        }
    }
    public void Move()
    {
     //   Rigidbody.AddForce(walkDirection * speed, ForceMode2D.Force);
        Rigidbody.velocity = new Vector2(walkDirection.x * speed, Rigidbody.velocity.y);
    }

    bool CheckIsGrounded()
    {
		if(Rigidbody.velocity.y > 0)
		{
			return false;
		}
        int groundCollisionMask = LayerMask.GetMask("Ground");
        float maxGroundDistance = 0.1f;
        float rayDistance = _collider.bounds.extents.y + maxGroundDistance;
        float[] xOrigins = new float[]{
            -_collider.bounds.extents.x + 0.1f,
            0,
            _collider.bounds.extents.x - 0.1f
        };

        Vector2 pos = transform.position;
        foreach (float xOrigin in xOrigins)
        {
            Vector2 origin = new Vector2(pos.x + xOrigin, pos.y);
            var hit = Physics2D.Raycast(origin, Vector2.down, rayDistance, groundCollisionMask);
            if(hit.collider != null)
            {
                return true;
            }
        }

        return false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Box")
        {
            var weaponBox = other.GetComponent<WeaponBox>();
            PickupWeapon(weaponBox.WeaponPrefab);
            weaponBox.DestroyBox();
        }
    }
}
