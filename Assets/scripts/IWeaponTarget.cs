using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponTarget {
    bool ApplyHit(HitInfo hitInfo);
    bool IsActive();
}
