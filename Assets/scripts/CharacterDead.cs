using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachines;
public class CharacterDeadState : State<Character>
{
    public override void Enter()
    {
        base.Enter();
        Agent.Animator.Play("dead");
        Agent.IgnoreBullets = true;
        Agent._MovementDirection = Vector2.zero;
        if(Agent.CurrentWeapon != null)
        {
            Agent.CurrentWeapon.Shooting = false;
        }
    }


    public override void Exit()
    {
        base.Exit();
        Agent.IgnoreBullets = false;
        if (Agent.CurrentWeapon != null)
        {
            Agent.CurrentWeapon.Shooting = true;
        }
    }
}
