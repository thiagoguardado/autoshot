using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachines;
public class CharacterJumping : State<Character>
{
    public int _JumpingAnimation = Animator.StringToHash("jump");
    public int _FallingAnimation = Animator.StringToHash("fall");
    public bool _StoppedJumping = false;
    public override void Enter()
    {
        base.Enter();
        
        Vector2 velocity = Agent.velocity;
        velocity.y += Agent.JumpForceMax;
        Agent.velocity = velocity;
        _StoppedJumping = false;
    }

    public override void Update()
    {
        base.Update();
        if(!_StoppedJumping)
        {
            if(!Agent.InputIsJumping)
            {
                Vector2 velocity = Agent.velocity;
                velocity.y = Mathf.Min(Agent.JumpForceMin, velocity.y);
                Agent.velocity = velocity;
                _StoppedJumping = true;
            }
        }
        
        Agent.Move();

        if(Agent.velocity.y > 0)
        {
            Agent.Animator.Play(_JumpingAnimation);
        }
        else
        {
            Agent.Animator.Play(_FallingAnimation);
        }

    }
}
