using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class ConsumableSpawnManager : MonoBehaviour
    {
        protected NetManager netManager;
        [SerializeField] protected List<ConsumableSpawner> spawners = new List<ConsumableSpawner>();
        protected Dictionary<int, ConsumableSpawner> consumables = new Dictionary<int, ConsumableSpawner>();

        public NetManager NetHandler
        {
            get { return netManager; }
            set { netManager = value; }
        }

        public void SpawnAll()
        {
            foreach(ConsumableSpawner cs in spawners)
            {
                cs.SpawnConsumable();
                consumables[cs.SpawnedObject.GetInstanceID()] = cs;
            }
        }

        public void Cooldown(int id)
        {
            consumables[id].SetCooldown();
        }

    }
}

