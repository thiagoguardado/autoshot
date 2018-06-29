using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager _Instance = null;

    public SpawnableObjects SpawnableObjects = new SpawnableObjects();
    public Level[] levels;

    private const string MenuSceneName = "Menu";


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
    public delegate void WaveCount(int currentWave, int totalWaves);
    public delegate void VoidDelegate();

    public GameObjectSpawnPointDelegate OnRequestEnemySpawn;

    public GameObjectDelegate OnNotifySpawn;
    public CharacterDelegate OnNotifyDeath;
    public LevelFinishedDelegate OnNotifyLevelFinished;
    public WaveNotify OnNotifyWaveStarting;
    public WaveCount OnNotifyWaveChanged;

    private InGamePanelController PanelController;
    public GameLevels gameLevels { get; private set; }
  

    private static void CreateInstance()
    {
        GameObject prefab = Resources.Load<GameObject>("GameManager");
        _Instance = Instantiate(prefab).GetComponent<GameManager>();
        _Instance.name = "_GameManager";
        _Instance.transform.SetAsFirstSibling();
    }

    public void RequestSpawn(GameObject prefab, int spawnPoint)
    {
        if(OnRequestEnemySpawn != null)
        {
            OnRequestEnemySpawn(prefab, spawnPoint);
        }
    }

    public void NotifyEnemySpawn(GameObject gameObject)
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

    public void NotifyWaveChanged(int waveCount, int waveTotal)
    {
        if (OnNotifyWaveChanged != null)
        {
            OnNotifyWaveChanged(waveCount, waveTotal);
        }
    }


    public void Pause()
    {
        if (!PanelController.hasPanelOpened)
        {
            Time.timeScale = 0f;
            PanelController.OpenPausePanel();
        }
        else
        {
            Time.timeScale = 1f;
            PanelController.ClosePausePanel();
        }

    }

    public void StartLevel(int id)
    {

        LevelManager.Instance.StartLevel(gameLevels.GetLevelById(id));
    }


    public void LoadMenu()
    {
        ScenesManager.Instance.TransitionToScene(MenuSceneName);
    }


    void Awake()
    {
        if(_Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            _Instance = this;
            PanelController = GetComponentInChildren<InGamePanelController>();
            gameLevels = new GameLevels(levels);
            DontDestroyOnLoad(gameObject);
        }
    }


    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Pause();
        }
    }

}

public class GameLevels
{

    public Dictionary<int,Level> levels = new Dictionary<int, Level>();

    public GameLevels(Level[] levels)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            this.levels.Add(levels[i].id, levels[i]);
        }

        levels[0].isOpened = true;

    }

    public void OpenLevel(int levelId)
    {
        levels[levelId].isOpened = true;
    }

    public Level GetLevelById(int id)
    {
        if (levels.ContainsKey(id))
            return levels[id];

        return null;
    }
}

[System.Serializable]
public class Level
{
    public int id;
    public string sceneName;
    [HideInInspector] public bool isOpened;

    public Level(int id, string sceneName)
    {
        this.sceneName = sceneName;
        this.isOpened = false;
    }
}
