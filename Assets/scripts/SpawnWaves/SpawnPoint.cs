using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(SpriteRenderer))]
[SelectionBase]
public class SpawnPoint : MonoBehaviour {
    public int id = 0;
    
    private SpriteRenderer _SpriteRenderer;
    private Text _textUi;

    void OnValidate()
    {
        _textUi = GetComponentInChildren<Text>();
        if(_textUi != null)
        {
            _textUi.text = id.ToString();
        }
        name = "SpawnPoint" + id.ToString();
    }

    void Awake()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.Instance.OnRequestSpawn += OnSpawn;
        
    }

    void OnDestroy()
    {
        GameManager.Instance.OnRequestSpawn -= OnSpawn;
    }

    void OnSpawn(GameObject prefab, int spawnPoint)
    {
        if(!enabled)
        {
            return;
        }
        if(spawnPoint == id)
        {
            Spawn(prefab);
        }
    }

    void Spawn(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
        GameManager.Instance.NotifySpawn(instance);

    }

    Vector3 GetSpawnPosition()
    {
        var min = _SpriteRenderer.bounds.min;
        var max = _SpriteRenderer.bounds.max;
        Vector2 pos = new Vector2();
        pos.x = Random.Range(min.x, max.x);
        pos.y = Random.Range(min.y, max.y);
        return pos;
    }
}
