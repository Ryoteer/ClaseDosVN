using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncSceneLoader : MonoBehaviour
{
    [Header("<color=orange>Inputs</color>")]
    [SerializeField] private KeyCode _loadSceneKey = KeyCode.Space;

    [Header("<color=orange>UI</color>")]
    [SerializeField] private Image _fillBar;
    [SerializeField] private Color _loadingColor = Color.red;
    [SerializeField] private Color _doneColor = Color.green;

    public void LoadSceneAsync(string scene)
    {
        StartCoroutine(AsyncLoad(scene));
    }

    private IEnumerator AsyncLoad(string scene)
    {
        _fillBar.fillAmount = 0f;
        _fillBar.color = _loadingColor;

        float progress = 0f;

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);

        asyncOp.allowSceneActivation = false;

        while(asyncOp.progress < .9f)
        {
            progress = asyncOp.progress / .9f;

            Debug.Log($"Progress: {progress}.");

            _fillBar.fillAmount = progress;

            yield return null;
        }

        _fillBar.fillAmount = 1f;
        _fillBar.color = _doneColor;

        while (!Input.GetKeyDown(_loadSceneKey))
        {
            yield return null;
        }

        asyncOp.allowSceneActivation = true;
    }
}
