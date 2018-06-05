using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Spawner : MonoBehaviour {
    public GameObject SpawnPrefab;
    public float Interval;
    Collider2D _Collider;
    float _Timeout;
	// Use this for initialization
    void Awake()
    {
        _Collider = GetComponent<Collider2D>();
    }

	void Start () {
        Reset();
	}
	
    void Reset()
    {
        _Timeout = Interval;
    }
    Vector2 GetRandomPosition()
    {
        var pos = new Vector2();
        Vector2 min = _Collider.bounds.min;
        Vector2 max = _Collider.bounds.max;
        pos.x = Random.Range(min.x, max.x);
        pos.y = Random.Range(min.y, max.y);
        return pos;
    }
    void Spawn()
    {
        GameObject spawnedObject = Instantiate(SpawnPrefab);
        spawnedObject.transform.position = GetRandomPosition();
    }
	// Update is called once per frame
	void Update () {
        _Timeout -= Time.deltaTime;
        if(_Timeout <= 0)
        {
            Spawn();
            Reset();
        }
	}
}
