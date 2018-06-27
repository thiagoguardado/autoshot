using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[SelectionBase]
public class SpawnPoint : MonoBehaviour {
    private SpriteRenderer _SpriteRenderer;

    protected virtual void Awake()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public GameObject Spawn(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
        return instance;
    }

    private Vector3 GetSpawnPosition()
    {
        var min = _SpriteRenderer.bounds.min;
        var max = _SpriteRenderer.bounds.max;
        Vector2 pos = new Vector2();
        pos.x = Random.Range(min.x, max.x);
        pos.y = Random.Range(min.y, max.y);
        return pos;
    }
}
