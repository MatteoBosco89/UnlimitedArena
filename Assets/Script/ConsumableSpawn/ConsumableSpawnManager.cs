using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameManager
{
    public class ConsumableSpawnManager : MonoBehaviour
    {
        [SerializeField] protected int interval = 1;
        protected List<Collectible> collectibles = new List<Collectible>();
        protected float timer = 0;

        public void AddCollectible(Collectible c)
        {
            collectibles.Add(c);
        }

        public void RemoveCollectible(Collectible c)
        {
            collectibles.Remove(c);
        }

        public void CheckIsActive()
        {
            collectibles.RemoveAll(item => item.CooldownFinished == true);
        }

        protected void DoCountdown()
        {
            foreach (Collectible c in collectibles) c.DoCooldown();
        }

        private void FixedUpdate()
        {
            if (collectibles.Count == 0) return;
            CheckIsActive();
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = interval;
                DoCountdown();
            }
        }

    }
}

