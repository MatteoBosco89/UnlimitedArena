using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerSelection
{
    public class NetManager : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> characters;
        protected GameObject player;
        protected PlayerSpawn ps;

        private void Awake()
        {
            ps = GetComponent<PlayerSpawn>();
        }

        public GameObject Player
        {
            get { return player; }
        }

        public void ChooseMe(string ch)
        {
            if (ch == "Cube") ps.Player = characters[0];
            if (ch == "Sphere") ps.Player = characters[1];

            ps.Spawn();
        }

    }
}

