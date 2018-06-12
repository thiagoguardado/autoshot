using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSensorTester : MonoBehaviour {
    public VisionSensor sensor = new VisionSensor();

 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        sensor.Sense(transform.position);
        sensor.DebugDraw();
	}
}
