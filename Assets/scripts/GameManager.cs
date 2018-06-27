using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager _Instance = null;

    public SpawnableObjects SpawnableObjects = new SpawnableObjects();
    
    public static GameManager Instance
    {
        get
        {
            if(_Instance == null)
            {
                CreateInstance();
            }
            return _Instance;
        }
    }

    public delegate void GameObjectSpawnPointDelegate(GameObject prefab, int spawnPoint);
    public delegate void GameObjectDelegate(GameObject gameObject);
    public delegate void CharacterDelegate(Character character);
    public delegate void LevelFinishedDelegate(bool successful);
    public delegate void WaveNotify(SpawnWave wave);
    public delegate void VoidDelegate();

    public GameObjectSpawnPointDelegate OnRequestSpawn;
    public GameObjectDelegate OnNotifySpawn;
    public CharacterDelegate OnNotifyDeath;
    public LevelFinishedDelegate OnNotifyLevelFinished;
    public WaveNotify OnNotifyWaveStarting;
    
    private static void CreateInstance()
    {
        GameObject prefab = Resources.Load<GameObject>("GameManager");
        
        _Instance = Instantiate(prefab).GetComponent<GameManager>();
        _Instance.name = "_GameManager";
        _Instance.transform.SetAsFirstSibling();
    }

    public void RequestSpawn(GameObject prefab, int spawnPoint)
    {
        if(OnRequestSpawn != null)
        {
            OnRequestSpawn(prefab, spawnPoint);
        }
    }

    public void NotifySpawn(GameObject gameObject)
    {
        if(OnNotifySpawn != null)
        {
            OnNotifySpawn(gameObject);
        }
    }

    public void NotifyDeath(Character character)
    {
        if(OnNotifyDeath != null)
        {
            OnNotifyDeath(character);
        }
    }

    public void NotifyLevelFinished(bool successful)
    {
        if (OnNotifyLevelFinished != null)
        {
            OnNotifyLevelFinished(successful);
        }
    }

    public void NotifyWaveStarting(SpawnWave wave)
    {
        if (OnNotifyWaveStarting != null)
        {
            OnNotifyWaveStarting(wave);
        }
    }


    void Awake()
    {
        if(_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

}
