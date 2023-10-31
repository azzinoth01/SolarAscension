using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SolarAscension
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader _instance;
        public static SceneLoader Instance { get { return _instance; } }

        private bool _loading;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void LoadScene(int index)
        {
            if (_loading)
            {
                return;
            }
            _loading = true;

            LoadingScreenHandler.Root.SetActive(true);
            StartCoroutine(LoadSceneAsync(index));
        }

        public void LoadScene(string name)
        {
            if (_loading)
            {
                return;
            }
            _loading = true;

            LoadingScreenHandler.Root.SetActive(true);
            StartCoroutine(LoadSceneAsync(name));
        }

        IEnumerator LoadSceneAsync(int index)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                LoadingScreenHandler.SetProgress(asyncOperation.progress);
                if (asyncOperation.progress >= 0.9f)
                {
                    break;
                }
                yield return null;
            }
            LoadingScreenHandler.SetProgress(1);

            yield return new WaitForSeconds(0.2f);

            LoadingScreenHandler.Root.SetActive(false);
            asyncOperation.allowSceneActivation = true;
            _loading = false;
        }

        IEnumerator LoadSceneAsync(string name)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                LoadingScreenHandler.SetProgress(asyncOperation.progress);
                if (asyncOperation.progress >= 0.9f)
                {
                    break;
                }
                yield return null;
            }
            LoadingScreenHandler.SetProgress(1);

            yield return new WaitForSeconds(0.2f);

            LoadingScreenHandler.Root.SetActive(false);
            asyncOperation.allowSceneActivation = true;
            _loading = false;
        }
    }
}