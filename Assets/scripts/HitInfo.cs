using UnityEngine;

[System.Serializable]
public class HitInfo {
    [HideInInspector] public Vector2 StunDirection;
    public float StunForce;
    public float StunTime;

    public HitInfo(Vector2 stunDirection, float stunForce, float stunTime)
    {
        StunDirection = stunDirection;
        StunForce = stunForce;
        StunTime = stunTime;
    }
}
