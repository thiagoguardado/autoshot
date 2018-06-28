using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtTrigger : MonoBehaviour {
    public HitInfo HitInfo = new HitInfo();
    public Collider IgnoreCollider = null;
    public List<CharacterFaction> FriendFactions = new List<CharacterFaction>();
    private Collider2D collider2D;
    private ContactFilter2D contactFilter = new ContactFilter2D();
    Collider2D[] overlapping;

    void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        contactFilter.useTriggers = true;
        overlapping = new Collider2D[5];
    }


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

    void Update()
    {

        CheckOverlap();
    }

    private void CheckOverlap()
    {
        collider2D.OverlapCollider(contactFilter, overlapping);

        for (int i = 0; i < overlapping.Length; i++)
        {
            if (overlapping[i] == null)
            {
                return;
            }

            if (overlapping[i] != IgnoreCollider)
            {
                OnHit(overlapping[i].gameObject);
            }
        }
    }

    //public void OnTriggerStay2D(Collider2D other)
    //{

    //    if (other == IgnoreCollider)
    //    {
    //        return;
    //    }
    //    OnHit(other.gameObject);
    //}
}
