using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimation : MonoBehaviour {
    public float RotationSpeed = 360;
	
	// Update is called once per frame
	void Update () {
        Vector3 euler = transform.eulerAngles;
        euler.z += RotationSpeed * Time.deltaTime;
        transform.eulerAngles = euler;
	}
}
