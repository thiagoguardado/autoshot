using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactionSelector : MonoBehaviour {


    public const float cooldownToPickAgain = 1.0f;

    private SpriteRenderer visualObjectSpriteRenderer;
    private Collider2D visualObjectTrigger;

    public WeaponClass weaponClass;
    public Color color = Color.white;    
    public List<FactionWeapon> weapons;
    public GameObject visualObject;
    public bool useGravity = true;

    public Weapon currentInstantiatedWeapon { get; private set; }
    private int lastAmmo;
    private bool pickedOnce = false;



    public void Awake()
    {
        visualObjectSpriteRenderer = visualObject.GetComponent<SpriteRenderer>();
        visualObjectTrigger = visualObject.GetComponent<Collider2D>();
        visualObjectSpriteRenderer.color = color;

        if (useGravity)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            visualObject.transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
        }
        else {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            visualObject.transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        }

    }

    public void CharacterPickSelector(Character holderCharacter, FactionWeapon weaponToInstantiate)
    {

        // instantiate weapon
        var currentWeaponObject = Instantiate(weaponToInstantiate.weaponPrefab);
        currentInstantiatedWeapon = currentWeaponObject.GetComponent<Weapon>();
        currentInstantiatedWeapon.NewHolder(holderCharacter, holderCharacter.transform.position, holderCharacter._Collider, holderCharacter.gameObject, holderCharacter.friendFactions);
        currentInstantiatedWeapon.Shooting = true;
        
        // update ammo
        if (!pickedOnce)
        {
            pickedOnce = true;
        }
        else
        {
            currentInstantiatedWeapon.Ammo = lastAmmo;
        }


        // deactivate collision and sprite
        DeactivateBox();
    }


    public void CharacterDropSelector(Vector3 positionDropped)
    {


        // update ammo
        lastAmmo = currentInstantiatedWeapon.Ammo;
        currentInstantiatedWeapon.Shooting = false;
       
        // destroy selector if weapon has 0 ammo
        if (currentInstantiatedWeapon.Ammo <= 0 && !currentInstantiatedWeapon.InfiniteAmmo)
        {
            currentInstantiatedWeapon.DestroyWeapon();
            DestroySelector();
            return;
        }

        // destroy weapon
        currentInstantiatedWeapon.DestroyWeapon();


        // activate collision and sprite
        transform.position = positionDropped;
        ActivateBox();

    }


    public void DestroySelector()
    {
        Destroy(gameObject);
    }


    private void DeactivateBox()
    {
        visualObject.SetActive(false);

    }

    private void ActivateBox()
    {
        visualObject.SetActive(true);
        visualObjectTrigger.enabled = false;
        StartCoroutine(WaitBeforeActivateCollision());

    }

    private IEnumerator WaitBeforeActivateCollision()
    {
        yield return new WaitForSeconds(cooldownToPickAgain);

        visualObjectTrigger.enabled = true;

    }

}


[System.Serializable]
public class FactionWeapon
{
    public CharacterFaction faction;
    public Weapon weaponPrefab;
    public AudioClip weaponAudio;

}