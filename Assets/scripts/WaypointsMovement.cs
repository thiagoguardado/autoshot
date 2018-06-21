using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsMovement : MonoBehaviour {

 
    [Header("Waypoints")]
    [Tooltip("A plataforma sempre começará indo em direção ao primeiro item da lista")]
    public List<Transform> waypoints = new List<Transform>();
    public bool clockwise = true;
    public float speed = 1f;
    public float nextWaypointCheckThreshold = 0.1f;
    public bool closedPath = true;
    private int currentWaypointIndex = 0;
    private Vector3 nextWaypoint
    {
        get
        {
            return waypoints[currentWaypointIndex].position;
        }
    }

    [Header("Debug")]
    public Color lineColor = Color.red;
    public float directionSymbolSize = 1f;


    private void Start()
    {

        if (closedPath)
        {
            currentWaypointIndex = 0;
        } else
        {
            if (clockwise)
            {
                currentWaypointIndex = 1;
            }
            else
            {
                currentWaypointIndex = 0;
            }
        }

    }


    private void Update()
    {

        MoveToNextWaypoint();

        CheckIfCloseToNextWaypoint();


    }

    private void CheckIfCloseToNextWaypoint()
    {
        if ((transform.position - nextWaypoint).sqrMagnitude <= nextWaypointCheckThreshold * nextWaypointCheckThreshold)
        {

            if (closedPath)
            {

                if (clockwise)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
                }
                else
                {
                    currentWaypointIndex = (currentWaypointIndex - 1);
                    if (currentWaypointIndex < 0)
                        currentWaypointIndex = waypoints.Count - 1;
                }
            }
            else
            {

                if (clockwise)
                {
                    if (currentWaypointIndex >= (waypoints.Count - 1))
                    {
                        transform.position = waypoints[0].position;
                        currentWaypointIndex = 1;
                    }
                    else
                    {
                        currentWaypointIndex += 1;
                    }

                }
                else
                {
                    if (currentWaypointIndex <= 0)
                    {
                        transform.position = waypoints[waypoints.Count - 1].position;
                        currentWaypointIndex = waypoints.Count - 2;
                    }
                    else
                    {
                        currentWaypointIndex -= 1;
                    }
                }
            }



        }
    }

    private void MoveToNextWaypoint()
    {
        transform.Translate((nextWaypoint - transform.position).normalized * speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {

        if (waypoints.Count > 1)
        {

            for (int i = 1; i < waypoints.Count; i++)
            {
                Debug.DrawLine(waypoints[i - 1].position, waypoints[i].position, lineColor);
                DrawDirection(waypoints[i - 1].position + (waypoints[i].position - waypoints[i - 1].position) / 2, waypoints[i].position - waypoints[i - 1].position, clockwise);
            }

            if (closedPath)
            {
                Debug.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position, lineColor);
                DrawDirection(waypoints[waypoints.Count - 1].position + (waypoints[0].position - waypoints[waypoints.Count - 1].position) / 2, waypoints[0].position - waypoints[waypoints.Count - 1].position, clockwise);
            }
          

        }

        // draw direction

        
    }

    void DrawDirection(Vector3 start, Vector3 direction, bool clockwise)
    {
        Vector3 left = Vector3.Cross(Vector3.back, direction.normalized);
        Vector3 right = Vector3.Cross(direction.normalized, Vector3.back);
        Debug.DrawLine(start + left * directionSymbolSize*0.67f, start + (clockwise?1:-1) * direction.normalized * directionSymbolSize,lineColor);
        Debug.DrawLine(start + right * directionSymbolSize * 0.67f, start + (clockwise ? 1 : -1) * direction.normalized * directionSymbolSize, lineColor);
        Debug.DrawLine(start + right * directionSymbolSize * 0.67f, start + left * directionSymbolSize * 0.67f, lineColor);
    }

}
