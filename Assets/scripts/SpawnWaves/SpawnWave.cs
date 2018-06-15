using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnWave", menuName = "_AutoShot/SpawnWave", order = 1)]
public class SpawnWave: ScriptableObject {
    public List<SpawnInstance> SpawnPoints;

    [System.Serializable]
    public class SpawnInstance
    {
        [HideInInspector]
        public string name;
        public int slimes = 0;
        public int ghosts = 0;
    }

    void OnValidate()
    {
        for(int i = 0; i < SpawnPoints.Count; i++)
        {
            SpawnPoints[i].name = "" + i.ToString();
        }
    }
}
