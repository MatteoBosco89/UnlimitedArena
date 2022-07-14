using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManager
{
    public class SceneChangeManager : MonoBehaviour
    {

        protected NetManager netManager;
        protected AsyncOperation loadingOperation;
        protected LoadingScreenManager loadingScreenManager;
        protected float loadProgress = 0.0f;
        protected string sceneToLoad;

        public float LoadProgress
        {
            get { return loadProgress; }
        }

        private void Awake()
        {
            netManager = GetComponent<NetManager>();
            loadingScreenManager = GetComponent<LoadingScreenManager>();
        }

        [System.Obsolete]
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void LoadingScreen(string scene)
        {
            sceneToLoad = scene;
            SceneManager.LoadScene("UnlimitedArena_Loading");
            loadingScreenManager.IsLoading = true;
        }

        public void ChangeScene()
        {
            loadingOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            loadProgress = loadingOperation.progress;
            netManager.InGame = true;
        }

        [System.Obsolete]
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (netManager.InGame) netManager.SpawnPlayer();
            else if (loadingScreenManager.IsLoading) loadingScreenManager.Loading();
        }
    }
}




