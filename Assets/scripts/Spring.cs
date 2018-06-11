using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {

    public float springForce;
    public Vector2 springDirection;

    public float characterCooldown = 0.2f;

    private class CharacterCooldown
    {
        public Character character;
        public float timer;

        public CharacterCooldown(Character character, float timer)
        {
            this.character = character;
            this.timer = timer;
        }
    }
    private List<CharacterCooldown> cooldownCharacters = new List<CharacterCooldown>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        // check if other has charactercomponent
        Character character = collision.attachedRigidbody.GetComponent<Character>();
        if (!character)
            return;

        // check if character in cooldown
        foreach (var item in cooldownCharacters)
        {
            if (item.character == character)
                return;
        }

        // if not in cooldown
        /**
        character.Spring(springDirection.normalized * springForce);
        cooldownCharacters.Add(new CharacterCooldown(character, characterCooldown));
        **/

    }


    private void Update()
    {
        // decrease dictionary cooldown
        DecreaseTimers();
    }

    private void DecreaseTimers()
    {
        var cooldownCharactersArray = cooldownCharacters.ToArray();
        foreach (var item in cooldownCharactersArray)
        {
            item.timer -= Time.deltaTime;

            if (item.timer <= 0f)
            {
                cooldownCharacters.Remove(item);
            }
        }
    }
}
