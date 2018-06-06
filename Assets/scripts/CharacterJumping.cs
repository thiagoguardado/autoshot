using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachines;
public class CharacterJumping : State<Character>
{
    public string animationUp;
    public string animationDown;
    public override void Enter()
    {
        base.Enter();
        //Agent.Rigidbody.AddForce(Vector2.up * Agent.JumpForce, ForceMode2D.Impulse);
        Vector2 velocity = Agent.Rigidbody.velocity;
        velocity.y += Agent.JumpForce;
        Agent.Rigidbody.velocity = velocity;

    }

    public override void Update()
    {
        base.Update();
        if(Agent.input_jumping)
        {
            Agent.Rigidbody.AddForce(Vector2.up * 5, ForceMode2D.Force);
        }
        
        Agent.Move();

        if(Agent.Rigidbody.velocity.y > 0)
        {
            Agent.Animator.Play("jump");
        }
        else
        {
            Agent.Animator.Play("fall");
        }
    }
}
