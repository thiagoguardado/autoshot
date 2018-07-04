using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    PlayerInput Player;
    public float Speed = 1;
    public float ShakeMagnitude = 0.1f;
    public float ShakeTime = 0.1f;
    Coroutine _ScreenShakeCoroutine;
    

    void Awake()
    {
        Player = FindObjectOfType<PlayerInput>();
        GameManager.Instance.OnRequestScreenshake += ScreenShake;
    }
    void OnDestroy()
    {
        GameManager.Instance.OnRequestScreenshake -= ScreenShake;
    }

	// Update is called once per frame
	void Update () {
        
        Vector3 pos = transform.position;
        pos = Vector2.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime);
        pos.z = transform.position.z;
        transform.position = pos;
	}

    public void ScreenShake()
    {
        if(_ScreenShakeCoroutine != null)
        {
            StopCoroutine(_ScreenShakeCoroutine);
            _ScreenShakeCoroutine = null;
        }

        _ScreenShakeCoroutine = StartCoroutine(ScreenShakeCoroutine());
    }

    IEnumerator ScreenShakeCoroutine()
    {
        Vector2 shakeDirection = new Vector2();
        
        float timeout = ShakeTime;
        while (timeout > 0)
        {
            shakeDirection.x = Random.Range(-1.0f, 1.0f);
            shakeDirection.y = Random.Range(-1.0f, 1.0f);
            transform.position += (Vector3)shakeDirection * ShakeMagnitude;
            timeout -= Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
