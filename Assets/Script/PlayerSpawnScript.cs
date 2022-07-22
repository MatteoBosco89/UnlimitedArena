using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameManager
{
    public class PlayerSpawnScript : MonoBehaviour
    {

        [SerializeField] protected List<GameObject> spawnPointsList;
        protected List<Vector3> spawnPositions;

        private void Awake()
        {
            spawnPositions = new List<Vector3>();
            foreach (GameObject o in spawnPointsList) spawnPositions.Add(o.transform.position);
        }
        public List<Vector3> SpawnPositions
        {
            get { return spawnPositions; }
        }

        public List<GameObject> PlayerSpawners
        {
            get { return spawnPointsList; }
        }
    }
}

