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
        SetChildrenActive(false);
    }

   

    public override void Exit()
    {
       
        base.Exit();

        SetChildrenActive(true);

        Agent.IgnoreBullets = false;
        if (Agent.CurrentWeapon != null)
        {
            Agent.CurrentWeapon.Shooting = true;
        }
    }

    public void SetChildrenActive(bool active)
    {
        if (Agent.WeaponCanvas != null)
        {
            Agent.WeaponCanvas.gameObject.SetActive(active);
        }
        if (Agent.HurtTrigger != null)
        {
            Agent.HurtTrigger.gameObject.SetActive(active);
        }
        if (Agent.CurrentWeapon != null)
        {
            Agent.CurrentWeapon.Shooting = active;
            Agent.CurrentWeapon.gameObject.SetActive(active);
        }
    }
}
