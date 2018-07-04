using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour {

    private Animator _Animator;
    Character _Character;
    public CharacterSpriteAnimations animations = new CharacterSpriteAnimations();
    public AnimationCurve RecoilCurve;
    private Coroutine _RecoilCoroutine = null;
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



    public void Recoil(Vector2 direction)
    {
        if(_RecoilCoroutine != null)
        {
            StopCoroutine(_RecoilCoroutine);
            _RecoilCoroutine = null;
        }
        _RecoilCoroutine = StartCoroutine(RecoilRoutine(direction));
    }

    IEnumerator RecoilRoutine(Vector2 direction)
    {
        float recoilTime = 0.1f;
        float recoilMagnitude = 0.25f;
        
        Vector2 targetPosition = direction * recoilMagnitude;
        float timeout = recoilTime;
        while (timeout > 0)
        {
            float t = 1 - (timeout / recoilTime);
            transform.localPosition = Vector2.Lerp(Vector2.zero, targetPosition, RecoilCurve.Evaluate(t));
            timeout -= Time.deltaTime;
            yield return null;
        }
        yield return null;
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
