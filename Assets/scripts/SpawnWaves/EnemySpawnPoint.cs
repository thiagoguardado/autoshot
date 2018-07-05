using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class EnemySpawnPoint : SpawnPoint {
    public int id = 0;

    void OnValidate()
    {
        labelText = id.ToString();
        SetLabel(id.ToString());
        name = "SpawnPoint" + id.ToString();
    }

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.OnRequestSpawnEnemy += OnSpawnEnemy;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnRequestSpawnEnemy -= OnSpawnEnemy;
    }

    void OnSpawnEnemy(ObjectPool pool, int spawnPoint)
    {
        if(!enabled)
        {
            return;
        }
        if(spawnPoint == id)
        {
            var instance = Spawn(pool);
            GameManager.Instance.NotifyEnemySpawn(instance);
        }
    }   
}
