using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private static LevelManager _Instance;
    public static LevelManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                CreateInstance();
            }
            return _Instance;
        }
    }

    public const string _ResourceName = "LevelManager";

    public bool levelStarted { get; private set; }
    public bool levelEnded { get; private set; }
    public bool inGame { get { return levelStarted && !levelEnded; } }

    private Level currentLevel;

    private static void CreateInstance()
    {
        var prefab = Resources.Load<GameObject>(_ResourceName);
        var instance = Instantiate(prefab);
        instance.name = "_" + _ResourceName;
        instance.transform.SetAsFirstSibling();
        _Instance = instance.GetComponent<LevelManager>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnNotifyLevelFinished += EndLevel;
        GameManager.Instance.OnNotifyDeath += CheckPlayersAlive;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnNotifyLevelFinished -= EndLevel;
        GameManager.Instance.OnNotifyDeath -= CheckPlayersAlive;
    }


    private void Awake()
    {
        Reset();

        DontDestroyOnLoad(gameObject);
 
    }

    private void Start()
    {

        #if UNITY_EDITOR
        // if start from editor
        foreach (int key in GameManager.Instance.gameLevels.levels.Keys)
        {
            if (GameManager.Instance.gameLevels.levels[key].sceneName != "Menu")
            {
                StartLevel();
                break;
            }
        }
        #endif
    }

    public void Reset()
    {
        levelStarted = false;
        levelEnded = false;
    }

    public void StartLevel()
    {
        levelStarted = true;
    }

    public void EndLevel(bool success)
    {
        levelEnded = true;

        if (success)
        {
            GameManager.Instance.gameLevels.OpenLevel(currentLevel.id + 1);
        }

    }


    // check if has any player still alive
    public void CheckPlayersAlive(Character character)
    {
        if (inGame)
        {

            Character[] chars = GameObject.FindObjectsOfType<Character>();

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i].CharacterFaction == CharacterFaction.Player && !chars[i].IsDead)
                {
                    return;
                }
            }

            GameManager.Instance.NotifyLevelFinished(false);

        }
    }


    public void StartLevel(Level levelToStart)
    {
        Reset();

        if (ScenesManager.Instance.TransitionToScene(levelToStart.sceneName))
        {
            currentLevel = levelToStart;

            StartLevel();
        }

    }
}
