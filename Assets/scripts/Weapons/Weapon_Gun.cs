using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Gun : Weapon
{
    [Header("Gun Shot Configuration")]
    public GameObject ShotPrefab = null;
    public float Spread = 1.0f;
    public int Shots = 1;
    public float ShotSpeed = 1.0f;
    public int MaxAmmo = 10;



    public override void Shot()
    {

        for (int i = 0; i < Shots; i++)
        {
            ShootProjectile();
            Ammo--;
        }
    }


    private void ShootProjectile()
    {
        Vector2 shotDirection = _ClosestTarget.transform.position - transform.position;
        float spreadAngle = Random.Range(-Spread, Spread);
        float sin = Mathf.Sin(spreadAngle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(spreadAngle * Mathf.Deg2Rad);

        Vector2 spreadedDirection = new Vector2();
        spreadedDirection.x = (cos * shotDirection.x) - (sin * shotDirection.y);
        spreadedDirection.y = (sin * shotDirection.x) + (cos * shotDirection.y);

        if (ShotPrefab != null)
        {
            GameObject bulletObject = Instantiate(ShotPrefab, transform.position, Quaternion.identity);
            var bullet = bulletObject.GetComponent<Bullet>();

            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), IgnoreCollider);
            bullet.IgnoreCollider = IgnoreCollider;


            bulletObject.transform.position = transform.position;
            var rigidBody = bulletObject.GetComponent<Rigidbody2D>();
            rigidBody.velocity = spreadedDirection * ShotSpeed;
        }

    }

}
