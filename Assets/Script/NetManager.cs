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
        [SerializeField] protected GameObject playerManagerPrefab;
        protected int currentSpawnPoint = 0;
        protected NetworkClient myClient;
        protected PlayerManagerScript playerManager;
        protected List<Vector3> spawnPoints;
        protected List<GameObject> playerSpawners;
        protected PlayerSpawnScript pss;
        protected int clientCounter = 0;

        public List<Vector3> SpawnPositionsList
        {
            get { return spawnPoints; }
        }

        public PlayerManagerScript PManager
        {
            get { return playerManager; }
            set { playerManager = value; }
        }

        public NetworkClient MyClient
        {
            get { return myClient; }
            set { myClient = value; }
        }

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
            PlayerPrefs.SetString("SCORES", "");
            scm = GetComponent<SceneChangeManager>();
            //LogFilter.currentLogLevel = 0;
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
            RegisterServerHandlers();
        }

        protected void RegisterServerHandlers()
        {
            NetworkServer.RegisterHandler(UAMess.MSG_HOST_SIGNAL_DEATH, OnSignalDeath);
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
            pss = GameObject.FindGameObjectWithTag("PlayerSpawnManager").GetComponent<PlayerSpawnScript>();
            LoadSpawnPositions();
            inGame = false;
            if (isHost) myClient = StartHost();
            else myClient = StartClient();
        }

        protected void LoadSpawnPositions()
        {
            spawnPoints = pss.SpawnPositions;
            playerSpawners = pss.PlayerSpawners;
        }

        public void ConsumablePickedUp(GameObject consumable)
        {
            consumable.SetActive(false);
            //consumableSpawnManager = csm.GetComponent<ConsumableSpawnManager>();
            //consumableSpawnManager.Cooldown(consumable.GetInstanceID());
        }

        public void SetPvP(bool flag)
        {
            foreach (GameObject o in connectedPlayer.Values) o.GetComponent<PlayerManagerScript>().SetPvP(flag);
        }

        public void SignalDeath(int conn)
        {
            NetworkSignalDeathMessage msg = new NetworkSignalDeathMessage();
            msg.connectionId = conn;
            myClient.Send(UAMess.MSG_HOST_SIGNAL_DEATH, msg);
        }

        public void SetNextSpawn()
        {
            NetworkPositionMessage msg = new NetworkPositionMessage();
            msg.position = RandomSpawnPoint();
            foreach (NetworkConnection o in NetworkServer.connections)
            {
                Debug.LogError(o.connectionId);
                o.Send(UAMess.MSG_NEW_SPAWN_POSITION, msg);
                o.FlushChannels();
            }
        }

        [System.Serializable]
        public class CustomizedPlayer
        {
            public GameObject playerModel;
        }

        public class NetworkSignalDeathMessage : MessageBase
        {
            public int connectionId;
        }

        public class NetworkPositionMessage : MessageBase
        {
            public Vector3 position;
        }
        [System.Serializable]
        public class NetworkHeroSelectionMessage : MessageBase
        {
            public int chosenPlayer;
        }

        public class UAMess
        {
            public static short MSG_NEW_SPAWN_POSITION = 1000;
            public static short MSG_PVP = 1005;
            public static short MSG_HOST_SIGNAL_DEATH = 1010;
        }

        public Vector3 SpawnPoint()
        {
            Vector3 p = startPositions[currentSpawnPoint].position;
            currentSpawnPoint++;
            if (currentSpawnPoint >= startPositions.Count) currentSpawnPoint = 0;
            return p;
        }

        public GameObject SSpawnPoint()
        {
            GameObject spanwer = playerSpawners[currentSpawnPoint];
            currentSpawnPoint++;
            if (currentSpawnPoint >= playerSpawners.Count) currentSpawnPoint = 0;
            return spanwer;
        }

        public Vector3 RandomSpawnPoint()
        {
            return spawnPoints[Random.Range(0, spawnPoints.Count)];
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {
            player = Instantiate(playerManagerPrefab, SpawnPoint(), Quaternion.identity);
            NetworkHeroSelectionMessage msg = extraMessageReader.ReadMessage<NetworkHeroSelectionMessage>();
            player.GetComponent<PlayerManagerScript>().ChosenPlayer = msg.chosenPlayer;
            player.GetComponent<PlayerManagerScript>().ClientId = conn.connectionId;
            player.GetComponent<PlayerManagerScript>().PlayerName = "Player " + conn.connectionId;
            if (isHost) connectedPlayer[conn.connectionId] = player;
            NetworkServer.AddPlayerForConnection(conn, player, 0);
        }

        public void PlayerWasKilled(int connectionId)
        {
            if (!isHost) return;
            NetworkConnection conn = NetworkServer.connections[connectionId];
            GameObject player = conn.playerControllers[0].gameObject;
            var newPlayer = Instantiate(playerManagerPrefab, SpawnPoint(), Quaternion.identity);
            newPlayer.GetComponent<PlayerManagerScript>().ChosenPlayer = player.GetComponent<PlayerManagerScript>().ChosenPlayer;
            newPlayer.GetComponent<PlayerManagerScript>().ClientId = connectionId;
            newPlayer.GetComponent<PlayerManagerScript>().PlayerName = player.GetComponent<PlayerManagerScript>().PlayerName;
            Destroy(player);
            connectedPlayer[connectionId] = newPlayer;
            NetworkServer.ReplacePlayerForConnection(conn, newPlayer, 0);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            NetworkHeroSelectionMessage msg = new NetworkHeroSelectionMessage();
            msg.chosenPlayer = PlayerPrefs.GetInt("character");
            ClientScene.AddPlayer(conn, 0, msg);
            myClient.RegisterHandler(UAMess.MSG_NEW_SPAWN_POSITION, OnSetSpawnPosition);
        }

        private void OnSetSpawnPosition(NetworkMessage netMsg)
        {
            NetworkPositionMessage msg = netMsg.ReadMessage<NetworkPositionMessage>();
            playerManager.TheSpawnPosition = msg.position;
            Debug.LogError(msg.position);
        }

        private void OnSignalDeath(NetworkMessage netMsg)
        {
            NetworkSignalDeathMessage msg = netMsg.ReadMessage<NetworkSignalDeathMessage>();
            PlayerWasKilled(msg.connectionId);
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

