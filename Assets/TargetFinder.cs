using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour {
    public Character IgnoreTarget;
    public List<Character> TargetsInside;
    public List<Character> LineOfSightTargets;
    public Character ClosestTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckLineOfSightTargets();
        FindClosestTarget();
	}
    void CheckLineOfSightTargets()
    {
        int layerMask = LayerMask.GetMask("Ground");
        LineOfSightTargets.Clear();
        foreach(Character character in TargetsInside)
        {
            if(character == IgnoreTarget)
            {
                continue;
            }
            float distance = Vector2.Distance(transform.position, character.transform.position);
            Vector2 direction = character.transform.position - transform.position;
            var hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, layerMask);
            if(hit.collider == null)
            {
                LineOfSightTargets.Add(character);
            }
        }
    }

    void FindClosestTarget()
    {
        float bestSqrDistance = float.MaxValue;
        ClosestTarget = null;
        foreach(Character character in LineOfSightTargets)
        {
            float sqrDistance = Vector2.SqrMagnitude(character.transform.position - transform.position);
            if(sqrDistance < bestSqrDistance)
            {
                bestSqrDistance = sqrDistance;
                ClosestTarget = character;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Character")
        {
            var character = other.GetComponent<Character>();
            if (!TargetsInside.Contains(character))
            {
                TargetsInside.Add(character);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Character")
        {
            var character = other.GetComponent<Character>();
            if (TargetsInside.Contains(character))
            {
                TargetsInside.Remove(character);
            }
        }
    }
}
