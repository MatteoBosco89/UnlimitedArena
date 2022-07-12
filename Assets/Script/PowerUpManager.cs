using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PowerUpManager : MonoBehaviour
    {
        [SerializeField] protected GameObject _playerManager;
        [SerializeField] protected List<GameObject> powerups;
        protected string countdown = "CountDown";
        protected Dictionary<string, int> timeRemain = new Dictionary<string, int>();
        protected Dictionary<string, PowerupHandler> handlers = new Dictionary<string, PowerupHandler>();
        protected List<string> powerupsId = new List<string>();
        protected PowerupHandler powerupHandler;

        public int GetTimeRemaining(string id)
        {
            return timeRemain[id];
        }

        public float GetAura(string id)
        {
            PowerupHandler p = handlers[id];
            return p.Multiplier;
        }

        private void Start()
        {
            // vedere bene fa errore se modifica on the fly
            foreach(GameObject p in powerups)
            {
                string id = GetIdFromGameObject(p);
                timeRemain[id] = 0;
                powerupsId.Add(id);
                handlers[id] = p.GetComponent<PowerupHandler>();
            }
            InvokeRepeating(countdown, 0.1f, 1);
        }

        private string GetIdFromGameObject(GameObject o)
        {
            powerupHandler = o.GetComponent<PowerupHandler>();
            return powerupHandler.Id;
        }

        private int GetModifierFromGameObject(GameObject o)
        {
            powerupHandler = o.GetComponent<PowerupHandler>();
            return powerupHandler.ModifierTime;
        }

        public void PowerUpPickup(GameObject gameObject)
        {
            timeRemain[GetIdFromGameObject(gameObject)] = GetModifierFromGameObject(gameObject);
        }

        protected void CountDown()
        {
            foreach (string p in powerupsId)
            {
                if (timeRemain[p] > 0) timeRemain[p] -= 1;
            }
        }

    }
}

