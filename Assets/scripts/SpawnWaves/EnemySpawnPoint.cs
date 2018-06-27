using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class EnemySpawnPoint : SpawnPoint {
    public int id = 0;    
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

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.OnRequestEnemySpawn += OnSpawnEnemy;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnRequestEnemySpawn -= OnSpawnEnemy;
    }

    void OnSpawnEnemy(GameObject prefab, int spawnPoint)
    {
        if(!enabled)
        {
            return;
        }
        if(spawnPoint == id)
        {
            var instance = Spawn(prefab);
            GameManager.Instance.NotifyEnemySpawn(instance);
        }
    }   
}
