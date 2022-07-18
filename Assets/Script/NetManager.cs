using System.Collections;
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
        protected ConsumableSpawnManager consumableSpawnManager;
        protected GameObject csm;

        public bool InGame
        {
            get { return inGame; }
            set { inGame = value; }
        }

        public ConsumableSpawnManager ConsumableManager
        {
            get { return consumableSpawnManager; }
            set { consumableSpawnManager = value; }
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
            networkManager.StartClient();
            SetConsumableSpawnManager();
            SpawnConsumables();
        }

        public void ConsumablePickedUp(GameObject consumable)
        {
            consumable.SetActive(false);
            consumableSpawnManager = csm.GetComponent<ConsumableSpawnManager>();
            consumableSpawnManager.Cooldown(consumable.GetInstanceID());
        }

        public void SpawnConsumables()
        {
            consumableSpawnManager.SpawnAll();
        }
        
        public void SetConsumableSpawnManager()
        {
            csm = GameObject.FindGameObjectWithTag("ConsumableSpawnManager");
            consumableSpawnManager = csm.GetComponent<ConsumableSpawnManager>();
            consumableSpawnManager.NetHandler = gameObject.GetComponent<NetManager>();
        }

        

    }
}

