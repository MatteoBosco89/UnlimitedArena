using System.Collections;
using System.Collections.Generic;
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
        protected int chosenPlayer = 0;
        protected GameObject player;
        protected bool isHost = false;
        protected Dictionary<int, GameObject> connectedPlayer = new Dictionary<int, GameObject>();
        [SerializeField] protected CustomizedPlayer[] players;

        public bool IsHost
        {
            get { return isHost; }
        }

        public int ChosenPlayer
        {
            set { chosenPlayer = value; }
        }

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

        public void SetPvP(bool flag)
        {
            foreach (GameObject o in connectedPlayer.Values) o.GetComponent<PlayerManagerScript>().SetPvP(flag);

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
            player = Instantiate(spawnPrefabs[0], startPositions[0].position, Quaternion.identity);
            NetworkMessage msg = extraMessageReader.ReadMessage<NetworkMessage>();
            player.GetComponent<PlayerManagerScript>().ChosenPlayer = msg.chosenPlayer;
            player.GetComponent<PlayerManagerScript>().ClientId = conn.connectionId;
            //Debug.LogError("Conn id " + conn.connectionId);
            if (isHost) connectedPlayer[conn.connectionId] = player; 
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            NetworkMessage msg = new NetworkMessage();
            msg.chosenPlayer = PlayerPrefs.GetInt("character");
            ClientScene.AddPlayer(conn, 0, msg);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            connectedPlayer.Remove(conn.connectionId);
        }

        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            //base.OnClientSceneChanged(conn);
        }

    }
}

