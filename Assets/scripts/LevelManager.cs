using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private static void CreateInstance()
    {
        var prefab = Resources.Load<GameObject>(_ResourceName);
        var instance = Instantiate(prefab);
        instance.name = "_" + _ResourceName;
        instance.transform.SetAsFirstSibling();
        _Instance = instance.GetComponent<LevelManager>();
    }


    private void Awake()
    {
        Reset();
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

    public void EndLevel()
    {
        levelEnded = true;
    }




}
