using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour
{
    public Slider slider;
    AsyncOperation async_operation;

    void Start()
    {
        StartCoroutine(StartLoad());
    }

    public IEnumerator StartLoad()
    {
        yield return new WaitForSeconds(1f);
        async_operation = GameManager.Instance.LoadChatScene();
        async_operation.allowSceneActivation = false;

        while (async_operation.progress < .9f)
        {
            slider.value = async_operation.progress;
            yield return new WaitForEndOfFrame();
        }

        slider.value = 1f;

        async_operation.allowSceneActivation = true;
    }
}
