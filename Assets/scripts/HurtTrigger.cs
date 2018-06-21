using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtTrigger : MonoBehaviour {
    public HitInfo HitInfo = new HitInfo();
    public Collider IgnoreCollider = null;
    public List<CharacterFaction> FriendFactions = new List<CharacterFaction>();

    // Use this for initialization

    public void OnHit(GameObject other)
    {
        IWeaponTarget target = other.GetComponent<IWeaponTarget>();
        if(target == null)
        {
            return; 
        }
        
        if (!FriendFactions.Contains(target.GetCharacaterFaction()))
        {
            HitInfo.StunDirection = other.transform.position - transform.position;
            target.ApplyHit(HitInfo);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other == IgnoreCollider)
        {
            return;
        }
        OnHit(other.gameObject);
    }
}
