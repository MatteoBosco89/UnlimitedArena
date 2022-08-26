using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class CollectibleCountdown : MonoBehaviour
    {
        [SerializeField] protected int countdownTimeInSeconds = 30;
        protected Collectible collectible;
        protected float timer = 0;
        protected bool inCooldown = false;

        public void SetCooldown()
        {
            timer = countdownTimeInSeconds;
            inCooldown = true;
            collectible.Deactive();
        }

        public Collectible MyCollectible
        {
            get { return collectible; }
            set { collectible = value; }
        }

        private void FixedUpdate()
        {
            if (!inCooldown) return;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                inCooldown = false;
                collectible.Active();
            }
        }


    }
}

