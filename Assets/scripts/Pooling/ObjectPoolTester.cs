using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolTester: MonoBehaviour {


    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Spawn(SpawnableObjects.Instance.SlimePool);
        }
        if (Input.GetKeyDown("2"))
        {
            Spawn(SpawnableObjects.Instance.SlimeWithGunPool);
        }
        if (Input.GetKeyDown("3"))
        {
            Spawn(SpawnableObjects.Instance.SlimeWithMeleePool);
        }
        if (Input.GetKeyDown("4"))
        {
            Spawn(SpawnableObjects.Instance.GhostPool);
        }
        if (Input.GetKeyDown("5"))
        {
            Spawn(SpawnableObjects.Instance.GhostWithMeleePool);
        }
        if (Input.GetKeyDown("6"))
        {
            Spawn(SpawnableObjects.Instance.GhostWithGunPool);
        }
        if (Input.GetKeyDown("7"))
        {
            Spawn(SpawnableObjects.Instance.MeleeWeaponSelectorPool);
        }
        if (Input.GetKeyDown("8"))
        {
            Spawn(SpawnableObjects.Instance.GunWeaponSelectorPool);
        }
        if (Input.GetKeyDown("0"))
        {
            Spawn(SpawnableObjects.Instance.GunBulletPool);
        }
    }

    void Spawn(ObjectPool pool)
    {
        var instance = pool.create();
        instance.transform.position = transform.position;
        instance.GetComponent<Rigidbody2D>().velocity = Vector3.right;
    }
}
