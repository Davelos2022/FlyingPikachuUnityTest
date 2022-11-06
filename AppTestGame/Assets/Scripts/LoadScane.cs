using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScane : MonoBehaviour
{
    AsyncOperation asyncOperation;
    public Slider loadingBar;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        loadingBar.value = 0;
        yield return new WaitForSeconds(1f);
        asyncOperation = SceneManager.LoadSceneAsync(1);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingBar.value = progress;
            yield return null;
        }
    }
}
