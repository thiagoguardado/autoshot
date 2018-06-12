using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachines;
public class CharacterIdle :  State<Character>
{

    public override void Update()
    {
        base.Update();
        Agent.Move();
        if(Agent.InputWalkDirection.x != 0)
        {
            Agent.Animator.Play("walk");
        }
        else
        {
            Agent.Animator.Play("idle");
        }
    }
}
