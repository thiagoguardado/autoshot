using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnController : MonoBehaviour {
    [SerializeField]
    private WeaponFactionSelector _GunSpawnPrefab;

    [SerializeField]
    private WeaponFactionSelector _MeleeSpawnPrefab;

    public float SpawnRandomInterval = 1.0f;

    List<WeaponSpawnPoint> _SpawnPoints;
    List<WeaponSpawnPoint> _MeleeSpawnPoints;
    List<WeaponSpawnPoint> _GunSpawnPoints;

    void Awake()
    {
        var spawnPointsArray = FindObjectsOfType<WeaponSpawnPoint>();
        _SpawnPoints = new List<WeaponSpawnPoint>(spawnPointsArray);
        _MeleeSpawnPoints = new List<WeaponSpawnPoint>();
        _GunSpawnPoints = new List<WeaponSpawnPoint>();
        for (int i = 0; i < _SpawnPoints.Count; i++)
        {
            if(_SpawnPoints[i].SpawnableWeponTypes.Contains(WeaponClass.Melee))
            {
                _MeleeSpawnPoints.Add(_SpawnPoints[i]);
            }
            if (_SpawnPoints[i].SpawnableWeponTypes.Contains(WeaponClass.Melee))
            {
                _GunSpawnPoints.Add(_SpawnPoints[i]);
            }
        }
    }

    void Start()
    {

        StartCoroutine(SpawnRandomRoutine());
    }

    IEnumerator SpawnRandomRoutine()
    {
        for(;;)
        {
            yield return new WaitForSeconds(SpawnRandomInterval);
            SpawnAny();
        }
    }
    
    void SpawnAny()
    {
        var spawnPoint = GetRandomSpawnPoint(ref _SpawnPoints);
        if (spawnPoint == null)
        {
            return;
        }

        var prefab = PrefabWithWeaponClass(spawnPoint.ChooseWeaponClass());
        if (prefab == null)
        {
            return;
        }

        spawnPoint.Spawn(prefab.gameObject);
    }

    void SpawnMelee()
    {
        var spawnPoint = GetRandomSpawnPoint(ref _MeleeSpawnPoints);
        spawnPoint.Spawn(_MeleeSpawnPrefab.gameObject);
    }

    void SpawnGun()
    {
        var spawnPoint = GetRandomSpawnPoint(ref _GunSpawnPoints);
        spawnPoint.Spawn(_GunSpawnPrefab.gameObject);
    }

    WeaponSpawnPoint GetRandomSpawnPoint(ref List<WeaponSpawnPoint> points)
    {
        if(points.Count <= 0)
        {
            return null;
        }

        int index = Random.Range(0, points.Count);
        return points[index];
    }


    WeaponFactionSelector PrefabWithWeaponClass(WeaponClass weaponClass)
    {
        switch (weaponClass)
        {
            case WeaponClass.Melee:
                return _MeleeSpawnPrefab;
            case WeaponClass.Gun:
                return _GunSpawnPrefab;
            case WeaponClass.None:
                return null;
            default:
                return null;
        }
    }
}
