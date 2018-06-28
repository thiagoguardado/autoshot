using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Melee : Weapon
{

    [Header("Melee Configuration")]
    public float hitRange = 1;
    public float areaAngle = 60f;
    public LineRenderer lineRenderer;
    public float animationDuration = 0.1f;
    public HitInfo hitInfo;
    private Vector3 targetDirection;

    protected override void Awake()
    {
        base.Awake();

        checkSight = false;
    }

    public override void Shot()
    {
        // find center direction
        targetDirection  = _ClosestTarget.transform.position - transform.position;

        // find targets in range
        HitTargets();

        // make animation
        StartCoroutine(LineAnimation(animationDuration));

        if (!InfiniteAmmo)
        {
            Ammo--;
        }
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
                }
            }
        }
    }

    private IEnumerator LineAnimation(float duration)
    {

        float angleToTarget = Vector3.Angle(targetDirection, Vector3.right) * Mathf.Sign(Vector3.Dot(targetDirection, Vector3.right));
        Quaternion startRotation = Quaternion.Euler(0, 0, angleToTarget + areaAngle / 2);
        Quaternion finalRotation = Quaternion.Euler(0, 0, angleToTarget - areaAngle / 2);

        lineRenderer.transform.localScale = new Vector3(Range, 1, 1);
        lineRenderer.enabled = true;

        float timer = 0f;

        while (timer<=duration)
        {
            lineRenderer.transform.rotation = Quaternion.Slerp(startRotation, finalRotation, timer / duration);
            timer += Time.deltaTime;
            yield return null;

        }

        lineRenderer.enabled = false;

    }

}
