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
    int waveCount = 0;
    int waveTotal = 0;
    bool startedSpawning = false;

    void Awake()
    {
        GameManager.Instance.OnNotifySpawn += OnSpawned;
        GameManager.Instance.OnNotifySpawn += PlayCharacterSpawnEffect;
        GameManager.Instance.OnNotifyDeath += OnCharacterDeath;

        waveTotal = spawnWaves.Count;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnNotifySpawn -= OnSpawned;
        GameManager.Instance.OnNotifySpawn -= PlayCharacterSpawnEffect;
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
        if (!LevelManager.Instance.inGame)
        {
            return;
        }

        _CurrentWaveTimeout -= Time.deltaTime;

        if((!startedSpawning && _CurrentWaveTimeout <= 0) || (startedSpawning && (_CurrentWaveTimeout <= 0 || spawnedEnemies.Count == 0)))
        {
            startedSpawning = true;
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        if(spawnWaves.Count == 0)
        {
            finished = true;
            

            if (spawnedEnemies.Count <= 0)
            {
                GameManager.Instance.NotifyLevelFinished(true);
            }

            
        }
        else
        {
            SpawnWave wave = spawnWaves[0];
            waveCount += 1;
            Spawn(wave);

            spawnWaves.RemoveAt(0);
            _CurrentWaveTimeout = WaveTime;

            GameManager.Instance.NotifyWaveChanged(waveCount,waveTotal);
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
                GameManager.Instance.RequestEnemySpawn(SpawnableObjects.Instance.SlimePool, spawnPoint);
            }
            for (int i = 0; i < spawn.slimes_with_gun; i++)
            {
                GameManager.Instance.RequestEnemySpawn(SpawnableObjects.Instance.SlimeWithGunPool, spawnPoint);
            }
            for (int i = 0; i < spawn.slimes_with_melee; i++)
            {
                GameManager.Instance.RequestEnemySpawn(SpawnableObjects.Instance.SlimeWithMeleePool, spawnPoint);
            }
            for (int i = 0; i < spawn.ghosts; i++)
            {
                GameManager.Instance.RequestEnemySpawn(SpawnableObjects.Instance.GhostPool, spawnPoint);
            }
            for (int i = 0; i < spawn.ghosts_with_gun; i++)
            {
                GameManager.Instance.RequestEnemySpawn(SpawnableObjects.Instance.GhostWithGunPool, spawnPoint);
            }
            for (int i = 0; i < spawn.ghosts_with_melee; i++)
            {
                GameManager.Instance.RequestEnemySpawn(SpawnableObjects.Instance.GhostWithMeleePool, spawnPoint);
            }
        }
    }

    private void PlayCharacterSpawnEffect(GameObject gameObject)
    {
        VisualEffects.Instance.PlaySpawnEffect(gameObject.transform);
    }
}
