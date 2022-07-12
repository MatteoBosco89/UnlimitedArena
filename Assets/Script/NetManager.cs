using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace GameManager
{
    [System.Obsolete]
    public class NetManager : MonoBehaviour
    {
        protected SceneChangeManager scm;
        protected NetworkManager networkManager;
        protected bool inGame = false;

        public bool InGame
        {
            get { return inGame; }
            set { inGame = value; }
        }

        private void Awake()
        {
            networkManager = GetComponent<NetworkManager>();
            scm = GetComponent<SceneChangeManager>();
        }

        private string ChooseMap()
        {
            // choose the map with RNG
            return "WareHouseScene";
        }

        public void StartGame()
        {
           scm.LoadingScreen(ChooseMap());
        }

        public void SpawnPlayer()
        {
            inGame = false;
            networkManager.StartHost();

        }
        
    }
}

