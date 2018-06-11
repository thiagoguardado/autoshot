﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachines;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Character : MonoBehaviour {
    public Animator Animator;
    public Character target;
    public Weapon currentWeapon;
    private Vector2 previousWalkDirection = Vector2.zero;
    public Vector2 walkDirection;
    public float speed = 3;
    public bool input_jumping = false;
    public bool canWallJump = false;
    public float testWallRaycastDistance = 1f;
    public Vector2 wallJumpForce = new Vector2(2,10);
    public float StunTime = 1.0f;
    public Vector2 StunDirection = new Vector2(1, 0);
    public float StunForce = 3.0f;
    public Rigidbody2D Rigidbody;
    private Collider2D _collider;
    public Text WeaponNameGui;
    public Text WeaponAmmoGui;
    public WeaponTarget WeaponTarget;

    FiniteStateMachine<Character> _stateMachine;
    CharacterJumping _JumpingState;
    CharacterGrounded _GroundedState;
    CharacterStunned _StunnedState;

    public enum EventTriggers
    {
        EndState,
        Stunned,
        Springed
    }


    public float JumpForce = 1.0f;


    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _JumpingState = new CharacterJumping(canWallJump);
        _GroundedState = new CharacterGrounded();
        _StunnedState = new CharacterStunned();

        _GroundedState.AddCondition(() => input_jumping, _JumpingState);
        _GroundedState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);
        _GroundedState.AddTrigger((int)EventTriggers.Springed, _JumpingState);

        _JumpingState.AddCondition(CheckIsGrounded, _GroundedState);
        _JumpingState.AddTrigger((int)EventTriggers.Stunned, _StunnedState);
        _JumpingState.AddTrigger((int)EventTriggers.Springed, _JumpingState);


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
        if (currentWeapon != null)
        {
            currentWeapon.DestroyWeapon();
        }
        var currentWeaponObject = Instantiate(prefab);
        currentWeapon = currentWeaponObject.GetComponent<Weapon>();

        currentWeapon.NewHolder(this, transform.position, _collider, WeaponTarget);

    }
    void Update()
    {
        _stateMachine.Update();
        UpdateWeaponGui();
    }

    void UpdateWeaponGui()
    {
        if (currentWeapon == null)
        {
            WeaponNameGui.text = "";
            WeaponAmmoGui.text = "";
        }
        else
        {
            WeaponNameGui.text = currentWeapon.Name;

            if (currentWeapon is Weapon_Gun)
            {
                WeaponAmmoGui.text = currentWeapon.Ammo.ToString() + "/" + ((Weapon_Gun)currentWeapon).MaxAmmo.ToString();
            }
            
        }
    }


    public void StopMoving() {

        Rigidbody.velocity -= previousWalkDirection;
        previousWalkDirection = Vector2.zero;

    }

    public void Move()
    {

        //  Rigidbody.AddForce(walkDirection * speed, ForceMode2D.Force);
        Rigidbody.velocity = new Vector2(walkDirection.x * speed, Rigidbody.velocity.y);
        // Rigidbody.MovePosition(Rigidbody.position + new Vector2(walkDirection.x * speed * Time.deltaTime, 0));

        
        //Rigidbody.velocity -= previousWalkDirection;
        //Vector2 walkingVelocity = new Vector2(walkDirection.x * speed, 0);
        //Rigidbody.velocity += walkingVelocity;
        //previousWalkDirection = walkingVelocity;
        
    }

    public void Spring(Vector2 force)
    {

        // remove velocity component in force direction and add force
        Vector2 vel = Rigidbody.velocity;
        Vector2 reducingComponent = Vector2.Dot(force.normalized, vel) * force.normalized;
        vel -= reducingComponent;
        Rigidbody.velocity = vel;
        Rigidbody.AddForce(force, ForceMode2D.Impulse);

        _stateMachine.TriggerEvent((int)EventTriggers.Springed);
    }

    public void WallJump(float directionx)
    {

        Rigidbody.AddForce(Vector2.Scale(new Vector2(directionx,1), wallJumpForce), ForceMode2D.Impulse);

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
