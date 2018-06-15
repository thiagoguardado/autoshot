using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaveController : MonoBehaviour {
    public List<SpawnWave> spawnWaves;
    public List<Character> spawnedEnemies;
    public float StartTime = 3.0f;
    public float WaveTime = 20.0f;
    float _CurrentWaveTimeout = 0;
    bool finished = false;

    void Awake()
    {
        GameManager.Instance.OnNotifySpawn += OnSpawned;
        GameManager.Instance.OnNotifyDeath += OnCharacterDeath;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnNotifySpawn -= OnSpawned;
        GameManager.Instance.OnNotifyDeath -= OnCharacterDeath;
    }

    void Start()
    {
        _CurrentWaveTimeout = StartTime;
    }

    void Update()
    {
        if(!enabled)
        {
            return;
        }
        if(finished)
        {
            return;
        }

        _CurrentWaveTimeout -= Time.deltaTime;
        if (_CurrentWaveTimeout <= 0 || spawnedEnemies.Count == 0)
        {
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        if(spawnWaves.Count == 0)
        {
            finished = true;

            bool successful = spawnedEnemies.Count == 0;
            GameManager.Instance.NotifyLevelFinished(successful);
        }
        else
        {
            SpawnWave wave = spawnWaves[0];
            Spawn(wave);

            spawnWaves.RemoveAt(0);
            _CurrentWaveTimeout = WaveTime;

            GameManager.Instance.NotifyWaveStarting(wave);
        }
    }

    void OnSpawned(GameObject gameObject)
    {
        if(!enabled)
        {
            return;
        }

        var character = gameObject.GetComponent<Character>();
        if(character == null)
        {
            return;
        }
        spawnedEnemies.Add(character);
    }

    void OnCharacterDeath(Character character)
    {
        if(spawnedEnemies.Contains(character))
        {
            spawnedEnemies.Remove(character);
        }
    }

    void Spawn(SpawnWave spawnWave)
    {
        for(int spawnPoint = 0; spawnPoint < spawnWave.SpawnPoints.Count; spawnPoint++)
        {
            var spawn = spawnWave.SpawnPoints[spawnPoint];
            for (int i = 0; i < spawn.slimes; i++)
            {
                GameManager.Instance.RequestSpawn(GameManager.Instance.SpawnableObjects.Slime.gameObject, spawnPoint);
            }
            for (int i = 0; i < spawn.ghosts; i++)
            {
                GameManager.Instance.RequestSpawn(GameManager.Instance.SpawnableObjects.Ghost.gameObject, spawnPoint);
            }
        }
    }
}
