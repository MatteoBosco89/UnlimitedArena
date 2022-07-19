using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Character;

namespace GameManager
{
    public class NetManager : NetworkManager
    {
        protected SceneChangeManager scm;
        protected bool inGame = false;
        protected ConsumableSpawnManager consumableSpawnManager;
        protected GameObject csm;
        protected int chosenPlayer = 0; //index of the chosen character
        protected GameObject player;
        protected bool isHost = false;
        [SerializeField] protected CustomizedPlayer[] players;

        public bool InGame
        {
            get { return inGame; }
            set { inGame = value; }
        }

        public GameObject Player
        {
            get { return player; }
        }

        public ConsumableSpawnManager ConsumableManager
        {
            get { return consumableSpawnManager; }
            set { consumableSpawnManager = value; }
        }
        
        void Start()
        {
            scm = GetComponent<SceneChangeManager>();
        }
            
        private string ChooseMap()
        {
            // choose the map with RNG
            return "UnlimitedArena_WareHouseScene";
        }

        public void StartAsHost()
        {
            isHost = true;
            StartGame();
        }

        public void StartAsClient()
        {
            isHost = false;
            StartGame();
        }

        public void StartGame()
        {
            scm.LoadingScreen(ChooseMap());
        }

        [System.Obsolete]
        public void SpawnPlayer()
        {
            inGame = false;
            if (isHost) StartHost();
            else StartClient();
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


        [System.Serializable]
        public class CustomizedPlayer
        {
            public GameObject playerModel;
        }


        public class NetworkMessage : MessageBase
        {
            public int chosenPlayer;
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {
            Debug.LogError("INSTANTIATE");
            player = Instantiate(spawnPrefabs[0], startPositions[0].position, Quaternion.identity);
            GameObject model = Instantiate(players[PlayerPrefs.GetInt("character")].playerModel, player.transform.position, player.transform.rotation, player.transform);
            //thisCharWeapon = SearchByTag(thisChar, "WeaponContainer");
            //weaponManager.WeaponContainer = thisCharWeapon;
            //weaponManager.Spawn();
            player.GetComponent<PlayerManagerScript>().SpawnedChar = model;
            player.GetComponent<PlayerManagerScript>().YPos = -1.0f;
            Debug.LogError("BEFORE ADD");
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            Debug.LogError("AFTER ADD");
            //SetConsumableSpawnManager();
            //SpawnConsumables();
        }
        public override void OnClientConnect(NetworkConnection conn)
        {
            Debug.LogError("CLIENT CONNECT");
            NetworkMessage msg = new NetworkMessage();
            ClientScene.AddPlayer(conn, 0, msg);
        }

        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            //base.OnClientSceneChanged(conn);
        }

    }
}

