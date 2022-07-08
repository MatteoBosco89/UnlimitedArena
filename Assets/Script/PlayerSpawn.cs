using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSelection
{
    public class PlayerSpawn : MonoBehaviour
    {
        protected static GameObject player;

        public GameObject Player
        {
            get { return player; }
            set { player = value; }
        }

        public void Spawn()
        {
            player = Instantiate(Player, new Vector3(800, 10, 400), Quaternion.identity);
        }

    }
}

