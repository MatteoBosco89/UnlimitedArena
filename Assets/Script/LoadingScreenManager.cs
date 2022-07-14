using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class LoadingScreenManager : MonoBehaviour
    {
        [SerializeField] protected GameObject loadingScreen;
        protected bool isLoading = false;
        protected GameObject thisLoadingScreen;
        protected SceneChangeManager sceneChangeManager;

        private void Start()
        {
            sceneChangeManager = GetComponent<SceneChangeManager>();
        }

        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; }
        }

        public void Loading()
        {
            StartCoroutine(WaitForLoad());
        }

        IEnumerator WaitForLoad()
        {
            thisLoadingScreen = Instantiate(loadingScreen);
            isLoading = false;
            yield return new WaitForSeconds(1);
            sceneChangeManager.ChangeScene();
            
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(sceneChangeManager.LoadProgress);
        }
    }
}

