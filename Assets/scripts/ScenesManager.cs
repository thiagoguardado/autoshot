using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScenesManager : MonoBehaviour {


    private static ScenesManager _Instance;
    public static ScenesManager Instance
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

    public const string _ResourceName = "ScenesManager";
    

    private static void CreateInstance()
    {
        var prefab = Resources.Load<GameObject>(_ResourceName);
        var instance = Instantiate(prefab);
        instance.name = "_" + _ResourceName;
        instance.transform.SetAsFirstSibling();
        _Instance = instance.GetComponent<ScenesManager>();
    }


    public Image fadingPanel;
    public Color fadeColor;
    public float fadeDuration;
    private Coroutine fadingCoroutine;
    private bool isTransitioning;

 

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public bool TransitionToScene(string nextScene)
    {

        if (isTransitioning)
            return false;

        if (fadingCoroutine != null)
            StopCoroutine(fadingCoroutine);

        isTransitioning = true;
        fadingCoroutine = StartCoroutine(LoadScene(nextScene, () => isTransitioning = false));

        return true;
    }

    IEnumerator LoadScene(string nextScene, Action endAction = null)
    {

        Scene currenetScene = SceneManager.GetActiveScene();

        GameManager.Instance.NotifySceneStartedChanging();

        yield return StartCoroutine(Fade(fadeDuration, true));

        AsyncOperation loadingAsync = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        while (!loadingAsync.isDone)
            yield return null;

        AsyncOperation unloadingAsync = SceneManager.UnloadSceneAsync(currenetScene);

        while (!unloadingAsync.isDone)
            yield return null;

        GameManager.Instance.NotifySceneFinishedChanging(nextScene);

        yield return StartCoroutine(Fade(fadeDuration, false));

        if (endAction != null)
            endAction.Invoke();

        Destroy();

    }

    IEnumerator Fade(float duration, bool toAlphaOne) {

        float timer = 0f;
        Color color = fadeColor;

        while (timer <= duration)
        {
            color.a = (toAlphaOne ? (timer / duration) : (1 - timer / duration));
            fadingPanel.color = color;

            timer += Time.deltaTime;

            yield return null;
        }

        color.a = (toAlphaOne ? 1 : 0);
        fadingPanel.color = color;
    }

    private void Destroy()
    {
        _Instance = null;
        Destroy(gameObject);

    }

}
