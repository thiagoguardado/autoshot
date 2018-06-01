using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachines;
public class CharacterGrounded :  State<Character>
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("here");
    }
    public override void Update()
    {
        base.Update();
        Agent.Move();
        if(Agent.walkDirection.x != 0)
        {
            Agent.Animator.Play("walk");
        }
        else
        {
            Agent.Animator.Play("idle");
        }
    }
}
