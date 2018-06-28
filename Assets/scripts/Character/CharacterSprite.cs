using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour {

    private Animator _Animator;
    Character _Character;
    public CharacterSpriteAnimations animations = new CharacterSpriteAnimations();

    void Awake()
    {
        _Character = transform.parent.GetComponent<Character>();
        _Animator = GetComponent<Animator>();
    }

    public void Play(int animationHash)
    {
        _Animator.Play(animationHash);
    }

    public void DestroyObject()
    {
        Debug.Log("1");
        _Character.DestroyCharacter();
    }

    public class CharacterSpriteAnimations
    {
        public readonly int walk = Animator.StringToHash("walk");
        public readonly int idle = Animator.StringToHash("idle");
        public readonly int dead = Animator.StringToHash("dead");
        public readonly int stun = Animator.StringToHash("stun");
        public readonly int jump = Animator.StringToHash("jump");
        public readonly int fall = Animator.StringToHash("fall");
    }

}
