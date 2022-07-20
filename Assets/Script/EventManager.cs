using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] protected bool pvp = true;
        protected bool currPvp;

        private void Awake()
        {
            currPvp = pvp;
        }

        private void Update()
        {
            if (currPvp != pvp)
            {
                currPvp = pvp;
                GetComponent<NetManager>().SetPvP(currPvp);
            }
        }
    }
}
