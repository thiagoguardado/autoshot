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
            Agent.Sprite.Play(Agent.Sprite.animations.walk);
        }
        else
        {
            Agent.Sprite.Play(Agent.Sprite.animations.idle);
        }
    }
}
