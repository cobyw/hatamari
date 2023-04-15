using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool canChangeScenesOnStart = true;
    [SerializeField] private bool canChangeScenes = false;
    [SerializeField] private Image image;

    [SerializeField] private string nextScene = "GameScene";

    public void Start()
    {
        if (image == null)
        {
            Debug.LogError("No Transition image set in introMgr");
        }

        canChangeScenes = canChangeScenesOnStart;
        StartCoroutine(TransitionIn());
    }

    public void EnableSceneChange(string sceneToLoad = "")
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            nextScene = sceneToLoad;
        }

        canChangeScenes = true;
    }

    public void ChangeScene()
    {
        if (canChangeScenes)
        {
            StartCoroutine(TransitionOut(nextScene));
        }
    }

    public void ForceChangeScene(string sceneToLoad)
    {
        StartCoroutine(TransitionOut(sceneToLoad));
    }

    IEnumerator TransitionIn()
    {
        Color color = image.color;
        for (float alpha = 1f; alpha >= 0; alpha -= Time.deltaTime)
        {
            color.a = alpha;
            image.color = color;

            yield return null;
        }
    }

    IEnumerator TransitionOut(string sceneName)
    {
        Color color = image.color;
        for (float alpha = 0f; alpha <= 1; alpha += Time.deltaTime)
        {
            color.a = alpha;
            image.color = color;

            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }

}
