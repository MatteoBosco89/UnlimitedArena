using UnityEngine;
using UnityEngine.Networking;


namespace GameManager
{
    public class NetManager : MonoBehaviour
    {
        protected SceneChangeManager scm;
        [System.Obsolete]
        protected NetworkManager networkManager;
        protected bool inGame = false;

        public bool InGame
        {
            get { return inGame; }
            set { inGame = value; }
        }

        [System.Obsolete]
        private void Awake()
        {
            networkManager = GetComponent<NetworkManager>();
            scm = GetComponent<SceneChangeManager>();
        }

        private string ChooseMap()
        {
            // choose the map with RNG
            return "UnlimitedArena_WareHouseScene";
        }

        public void StartGame()
        {
           scm.LoadingScreen(ChooseMap());
        }

        [System.Obsolete]
        public void SpawnPlayer()
        {
            inGame = false;
            networkManager.StartHost();

        }
        
    }
}

