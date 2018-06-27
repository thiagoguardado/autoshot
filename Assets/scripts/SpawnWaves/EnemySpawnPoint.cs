using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class EnemySpawnPoint : SpawnPoint {
    public int id = 0;

    void OnValidate()
    {
        SetLabel(id.ToString());
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
