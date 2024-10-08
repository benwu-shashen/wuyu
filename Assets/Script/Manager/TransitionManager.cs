using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TransitionManager : MonoBehaviour
{
    [SceneName]
    public string startSceneName = string.Empty;

    private CanvasGroup fadeCanvasGroup;
    private bool isFade;

    private IEnumerator Start()
    {
        fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
        yield return StartCoroutine(LoadSceneSetActive(startSceneName));
        EventHandler.CallAfterSceneUnloadEvent();
    }

    private void OnEnable()
    {
        EventHandler.TransitionEvent += OnTransitionEvent;
    }

    private void OnDisable()
    {
        EventHandler.TransitionEvent -= OnTransitionEvent;
    }

    private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
    {
        if (!isFade)
            StartCoroutine(Transition(sceneToGo, positionToGo));
    }

    private IEnumerator Transition(string sceneName, Vector3 targetPosition)
    {
        EventHandler.CallBeforeSceneUnloadEvent();
        yield return Fade(1);
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        yield return LoadSceneSetActive(sceneName);
        EventHandler.CallAfterSceneUnloadEvent();
        EventHandler.CallMoveToPosition(targetPosition);
        yield return Fade(0);
    }

    private IEnumerator LoadSceneSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;
        fadeCanvasGroup.blocksRaycasts = true;
        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;
        isFade = false;
    }
}
