using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class SlimeController : MonoBehaviour {
    private Character _Character;
    public VisionSensor FrontalGroundSensor = new VisionSensor();
    public VisionSensor FrontalWallSensor = new VisionSensor();
    public VisionSensor GrundedSensor = new VisionSensor();


    private int _CurrentDirection = 1;


    public void Awake()
    {
        _Character = GetComponent<Character>();

    }
    public void Flip()
    {
        _CurrentDirection = -_CurrentDirection;
        UpdateSensorDirection();
    }

    public void UpdateSensorDirection()
    {
        FrontalGroundSensor.Origin.x = Mathf.Abs(FrontalGroundSensor.Origin.x) * _CurrentDirection;
        FrontalWallSensor.Origin.x = Mathf.Abs(FrontalGroundSensor.Origin.x) * _CurrentDirection;
        FrontalWallSensor.Angle = _CurrentDirection > 0 ? 0.0f : 180.0f;
    }

    public void Update()
    {
        if(FrontalWallSensor.Sense(transform.position))
        {
            Flip();
        }
        else if(GrundedSensor.Sense(transform.position))
        {
            if (FrontalGroundSensor.Sense(transform.position) == null)
            {
                Flip();
            }
        }
        
        
        _Character.InputWalkDirection = Vector2.right * _CurrentDirection;
        FrontalGroundSensor.DebugDraw();
        FrontalWallSensor.DebugDraw();
    }
}
