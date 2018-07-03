using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachines;

public class CharacterDeadState : State<Character>
{
    public override void Enter()
    {
        base.Enter();


        Agent.IsDead = true;

        if (Agent.CurrentWeaponSelector != null)
        {
            Agent.DropWeapon();
        }

        GameManager.Instance.NotifyDeath(Agent);

        Agent.Sprite.Play(Agent.Sprite.animations.dead);
        Agent.IgnoreBullets = true;
        Agent._MovementDirection = Vector2.zero;

        Agent.CanPickupWeapon = false;
        SetChildrenActive(false);
        
        // play sfx
        AudioManager.Instance.PlaySFX(Agent.deathAudio);

        // play effects
        VisualEffects.Instance.PlayDieEffect(Agent.transform);

        // notify event
        GameManager.Instance.NotifyDeath(Agent);
    }
    

    public override void Exit()
    {
       
        base.Exit();
        
        SetChildrenActive(true);

        Agent.IgnoreBullets = false;

        Agent.CanPickupWeapon = true;

        Agent.IsDead = false;

        if (Agent.CurrentWeaponSelector != null)
        {
            Agent.DropWeapon();
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
        if(Agent.CurrentWeaponSelector!= null)
        {
            Agent.CurrentWeaponSelector.currentInstantiatedWeapon.Shooting = active;
        }
    }
}
