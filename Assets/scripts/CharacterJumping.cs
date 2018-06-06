using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachines;
using System;

public class CharacterJumping : State<Character>
{
    public string animationUp;
    public string animationDown;
    public bool checkWall = false;
    public bool nextToWall = false;

    public CharacterJumping(bool checkWall)
    {
        this.checkWall = checkWall;
    }


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


        CheckWallJump();

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

    private void CheckWallJump()
    {
        if (checkWall)
        {
            if (Agent.input_jumping)
            {
                foreach (var direction in new[] { Vector2.right, Vector2.left })
                {
                    RaycastHit2D hit = Physics2D.Raycast(Agent.transform.position, direction, Agent.testWallRaycastDistance, LayerMask.GetMask("Ground"));
                    Debug.DrawRay(Agent.transform.position, direction * Agent.testWallRaycastDistance);
                    if (hit.transform != null)
                    {
                        Agent.WallJump(Mathf.Sign(direction.x) * -1);
                    }
                }
            }
        }
    }
}
