using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VisionSensor {

    public LayerMask LayerMask;
    public Vector2 Origin = Vector2.zero;
    [Range(0.0f, 360.0f)]
    public float Angle = 90.0f;
    public float Length = 3.0f;

    [Header("Debug")]
    public Color NormalDebugColor = Color.white;
    public Color HitDebugColor = Color.green;
    
    private Vector2 _RayOrigin = new Vector2();
    private Vector2 _RayDirection = new Vector2();
    private RaycastHit2D _RayHit;

    public VisionSensor()
    {

    }
    public VisionSensor(LayerMask mask, Vector2 origin, float angle, float length)
    {
        LayerMask = mask;
        Origin = origin;
        Angle = angle;
        Length = length;
    }
    public float DistanceToTarget
    {
        get
        {
            if(_RayHit.collider == null)
            {
                return Length;
            }
            else
            {
                return _RayHit.distance;
            }
        }
    }

    private Color _DebugColor
    {
        get
        {
            if(_RayHit.collider == null)
            {
                return NormalDebugColor;
            }
            else
            {
                return HitDebugColor;
            }
        }
    }

    public Vector2 VectorFromAngle(float angle)
    {
        Vector2 v = new Vector2();
        v.x = Mathf.Cos(angle * Mathf.Deg2Rad);
        v.y = Mathf.Sin(angle * Mathf.Deg2Rad);
        return v;
    }

    public GameObject Sense(Vector3 agentPosition)
    {
        _RayOrigin = (Vector2) agentPosition + Origin;
        _RayDirection = VectorFromAngle(Angle);
        _RayHit = Physics2D.Raycast(_RayOrigin, _RayDirection, Length, LayerMask.value);
        if(_RayHit.collider != null)
        {
            return _RayHit.collider.gameObject;
        }

        return null;
    }

    public void DebugDraw()
    {
       
        Debug.DrawLine(_RayOrigin, _RayOrigin + (_RayDirection * DistanceToTarget), _DebugColor);
    }

}
