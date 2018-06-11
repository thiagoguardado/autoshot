using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Melee : Weapon
{
    
    [Header("Melee Configuration")]
    public float areaAngle = 60f;
    public LineRenderer lineRenderer;
    public float animationDuration = 0.1f;
    public HitInfo hitInfo;
    private Vector3 targetDirection;
    

    public override void Shot()
    {
        // find center direction
        targetDirection  = _ClosestTarget.transform.position - transform.position;

        // find targets in range
        HitTargets();

        // make animation
        StartCoroutine(LineAnimation(animationDuration));
    }

    private void HitTargets()
    {

        foreach (var target in _Targets)
        {
            float angle = Mathf.Abs(Vector3.Angle(targetDirection, target.transform.position - transform.position));
            if (angle <= areaAngle/2)
            {
                var weponTarget = target.GetComponent<IWeaponTarget>();
     
                if (weponTarget != null)
                {
                    hitInfo.StunDirection = target.transform.position - transform.position;
                    weponTarget.ApplyHit(hitInfo);
                    Debug.Log("Melee Hit " + target.name);
                }
            }
        }
    }

    private IEnumerator LineAnimation(float duration)
    {

        
        Quaternion startRotation = Quaternion.Euler(0, 0, -Vector3.Angle(targetDirection, Vector3.right) + areaAngle / 2);
        Quaternion finalRotation = Quaternion.Euler(0, 0, -Vector3.Angle(targetDirection, Vector3.right) - areaAngle / 2);

        lineRenderer.transform.localScale = new Vector3(Range, 1, 1);
        lineRenderer.enabled = true;

        float timer = 0f;

        while (timer<=duration)
        {
            lineRenderer.transform.rotation = Quaternion.Lerp(startRotation, finalRotation, timer / duration);
            timer += Time.deltaTime;
            yield return null;

        }

        lineRenderer.enabled = false;

    }

}
