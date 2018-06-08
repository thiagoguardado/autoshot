using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachines;
public class CharacterStunned : State<Character>
{
    float Timeout = 1;
 
    public override void Enter()
    {
        base.Enter();
        HitInfo hit = Agent.HitInfo;
        Timeout = hit.StunTime;
        Agent.velocity += hit.StunDirection.normalized * hit.StunForce;
        Agent.Animator.Play("stun");
    }

    public override void Update()
    {
        base.Update();

        Timeout -= Time.deltaTime;
        if(Timeout <= 0)
        {
            Agent.StateEnded();
        }
    }
}
