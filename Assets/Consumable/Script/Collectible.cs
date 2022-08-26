using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GameManager
{
    public class Collectible : NetworkBehaviour
    {
        [SerializeField] protected int cooldownInSeconds = 30;
        [SerializeField] protected ConsumableSpawnManager spawnManager;
        [SerializeField] protected float reduction = 1;
        protected float timer = 0;
        protected bool cooldownFinished = true;

        public bool CooldownFinished
        {
            get { return cooldownFinished; }
        }

        private void OnTriggerEnter(Collider other)
        { 
            Deactive();
        }

        public void Active()
        {
            gameObject.SetActive(true);
        }

        public void Deactive()
        {
            cooldownFinished = false;
            timer = cooldownInSeconds;
            spawnManager.AddCollectible(this);
            gameObject.SetActive(false);
        }

        public void DoCooldown()
        {
            timer -= reduction;
            if (timer <= 0)
            {
                cooldownFinished = true;
                Active();
            }
        }
    }
}

